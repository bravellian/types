using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

namespace Bravellian;

public interface IBvJsonParsable<TSelf>
    where TSelf : IBvJsonParsable<TSelf>?
{
    static abstract JsonTypeInfo<TSelf> TypeInfo { get; }

    static virtual TSelf Parse(JsonNode value)
    {
        var result = value.Deserialize(TSelf.TypeInfo);

        if (result is null)
        {
            throw new ArgumentException("The provided value is not a valid JSON node.", nameof(value));
        }

        return result;
    }

    static virtual bool TryParse(JsonNode? value, [MaybeNull] out TSelf result)
    {
        try
        {
            if (value is JsonNode json)
            {
                result = json.Deserialize(TSelf.TypeInfo);
                return true;
            }
        }
#pragma warning disable ERP022 // Unobserved exception in a generic exception handler
        catch
        {
            result = default;
            return false;
        }
#pragma warning restore ERP022 // Unobserved exception in a generic exception handler

        result = default;
        return false;
    }

    static virtual TSelf Parse(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        JsonNode? json = JsonSerializer.Deserialize(value, SourceGenerationContext.Default.JsonNode);

        if (json is not JsonNode)
        {
            throw new ArgumentException("The provided value is not a valid JSON node.", nameof(value));
        }
        return TSelf.Parse(json);
    }

    static virtual bool TryParse([NotNullWhen(true)] string? value, [MaybeNullWhen(false)] out TSelf result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        JsonNode? json = JsonSerializer.Deserialize(value, SourceGenerationContext.Default.JsonNode);
        return TSelf.TryParse(json, out result);
    }
}
