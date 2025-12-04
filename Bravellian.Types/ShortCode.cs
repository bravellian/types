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

namespace Bravellian;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(ShortCodeJsonConverter))]
[TypeConverter(typeof(ShortCodeTypeConverter))]
public class ShortCode : IBvParsable<ShortCode>
{
    private const string Chars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";

    public ShortCode()
    {
        this.RawCode = GenerateInternal(8);
    }

    public ShortCode(string code)
    {
        this.RawCode = code.Replace("-", string.Empty, StringComparison.Ordinal).Trim().ToUpperInvariant();
    }

    public string FormattedCode
    {
        get
        {
            var middle = this.RawCode.Length / 2;
            return $"{this.RawCode[..middle]}-{this.RawCode[middle..]}";
        }
    }

    public string RawCode { get; }

    public static ShortCode Generate(int length)
    {
        var code = GenerateInternal(length);
        return new ShortCode(code);
    }

    public override string ToString()
    {
        return this.RawCode;
    }

    private static string GenerateInternal(int length)
    {
        var charsLength = (byte)Chars.Length;

        var sb = new StringBuilder();
        _ = new byte[1];
        for (var i = 0; i < length; i++)
        {
            byte[] randomNumber;
            do
            {
                randomNumber = RandomNumberGenerator.GetBytes(1);
            }
            while (!IsFairRoll(randomNumber[0], charsLength));

            var index = (byte)(randomNumber[0] % charsLength);
            sb.Append(Chars[index]);
        }

        return sb.ToString();
    }

    private static bool IsFairRoll(byte roll, byte maxValue)
    {
        // There are MaxValue / maxValue full sets of numbers that can come up
        // in a single byte.  For instance, if we have a 6 sided die, there are
        // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
        var fullSetsOfValues = byte.MaxValue / maxValue;

        // If the roll is within this range of fair values, then we let it continue.
        // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
        // < rather than <= since the = portion allows through an extra 0 value).
        // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
        // to use.
        return roll < maxValue * fullSetsOfValues;
    }

    public class ShortCodeJsonConverter : JsonConverter<ShortCode>
    {
        public override ShortCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new ShortCode(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, ShortCode value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());

        public override void WriteAsPropertyName(
            Utf8JsonWriter writer,
            ShortCode value,
            JsonSerializerOptions options) =>
                writer.WritePropertyName(value.ToString());

        public override ShortCode ReadAsPropertyName(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                this.Read(ref reader, typeToConvert, options);
    }

    // TypeConverter for ShortCode to and from string and Guid
    public class ShortCodeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
            sourceType == typeof(string) || sourceType == typeof(Guid) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
            destinationType == typeof(string) || destinationType == typeof(Guid) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                return new ShortCode(s);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is ShortCode type && destinationType == typeof(string))
            {
                return type.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public static ShortCode Parse(string value)
    {
        return new ShortCode(value);
    }

    public static bool TryParse([NotNullWhen(true)] string? value, [MaybeNullWhen(false)] out ShortCode result)
    {
        result = new ShortCode(value);
        return true;
    }
}
