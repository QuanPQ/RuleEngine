using System.Text.Json.Serialization;

namespace RuleEngine.Engine.Models;

public class RuleConditionModel
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ConditionType? Type { get; set; }

    public List<RuleConditionModel>? Conditions { get; set; }
    public RuleConditionModel? Condition { get; set; }
    public string? Field { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Operator? Operator { get; set; }

    public object? Value { get; set; }
    public List<object>? Values { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Operator? CompareOperator { get; set; }

    public bool IsComposite =>
        Type == ConditionType.AND || Type == ConditionType.OR || Type == ConditionType.NOT;
}
