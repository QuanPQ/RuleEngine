using Microsoft.AspNetCore.Mvc;
using RuleEngine.DTOs;
using RuleEngine.Services;

namespace RuleEngine.Controllers;

[Route("api/v1/rules")]
[ApiController]
public class RulesController : ControllerBase
{
    private readonly RuleService _ruleService;

    public RulesController(RuleService ruleService)
    {
        _ruleService = ruleService;
    }

    [HttpPost("evaluate")]
    public ActionResult<RuleEvaluationResponseDto> Evaluate([FromBody] Dictionary<string, object> request)
    {
        return Ok(_ruleService.Evaluate(request));
    }

    [HttpDelete("cache")]
    public IActionResult EvictAll()
    {
        _ruleService.EvictAll();
        return Ok(new { message = "All rule caches cleared" });
    }

    [HttpDelete("cache/target/{targetType}/{targetCode}")]
    public IActionResult EvictByTarget(string targetType, string targetCode)
    {
        _ruleService.EvictByTarget(targetType, targetCode);
        return Ok(new { message = $"Cache cleared for {targetType}/{targetCode}" });
    }

    [HttpDelete("cache/rule/{ruleId}")]
    public IActionResult EvictByRuleId(string ruleId)
    {
        _ruleService.EvictByRuleId(ruleId);
        return Ok(new { message = $"Cache cleared for ruleId: {ruleId}" });
    }
}
