using System.Text.Json.Serialization;
using RuleEngine.Engine.Models;

namespace RuleEngine.DTOs;

public class RuleEvaluationResponseDto
{
    public string? RequestId { get; set; }
    public string? TargetType { get; set; }
    public string? TargetCode { get; set; }
    public bool Valid { get; set; }
    public bool HasErrors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; set; }

    public List<RuleViolation> Violations { get; set; } = new();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Data { get; set; }
}
