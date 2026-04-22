using System.Text.Json.Serialization;

namespace RuleEngine.Engine.Models;

public class RuleViolation
{
    public string RuleId { get; set; } = null!;
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Metadata { get; set; }

    public RuleViolation(string ruleId, string? errorCode, string? message, Dictionary<string, object>? metadata = null)
    {
        RuleId = ruleId;
        ErrorCode = errorCode;
        Message = message;
        Metadata = metadata;
    }
}
