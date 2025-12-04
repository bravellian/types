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

using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Diagnostics;

#nullable enable

namespace Bravellian;

[JsonConverter(typeof(EncryptedStringJsonConverter))]
[TypeConverter(typeof(EncryptedStringTypeConverter))]
public readonly partial record struct EncryptedString
        : IComparable,
          IComparable<EncryptedString>,
          IEquatable<EncryptedString>,
          IParsable<EncryptedString>
{
    private EncryptedString(string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Value = value;
        ProcessValue(value);
    }

    public string Value { get; init; }

    static partial void ProcessValue(string value);

    public static EncryptedString From(string value) => new(value);

    public override string ToString() => Value;

    public bool Equals(EncryptedString other)
    {
        return string.Equals(Value, other.Value, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode(StringComparison.Ordinal) ?? 0;
    }

    public int CompareTo(EncryptedString other)
    {
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        return obj is EncryptedString id ? Value.CompareTo(id.Value) : Value.CompareTo(obj);
    }

    public static bool operator <(EncryptedString left, EncryptedString right) => left.CompareTo(right) < 0;

    public static bool operator <=(EncryptedString left, EncryptedString right) => left.CompareTo(right) <= 0;

    public static bool operator >(EncryptedString left, EncryptedString right) => left.CompareTo(right) > 0;

    public static bool operator >=(EncryptedString left, EncryptedString right) => left.CompareTo(right) >= 0;

    public static bool TryParse([NotNullWhen(true)] string? value, [MaybeNullWhen(false)][NotNullWhen(true)] out EncryptedString result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        try
        {
            result = new EncryptedString(value);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out EncryptedString result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        try
        {
            result = new EncryptedString(value);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static EncryptedString? TryParse(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (EncryptedString.TryParse(value, out EncryptedString result))
        {
            return result;
        }

        return null;
    }

    public static EncryptedString Parse(string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);

        return new EncryptedString(value);
    }

    public static EncryptedString Parse(string value, IFormatProvider? provider)
    {
        Guard.IsNotNullOrWhiteSpace(value);

        return new EncryptedString(value);
    }

    public static EncryptedString GenerateRandom()
    {
        return new EncryptedString(Guid.NewGuid().ToString("N"));
    }

    public class EncryptedStringJsonConverter : JsonConverter<EncryptedString>
    {
        public override EncryptedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();

            if (!string.IsNullOrEmpty(s) && EncryptedString.TryParse(s, out EncryptedString result))
            {
                return result;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, EncryptedString value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);

        public override void WriteAsPropertyName(Utf8JsonWriter writer, EncryptedString value, JsonSerializerOptions options) =>
            writer.WritePropertyName(value.Value);

        public override EncryptedString ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Read(ref reader, typeToConvert, options);
        }
    }

    // TypeConverter for EncryptedString to and from string
    public class EncryptedStringTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
            sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                return EncryptedString.TryParse(s) ?? default;
            }

            return base.ConvertFrom(context, culture, value) ?? default;
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is EncryptedString type && destinationType == typeof(string))
            {
                return type.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
