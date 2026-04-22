using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using RuleEngine.Engine.Models;
using RuleEngine.Exceptions;

namespace RuleEngine.Engine;

public class RuleConditionEvaluator
{
    private readonly string _connectionString;
    private readonly ILogger<RuleConditionEvaluator> _logger;

    private static readonly Regex DbFuncPattern = new(
        @"^([A-Za-z_][\w.]*)\((.*)?\)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public RuleConditionEvaluator(IConfiguration configuration, ILogger<RuleConditionEvaluator> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _logger = logger;
    }

    public bool Evaluate(RuleConditionModel? condition, Dictionary<string, object> data)
    {
        if (condition == null) return true;
        return condition.IsComposite ? EvaluateComposite(condition, data) : EvaluateSimple(condition, data);
    }

    private bool EvaluateComposite(RuleConditionModel condition, Dictionary<string, object> data)
    {
        if (condition.Type == ConditionType.NOT)
            return !Evaluate(condition.Condition, data);

        var children = condition.Conditions;
        if (children == null || children.Count == 0) return true;

        return condition.Type == ConditionType.AND
            ? children.All(c => Evaluate(c, data))
            : children.Any(c => Evaluate(c, data));
    }

    private bool EvaluateSimple(RuleConditionModel condition, Dictionary<string, object> data)
    {
        if (condition.Field == null)
            throw new RuleEngineException($"Condition field is null");
        if (condition.Operator == null)
            throw new RuleEngineException($"Condition operator is null for field: {condition.Field}");

        var fieldValue = FieldResolver.Resolve(data, condition.Field);

        return condition.Operator switch
        {
            Operator.EQ => IsEqual(fieldValue, condition.Value),
            Operator.NEQ => !IsEqual(fieldValue, condition.Value),
            Operator.GT => Compare(fieldValue, condition.Value) > 0,
            Operator.GTE => Compare(fieldValue, condition.Value) >= 0,
            Operator.LT => Compare(fieldValue, condition.Value) < 0,
            Operator.LTE => Compare(fieldValue, condition.Value) <= 0,
            Operator.IN => IsIn(fieldValue, condition.Values),
            Operator.NOT_IN => !IsIn(fieldValue, condition.Values),
            Operator.BETWEEN => IsBetween(fieldValue, condition.Values),
            Operator.IS_NULL => fieldValue == null,
            Operator.IS_NOT_NULL => fieldValue != null,
            Operator.CONTAINS => ContainsCheck(fieldValue, condition.Value),
            Operator.NOT_CONTAINS => !ContainsCheck(fieldValue, condition.Value),
            Operator.STARTS_WITH => fieldValue?.ToString()?.StartsWith(condition.Value?.ToString() ?? "") == true,
            Operator.ENDS_WITH => fieldValue?.ToString()?.EndsWith(condition.Value?.ToString() ?? "") == true,
            Operator.MATCHES => fieldValue != null && Regex.IsMatch(fieldValue.ToString()!, condition.Value?.ToString() ?? ""),
            Operator.NOT_MATCHES => fieldValue == null || !Regex.IsMatch(fieldValue.ToString()!, condition.Value?.ToString() ?? ""),
            Operator.IS_VALID_DATE => IsValidDate(fieldValue, condition.Value?.ToString()),
            Operator.IS_NOT_VALID_DATE => !IsValidDate(fieldValue, condition.Value?.ToString()),
            Operator.DB_FUNCTION => EvaluateDbFunction(condition, data),
            _ => throw new RuleEngineException($"Unknown operator: {condition.Operator}")
        };
    }

