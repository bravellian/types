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

[JsonConverter(typeof(EmailAddressJsonConverter))]
[TypeConverter(typeof(EmailAddressTypeConverter))]
public readonly partial record struct EmailAddress
        : IComparable,
          IComparable<EmailAddress>,
          IEquatable<EmailAddress>,
          IParsable<EmailAddress>
{
    private EmailAddress(string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Value = value;
        ProcessValue(value, out System.Net.Mail.MailAddress address);
        Address = address;
    }

    public string Value { get; init; }

    public System.Net.Mail.MailAddress Address { get; init; }

    private static partial void ProcessValue(string value, out System.Net.Mail.MailAddress address);

    public static EmailAddress From(string value) => new(value);

    public override string ToString() => Value;

    public bool Equals(EmailAddress other)
    {
        return string.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }

    public int CompareTo(EmailAddress other)
    {
        return string.Compare(Value, other.Value);
    }

    public int CompareTo(object? obj)
    {
        return obj is EmailAddress id ? Value.CompareTo(id.Value) : Value.CompareTo(obj);
    }

    public static bool operator <(EmailAddress left, EmailAddress right) => left.CompareTo(right) < 0;

    public static bool operator <=(EmailAddress left, EmailAddress right) => left.CompareTo(right) <= 0;

    public static bool operator >(EmailAddress left, EmailAddress right) => left.CompareTo(right) > 0;

    public static bool operator >=(EmailAddress left, EmailAddress right) => left.CompareTo(right) >= 0;

    public static bool TryParse([NotNullWhen(true)] string? value, [MaybeNullWhen(false)][NotNullWhen(true)] out EmailAddress result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        try
        {
            result = new EmailAddress(value);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out EmailAddress result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        try
        {
            result = new EmailAddress(value);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static EmailAddress? TryParse(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (EmailAddress.TryParse(value, out EmailAddress result))
        {
            return result;
        }

        return null;
    }

    public static EmailAddress Parse(string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);

        return new EmailAddress(value);
    }

    public static EmailAddress Parse(string value, IFormatProvider? provider)
    {
        Guard.IsNotNullOrWhiteSpace(value);

        return new EmailAddress(value);
    }

    public static EmailAddress GenerateRandom()
    {
        return new EmailAddress(Guid.NewGuid().ToString("N"));
    }

    public class EmailAddressJsonConverter : JsonConverter<EmailAddress>
    {
        public override EmailAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();

            if (!string.IsNullOrEmpty(s) && EmailAddress.TryParse(s, out EmailAddress result))
            {
                return result;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);

        public override void WriteAsPropertyName(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options) =>
            writer.WritePropertyName(value.Value);

        public override EmailAddress ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Read(ref reader, typeToConvert, options);
        }
    }

    // TypeConverter for EmailAddress to and from string
    public class EmailAddressTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
            sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                return EmailAddress.TryParse(s) ?? default;
            }

            return base.ConvertFrom(context, culture, value) ?? default;
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is EmailAddress type && destinationType == typeof(string))
            {
                return type.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
