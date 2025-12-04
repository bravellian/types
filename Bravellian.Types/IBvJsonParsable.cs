// Copyright (c) Samuel McAravey
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
