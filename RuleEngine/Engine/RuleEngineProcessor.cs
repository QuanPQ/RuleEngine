using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using RuleEngine.Engine.Models;
using RuleEngine.Exceptions;
using RuleEngine.Models;

namespace RuleEngine.Engine;

public class RuleEngineProcessor
{
    private readonly RuleEngineContext _db;
    private readonly RuleConditionEvaluator _conditionEvaluator;
    private readonly RuleActionExecutor _actionExecutor;
    private readonly IMemoryCache _cache;
    private readonly ILogger<RuleEngineProcessor> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public RuleEngineProcessor(
        RuleEngineContext db,
        RuleConditionEvaluator conditionEvaluator,
        RuleActionExecutor actionExecutor,
        IMemoryCache cache,
        ILogger<RuleEngineProcessor> logger)
    {
        _db = db;
        _conditionEvaluator = conditionEvaluator;
        _actionExecutor = actionExecutor;
        _cache = cache;
        _logger = logger;
    }

    public RuleContext Evaluate(Guid applicationId, string targetType, string targetCode, Dictionary<string, object> data)
    {
        var context = new RuleContext(data)
        {
            TargetType = targetType,
            TargetCode = targetCode
        };

        var cacheKey = $"rules:{applicationId}";
        var rules = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return _db.RuleAssignmentViews
                .Where(r => r.ApplicationId == applicationId && r.TargetType == targetType && r.TargetCode == targetCode
                            && r.IsActived && !r.IsDeleted)
                .OrderBy(r => r.Priority)
                .ToList();
        })!;

        foreach (var rule in rules)
            context.RuleIndex[rule.RuleId] = rule;

        _logger.LogInformation("Loaded {Count} active rules for {TargetType}/{TargetCode}",
            rules.Count, targetType, targetCode);

        foreach (var rule in rules)
        {
            EvaluateRule(rule, context);
            if (rule.StopOnFirstFail && context.HasViolations)
            {
                _logger.LogInformation("Stopping: rule '{RuleId}' has StopOnFirstFail=true", rule.RuleId);
                break;
            }
        }

        FlushLogs(context);

        return context;
    }

    public void EvaluateRule(RuleAssignmentView rule, RuleContext context)
    {
        var ruleId = rule.RuleId;

        if (!context.EvaluatedRules.Add(ruleId))
        {
            _logger.LogWarning("Circular trigger detected for rule '{RuleId}', skipping", ruleId);
            return;
        }

        var startTime = DateTime.UtcNow;
        bool conditionResult = false;
        var executedActions = new List<string>();
        string? errorMessage = null;

        try
        {
            var condition = LoadCondition(ruleId);
            conditionResult = _conditionEvaluator.Evaluate(condition, context.Data);

            _logger.LogDebug("Rule '{RuleId}' condition result: {Result}", ruleId, conditionResult);

            if (conditionResult)
            {
                var actions = LoadActions(ruleId);
                foreach (var action in actions)
                {
                    var triggeredRuleId = _actionExecutor.Execute(action, context, ruleId);
                    executedActions.Add(action.Type.ToString());

                    if (triggeredRuleId != null)
                        TriggerRule(triggeredRuleId, context);
                }
            }
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
            context.HasErrors = true;
            _logger.LogError(e, "Error evaluating rule '{RuleId}'", ruleId);
        }
        finally
        {
            var fullError = BuildErrorMessage(errorMessage, context.ExecutionErrors);
            if (rule.EnableLog || fullError != null)
            {
                SaveLog(rule.RuleAssignmentId, context, conditionResult, executedActions, fullError, startTime);
            }
            context.ExecutionErrors.Clear();
        }
    }

    private void TriggerRule(string ruleId, RuleContext context)
    {
        if (context.RuleIndex.TryGetValue(ruleId, out var rule) && rule.IsActived && !rule.IsDeleted)
        {
            _logger.LogInformation("Triggering rule '{RuleId}'", ruleId);
            EvaluateRule(rule, context);
        }
        else
        {
            _logger.LogWarning("Triggered rule '{RuleId}' not found or inactive", ruleId);
        }
    }

    private RuleConditionModel? LoadCondition(string ruleId)
    {
        return _cache.GetOrCreate($"cond:{ruleId}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            var entity = _db.RuleConditions
                .FirstOrDefault(c => c.RuleId == ruleId && c.IsActived && !c.IsDeleted);
            if (entity == null) return null;
            return JsonSerializer.Deserialize<RuleConditionModel>(entity.ConditionJson, JsonOptions);
        });
    }

    private List<RuleActionModel> LoadActions(string ruleId)
    {
        return _cache.GetOrCreate($"actions:{ruleId}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            var entities = _db.RuleActions
                .Where(a => a.RuleId == ruleId && a.IsActived && !a.IsDeleted)
                .OrderBy(a => a.ActionOrder)
                .ToList();

            return entities.Select(e =>
            {
                var action = JsonSerializer.Deserialize<RuleActionModel>(e.ActionJson, JsonOptions) ?? new RuleActionModel();
                action.Type = Enum.Parse<ActionType>(e.ActionType);
                action.ActionOrder = e.ActionOrder;
                return action;
            }).ToList();
        })!;
    }

    private void SaveLog(Guid ruleAssignmentId, RuleContext context, bool conditionResult,
        List<string> executedActions, string? errorMessage, DateTime startTime)
    {
        try
        {
            var log = new RuleExecutionLog
            {
                RuleExecutionLog1 = Guid.NewGuid(),
                RuleAssignmentId = ruleAssignmentId,
                RequestId = context.RequestId,
                Input = JsonSerializer.Serialize(context.Data),
                ConditionResult = conditionResult,
                ActionsExecuted = JsonSerializer.Serialize(executedActions),
                ErrorMessage = errorMessage,
                ExecutionTimeMs = (int)(DateTime.UtcNow - startTime).TotalMilliseconds,
                IsActived = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = Guid.Empty
            };
            context.PendingLogs.Add(log);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create execution log for ruleAssignmentId '{Id}'", ruleAssignmentId);
        }
    }

    private void FlushLogs(RuleContext context)
    {
        if (context.PendingLogs.Count == 0) return;
        try
        {
            _db.RuleExecutionLogs.AddRange(context.PendingLogs);
            _db.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to batch-save {Count} execution logs", context.PendingLogs.Count);
        }
    }

    private static string? BuildErrorMessage(string? fatalError, List<string> executionErrors)
    {
        var parts = new List<string>();
        if (fatalError != null) parts.Add($"[FATAL] {fatalError}");
        parts.AddRange(executionErrors.Select(e => $"[ACTION_ERROR] {e}"));
        return parts.Count > 0 ? string.Join("\n", parts) : null;
    }

    public void EvictAll()
    {
        if (_cache is MemoryCache mc) mc.Compact(1.0);
        _logger.LogInformation("All rule caches cleared");
    }

    public void EvictByApplication(string applicationId)
    {
        _cache.Remove($"rules:{applicationId}");
        _logger.LogInformation("Cache evicted for {ApplicationId}", applicationId);
    }

    //public void EvictByTarget(string applicationId, string targetType, string targetCode)
    //{
    //    _cache.Remove($"rules:{applicationId}:{targetType}:{targetCode}");
    //    _logger.LogInformation("Cache evicted for {ApplicationId}/{TargetType}/{TargetCode}", applicationId, targetType, targetCode);
    //}

    public void EvictByRuleId(string ruleId)
    {
        _cache.Remove($"cond:{ruleId}");
        _cache.Remove($"actions:{ruleId}");
        _logger.LogInformation("Cache evicted for ruleId '{RuleId}'", ruleId);
    }
}
