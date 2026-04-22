using System.Text.Json;
using RuleEngine.DTOs;
using RuleEngine.Exceptions;

namespace RuleEngine.Middleware;

public class RuleExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RuleExceptionMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RuleExceptionMiddleware(RequestDelegate next, ILogger<RuleExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RuleRejectException ex)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            var response = new RuleEvaluationResponseDto
            {
                RequestId = Guid.NewGuid().ToString(),
                Valid = false,
                HasErrors = false,
                Violations = ex.Violations
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
        catch (Exception ex)
        {
            var requestId = Guid.NewGuid().ToString();
            _logger.LogError(ex, "Rule engine error [requestId={RequestId}]", requestId);

            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            var response = new RuleEvaluationResponseDto
            {
                RequestId = requestId,
                Valid = false,
                HasErrors = true,
                ErrorMessage = $"Rule evaluation encountered an internal error. Please contact support with requestId: {requestId}",
                Violations = new()
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
    }
}
