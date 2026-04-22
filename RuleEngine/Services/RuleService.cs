using RuleEngine.DTOs;
using RuleEngine.Engine;
using RuleEngine.Exceptions;

namespace RuleEngine.Services;

public class RuleService
{
    private readonly RuleEngineProcessor _engine;
    private readonly ILogger<RuleService> _logger;

    public RuleService(RuleEngineProcessor engine, ILogger<RuleService> logger)
    {
        _engine = engine;
        _logger = logger;
    }

    public RuleEvaluationResponseDto Evaluate(Dictionary<string, object> request)
    {
        var targetType = FieldResolver.Resolve(request, "targetType")?.ToString();
        var targetCode = FieldResolver.Resolve(request, "targetCode")?.ToString();

        if (string.IsNullOrWhiteSpace(targetType) || string.IsNullOrWhiteSpace(targetCode))
            throw new RuleEngineException("targetType and targetCode are required");

        _logger.LogInformation("Evaluating rules for {TargetType}/{TargetCode}", targetType, targetCode);

        var context = _engine.Evaluate(targetType, targetCode, request);

        return new RuleEvaluationResponseDto
        {
            RequestId = context.RequestId,
            TargetType = targetType,
            TargetCode = targetCode,
            Valid = !context.HasViolations,
            HasErrors = context.HasErrors,
            ErrorMessage = context.HasErrors
                ? $"Rule evaluation encountered an internal error. Please contact support with requestId: {context.RequestId}"
                : null,
            Violations = context.Violations,
            Data = context.Data
        };
    }

    public void EvictAll() => _engine.EvictAll();
    public void EvictByTarget(string targetType, string targetCode) => _engine.EvictByTarget(targetType, targetCode);
    public void EvictByRuleId(string ruleId) => _engine.EvictByRuleId(ruleId);
}
