namespace RuleEngine.Exceptions;

public class RuleEngineException : Exception
{
    public RuleEngineException(string message) : base(message) { }
    public RuleEngineException(string message, Exception inner) : base(message, inner) { }
}