    private static bool IsEqual(object? a, object? b)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;
        if (TryParseDecimal(a, out var da) && TryParseDecimal(b, out var db))
            return da == db;
        return a.ToString() == b.ToString();
    }

    private static int Compare(object? a, object? b)
    {
        if (a == null) return -1;
        if (b == null) return 1;
        if (TryParseDecimal(a, out var da) && TryParseDecimal(b, out var db))
            return da.CompareTo(db);
        return string.Compare(a.ToString(), b.ToString(), StringComparison.Ordinal);
    }

    private static bool IsIn(object? fieldValue, List<object>? values)
    {
        if (fieldValue == null || values == null) return false;
        return values.Any(v => IsEqual(fieldValue, v));
    }

    private static bool IsBetween(object? fieldValue, List<object>? values)
    {
        if (fieldValue == null || values == null || values.Count < 2) return false;
        return Compare(fieldValue, values[0]) >= 0 && Compare(fieldValue, values[1]) <= 0;
    }

    private static bool ContainsCheck(object? fieldValue, object? ruleValue)
    {
        if (fieldValue == null || ruleValue == null) return false;
        return fieldValue.ToString()!.Contains(ruleValue.ToString()!);
    }

    private static bool TryParseDecimal(object? value, out decimal result)
    {
        result = 0;
        if (value == null) return false;
        if (value is decimal d) { result = d; return true; }
        if (value is int i) { result = i; return true; }
        if (value is long l) { result = l; return true; }
        if (value is double dbl) { result = (decimal)dbl; return true; }
        if (value is float f) { result = (decimal)f; return true; }
        return decimal.TryParse(value.ToString(), out result);
    }

    private static bool IsValidDate(object? value, string? format)
    {
        if (value == null) return false;
        var str = value.ToString();
        if (string.IsNullOrWhiteSpace(str)) return false;

        if (!string.IsNullOrWhiteSpace(format))
        {
            return DateTime.TryParseExact(str, format,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out _);
        }

        return DateTime.TryParse(str, out _);
    }

    private bool EvaluateDbFunction(RuleConditionModel condition, Dictionary<string, object> data)
    {
        var fieldExpr = condition.Field!;
        var sqlParams = new List<SqlParameter>();
        var resolvedFunc = ResolveDbFunctionParams(fieldExpr, data, sqlParams);

        var sql = $"SELECT {resolvedFunc}";
        _logger.LogDebug("DB_FUNCTION: {Sql} with {Count} params", sql, sqlParams.Count);

        object? result;
        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddRange(sqlParams.ToArray());
            conn.Open();
            result = cmd.ExecuteScalar();
        }

        var cmpOp = condition.CompareOperator ?? Operator.EQ;
        return cmpOp switch
        {
            Operator.EQ => IsEqual(result, condition.Value),
            Operator.NEQ => !IsEqual(result, condition.Value),
            Operator.GT => Compare(result, condition.Value) > 0,
            Operator.GTE => Compare(result, condition.Value) >= 0,
            Operator.LT => Compare(result, condition.Value) < 0,
            Operator.LTE => Compare(result, condition.Value) <= 0,
            _ => throw new RuleEngineException($"Unsupported compareOperator for DB_FUNCTION: {cmpOp}")
        };
    }

    internal static string ResolveDbFunctionParams(string expression, Dictionary<string, object> data, List<SqlParameter> sqlParams)
    {
        var match = DbFuncPattern.Match(expression);
        if (!match.Success) return expression;

        var funcName = match.Groups[1].Value;
        var rawArgs = match.Groups[2].Value.Trim();
        if (string.IsNullOrEmpty(rawArgs)) return $"{funcName}()";

        var args = SplitArgs(rawArgs);
        var placeholders = new List<string>();
        int paramIndex = 0;

        foreach (var arg in args)
        {
            var trimmed = arg.Trim();
            var paramName = $"@p{paramIndex++}";

            if (trimmed.StartsWith("${") && trimmed.EndsWith("}"))
            {
                var fieldPath = trimmed[2..^1];
                sqlParams.Add(new SqlParameter(paramName, FieldResolver.Resolve(data, fieldPath) ?? DBNull.Value));
                placeholders.Add(paramName);
            }
            else if ((trimmed.StartsWith("'") && trimmed.EndsWith("'")) ||
                     (trimmed.StartsWith("\"") && trimmed.EndsWith("\"")))
            {
                var inner = trimmed[1..^1].Replace("''", "'");
                sqlParams.Add(new SqlParameter(paramName, inner));
                placeholders.Add(paramName);
            }
            else if (trimmed.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                sqlParams.Add(new SqlParameter(paramName, DBNull.Value));
                placeholders.Add(paramName);
            }
            else if (decimal.TryParse(trimmed, out var num))
            {
                sqlParams.Add(new SqlParameter(paramName, num));
                placeholders.Add(paramName);
            }
            else
            {
                throw new RuleEngineException($"Invalid DB_FUNCTION parameter: {trimmed}");
            }
        }

        return $"{funcName}({string.Join(", ", placeholders)})";
    }

    internal static List<string> SplitArgs(string rawArgs)
    {
        var args = new List<string>();
        int depth = 0;
        bool inQuote = false;
        char quoteChar = '\0';
        var current = new System.Text.StringBuilder();

        foreach (var c in rawArgs)
        {
            if (inQuote) { current.Append(c); if (c == quoteChar) inQuote = false; continue; }
            if (c is '\'' or '"') { inQuote = true; quoteChar = c; current.Append(c); }
            else if (c == '{') { depth++; current.Append(c); }
            else if (c == '}') { depth--; current.Append(c); }
            else if (c == ',' && depth == 0) { args.Add(current.ToString()); current.Clear(); }
            else { current.Append(c); }
        }
        if (current.Length > 0) args.Add(current.ToString());
        return args;
    }
}
