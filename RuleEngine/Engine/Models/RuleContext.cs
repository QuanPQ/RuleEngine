using RuleEngine.Models;

namespace RuleEngine.Engine.Models;

public class RuleContext
{
    public Dictionary<string, object> Data { get; set; }
    public HashSet<string> EvaluatedRules { get; } = new();
    public List<RuleViolation> Violations { get; } = new();
    public List<string> ExecutionErrors { get; } = new();
    public bool HasErrors { get; set; }
    public string RequestId { get; set; }
    public string? TargetType { get; set; }
    public string? TargetCode { get; set; }

    public Dictionary<string, RuleAssignmentView> RuleIndex { get; set; } = new();

    public List<RuleExecutionLog> PendingLogs { get; } = new();

    public RuleContext(Dictionary<string, object> data)
    {
        Data = data;
        RequestId = Guid.NewGuid().ToString();
    }

    public bool HasViolations => Violations.Count > 0 || HasErrors;
}
