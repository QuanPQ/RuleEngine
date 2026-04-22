using RuleEngine.Engine.Models;

namespace RuleEngine.Exceptions;

public class RuleRejectException : Exception
{
    public List<RuleViolation> Violations { get; }

    public RuleRejectException(List<RuleViolation> violations)
        : base($"Rule validation failed with {violations.Count} violation(s)")
    {
        Violations = violations;
    }

    public RuleRejectException(RuleViolation violation)
        : base(violation.Message)
    {
        Violations = new List<RuleViolation> { violation };
    }
}
