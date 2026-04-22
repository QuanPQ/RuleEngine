using System.Text.Json.Serialization;

namespace RuleEngine.Engine.Models;

public class RuleActionModel
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActionType Type { get; set; }

    public int ActionOrder { get; set; }
    public string? Field { get; set; }
    public string? Message { get; set; }
    public string? ErrorCode { get; set; }
    public string? Expression { get; set; }
    public string? RuleId { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public string? MetadataQuery { get; set; }
}
