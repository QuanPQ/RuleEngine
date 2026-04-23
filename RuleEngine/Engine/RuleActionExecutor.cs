using System.Text.Json;
using System.Text.RegularExpressions;
using DynamicExpresso;
using Microsoft.Data.SqlClient;
using RuleEngine.Engine.Models;
using RuleEngine.Exceptions;

namespace RuleEngine.Engine;

public class RuleActionExecutor
{
    private readonly string _connectionString;
    private readonly ILogger<RuleActionExecutor> _logger;
    private static readonly Regex FieldRefPattern = new(@"\$\{([^}]+)}", RegexOptions.Compiled);
    private static readonly Regex DbFuncPattern = new(
        @"^([A-Za-z_][\w.]*)\((.*)?\)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Interpreter Interpreter;

    static RuleActionExecutor()
    {
        Interpreter = new Interpreter();
        Interpreter.Reference(typeof(Math));
        Interpreter.Reference(typeof(DateTime));
        Interpreter.Reference(typeof(TimeSpan));
        Interpreter.Reference(typeof(Convert));
        Interpreter.Reference(typeof(string));
        Interpreter.Reference(typeof(decimal));
        Interpreter.Reference(typeof(int));
        Interpreter.Reference(typeof(double));
    }

    public RuleActionExecutor(IConfiguration configuration, ILogger<RuleActionExecutor> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _logger = logger;
    }

    public string? Execute(RuleActionModel action, RuleContext context, string currentRuleId)
    {
        return action.Type switch
        {
            ActionType.REQUIRE_FIELD => ExecuteRequireField(action, context, currentRuleId),
            ActionType.REJECT => ExecuteReject(action, context, currentRuleId),
            ActionType.SET_VALUE => ExecuteSetValue(action, context),
            ActionType.TRIGGER_RULE => action.RuleId,
            _ => throw new RuleEngineException($"Unknown action type: {action.Type}")
        };
    }

    private string? ExecuteRequireField(RuleActionModel action, RuleContext context, string ruleId)
    {
        var value = FieldResolver.Resolve(context.Data, action.Field!);
        if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
        {
            var message = action.Message != null
                ? ResolveMessage(action.Message, context.Data)
                : $"Field '{action.Field}' is required";
            var metadata = BuildMetadata(action, context);
            context.Violations.Add(new RuleViolation(ruleId, action.ErrorCode, action.Type.ToString(), message, metadata));
        }
        return null;
    }

    private string? ExecuteReject(RuleActionModel action, RuleContext context, string ruleId)
    {
        var message = ResolveMessage(action.Message, context.Data);
        var metadata = BuildMetadata(action, context);
        context.Violations.Add(new RuleViolation(ruleId, action.ErrorCode, action.Type.ToString(), message, metadata));
        return null;
    }

    private string? ExecuteSetValue(RuleActionModel action, RuleContext context)
    {
        try
        {
            var expression = ResolveFieldReferences(action.Expression!, context.Data);
            var result = EvaluateExpression(expression);
            FieldResolver.SetValue(context.Data, action.Field!, result);
            _logger.LogInformation("SET_VALUE: {Field} = {Result} (expression: {Expr})", action.Field, result, expression);
        }
        catch (Exception e)
        {
            var error = $"SET_VALUE failed | field: {action.Field} | expression: {action.Expression} | error: {e.Message}";
            context.ExecutionErrors.Add(error);
            context.HasErrors = true;
            _logger.LogError(e, "{Error}", error);
        }
        return null;
    }

    private static object? EvaluateExpression(string expression)
    {
        try
        {
            var parameters = new[]
            {
                new Parameter("today", typeof(DateTime), DateTime.Today),
                new Parameter("now", typeof(DateTime), DateTime.Now)
            };
            return Interpreter.Eval(expression, parameters);
        }
        catch (Exception ex)
        {
            throw new RuleEngineException($"Failed to evaluate expression: {expression} | Error: {ex.Message}", ex);
        }
    }

    private Dictionary<string, object>? BuildMetadata(RuleActionModel action, RuleContext context)
    {
        Dictionary<string, object>? result = null;

        if (action.Metadata != null && action.Metadata.Count > 0)
        {
            result = new Dictionary<string, object>(action.Metadata);
            foreach (var key in result.Keys.ToList())
            {
                if (result[key] is string strVal)
                    result[key] = ResolveMessage(strVal, context.Data)!;
            }
        }

        if (!string.IsNullOrWhiteSpace(action.MetadataQuery))
        {
            try
            {
                var dbMetadata = ExecuteMetadataQuery(action.MetadataQuery, context.Data);
                if (dbMetadata != null)
                {
                    result = result == null ? dbMetadata : result.Concat(dbMetadata).ToDictionary(k => k.Key, v => v.Value);
                }
            }
            catch (Exception e)
            {
                var error = $"metadataQuery failed | query: {action.MetadataQuery} | error: {e.Message}";
                context.ExecutionErrors.Add(error);
                context.HasErrors = true;
                _logger.LogError(e, "{Error}", error);
            }
        }

        return result;
    }

    private Dictionary<string, object>? ExecuteMetadataQuery(string funcExpr, Dictionary<string, object> data)
    {
        var sqlParams = new List<SqlParameter>();
        var resolvedFunc = RuleConditionEvaluator.ResolveDbFunctionParams(funcExpr, data, sqlParams);

        var sql = $"SELECT {resolvedFunc}";
        _logger.LogDebug("metadataQuery: {Sql}", sql);

        string? jsonResult;
        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddRange(sqlParams.ToArray());
            conn.Open();
            jsonResult = cmd.ExecuteScalar()?.ToString();
        }

        if (string.IsNullOrWhiteSpace(jsonResult)) return null;

        var trimmed = jsonResult.Trim();
        if (trimmed.StartsWith("["))
        {
            var list = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(trimmed);
            return list?.Count > 0 ? list[0] : null;
        }
        return JsonSerializer.Deserialize<Dictionary<string, object>>(trimmed);
    }

    private static string? ResolveMessage(string? message, Dictionary<string, object> data)
    {
        if (message == null) return null;
        return FieldRefPattern.Replace(message, match =>
        {
            var value = FieldResolver.Resolve(data, match.Groups[1].Value);
            return value?.ToString() ?? "null";
        });
    }

    private static string ResolveFieldReferences(string expression, Dictionary<string, object> data)
    {
        return FieldRefPattern.Replace(expression, match =>
        {
            var value = FieldResolver.Resolve(data, match.Groups[1].Value);
            if (value == null) return "null";
            if (value is int or long or decimal or double or float) return value.ToString()!;
            if (value is bool b) return b ? "true" : "false";
            return $"\"{value}\"";
        });
    }
}
