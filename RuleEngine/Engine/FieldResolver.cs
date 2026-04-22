using System.Text.Json;

namespace RuleEngine.Engine;

public static class FieldResolver
{
    public static object? Resolve(Dictionary<string, object> data, string fieldPath)
    {
        if (data == null || string.IsNullOrWhiteSpace(fieldPath))
            return null;

        var segments = fieldPath.Split('.');
        object? current = data;

        foreach (var segment in segments)
        {
            if (current is Dictionary<string, object> dict)
            {
                dict.TryGetValue(segment, out current);
            }
            else if (current is JsonElement jsonEl && jsonEl.ValueKind == JsonValueKind.Object)
            {
                if (jsonEl.TryGetProperty(segment, out var prop))
                    current = prop;
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        return UnwrapJsonElement(current);
    }

    public static void SetValue(Dictionary<string, object> data, string fieldPath, object? value)
    {
        if (data == null || string.IsNullOrWhiteSpace(fieldPath))
            return;

        var segments = fieldPath.Split('.');
        var current = data;

        for (int i = 0; i < segments.Length - 1; i++)
        {
            if (current.TryGetValue(segments[i], out var next))
            {
                if (next is Dictionary<string, object> nextDict)
                {
                    current = nextDict;
                }
                else if (next is JsonElement jsonEl && jsonEl.ValueKind == JsonValueKind.Object)
                {
                    var converted = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonEl.GetRawText())!;
                    current[segments[i]] = converted;
                    current = converted;
                }
                else
                {
                    var newDict = new Dictionary<string, object>();
                    current[segments[i]] = newDict;
                    current = newDict;
                }
            }
            else
            {
                var newDict = new Dictionary<string, object>();
                current[segments[i]] = newDict;
                current = newDict;
            }
        }

        current[segments[^1]] = value!;
    }

    private static object? UnwrapJsonElement(object? value)
    {
        if (value is not JsonElement el) return value;

        return el.ValueKind switch
        {
            JsonValueKind.String => el.GetString(),
            JsonValueKind.Number => el.TryGetInt64(out var l) ? l : el.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Object => JsonSerializer.Deserialize<Dictionary<string, object>>(el.GetRawText()),
            JsonValueKind.Array => JsonSerializer.Deserialize<List<object>>(el.GetRawText()),
            _ => el.GetRawText()
        };
    }
}
