using System.ComponentModel.DataAnnotations;

namespace RuleEngine.DTOs;

public class RuleEvaluationRequestDto
{
    [Required]
    public string? ApplicationId { get; set; }
    [Required]
    public string? TargetType { get; set; }
    [Required]
    public string? TargetCode { get; set; }
    [Required]
    public Dictionary<string, object>? Data { get; set; }
}
