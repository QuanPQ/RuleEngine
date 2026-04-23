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

    public RuleEvaluationResponseDto Evaluate(RuleEvaluationRequestDto request)
    {
        if (request == null)
        {
            throw new RuleEngineException("Request cannot be null");
        }

        if (request.Data == null ||
            string.IsNullOrWhiteSpace(request.ApplicationId) || 
            string.IsNullOrWhiteSpace(request.TargetType) || string.IsNullOrWhiteSpace(request.TargetCode))
            throw new RuleEngineException("applicationId, targetType, targetCode, and data are required");

        _logger.LogInformation("Evaluating rules for {TargetType}/{TargetCode}", request.TargetType, request.TargetCode);
        var applicationId = Guid.Parse(request.ApplicationId);
        var context = _engine.Evaluate(applicationId, request.TargetType, request.TargetCode, request.Data);

        return new RuleEvaluationResponseDto
        {
            RequestId = context.RequestId,
            ApplicationId = request.ApplicationId.ToString(),
            TargetType = request.TargetType,
            TargetCode = request.TargetCode,
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
    public void EvictByApplication(string applicationId) => _engine.EvictByApplication(applicationId);
    //public void EvictByTarget(string targetType, string targetCode) => _engine.EvictByTarget(targetType, targetCode);
    public void EvictByRuleId(string ruleId) => _engine.EvictByRuleId(ruleId);
}
