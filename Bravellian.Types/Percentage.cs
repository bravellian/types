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

namespace Bravellian;
/// <summary>
/// Represents a percentage value. The precision of a percentage is always truncated to
/// two decimal places for its scaled value (e.g., 12.34%), which requires four decimal
/// places for the underlying raw value (e.g., 0.1234).
/// </summary>
[JsonConverter(typeof(PercentageJsonConverter))]
[TypeConverter(typeof(PercentageTypeConverter))]
public readonly record struct Percentage
        : IComparable,
          IComparable<Percentage>,
          ISpanParsable<Percentage>,
          IDecimalBackedType<Percentage>
{
    private readonly decimal rawValue;

    /// <summary>
    /// Represents 0% (zero percent).
    /// </summary>
    public static readonly Percentage Zero = new(0m);
    /// <summary>
    /// Represents 100% (one hundred percent).
    /// </summary>
    public static readonly Percentage Hundred = new(1m);

    public static Percentage From(decimal value) => new Percentage(value);

    public static Percentage? From(decimal? value) => value.HasValue ? new Percentage(value.Value) : null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Percentage"/> struct from a double value.
    /// The value is truncated to ensure a precision of two decimal places on the scaled value.
    /// </summary>
    /// <param name="value">The percentage value as a double (e.g., 0.25 for 25%).</param>
    public Percentage(double value)
        : this(Convert.ToDecimal(value))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Percentage"/> struct from a decimal value.
    /// The value is truncated to ensure a precision of two decimal places on the scaled value.
    /// </summary>
    /// <param name="value">The percentage value as a decimal (e.g., 0.25 for 25%).</param>
    public Percentage(decimal value)
    {
        rawValue = TruncateToFourDecimalPlaces(value);
    }

    /// <summary>
    /// Gets the underlying value of the percentage (e.g., 0.25 for 25%).
    /// Always truncated to four decimal places.
    /// </summary>
    public decimal Value => rawValue;

    /// <summary>
    /// Gets the value scaled to percent (e.g., 25 for 25%).
    /// </summary>
    public decimal ScaledValue => rawValue * 100;

    private static decimal TruncateToFourDecimalPlaces(decimal value)
    {
        // We truncate to 4 decimal places for the raw value to ensure
        // the scaled value has a precision of 2 decimal places.
        return Math.Truncate(value * 10000m) / 10000m;
    }

    public static bool operator >(Percentage a, Percentage b) => a.Value > b.Value;

    public static bool operator <(Percentage a, Percentage b) => a.Value < b.Value;

    public static bool operator >=(Percentage a, Percentage b) => a.Value >= b.Value;

    public static bool operator <=(Percentage a, Percentage b) => a.Value <= b.Value;

    public static bool operator >(Percentage a, int b) => a.Value > b;

    public static bool operator <(Percentage a, int b) => a.Value < b;

    public static bool operator >=(Percentage a, int b) => a.Value >= b;

    public static bool operator <=(Percentage a, int b) => a.Value <= b;

    public static bool operator >(Percentage a, decimal b) => a.Value > b;

    public static bool operator <(Percentage a, decimal b) => a.Value < b;

    public static bool operator >=(Percentage a, decimal b) => a.Value >= b;

    public static bool operator <=(Percentage a, decimal b) => a.Value <= b;

    /// <summary>
    /// Returns a formatted string representation of the percentage value with 2 decimal places.
    /// This is a convenience method that calls ToString(2).
    /// </summary>
    /// <returns>
    /// A string representing the percentage with 2 decimal places and a percent symbol.
    /// </returns>
    /// <example>
    /// <code>
    /// // For a Percentage with Value = 0.123456m (stored as 0.1234)
    /// var percentage = new Percentage(0.123456m);
    /// string result = percentage.ToString(); // result is "12.34%"
    ///
    /// // For a Percentage with Value = 0.5 (50%)
    /// var percentage2 = new Percentage(0.5m);
    /// string result2 = percentage2.ToString(); // result2 is "50.00%"
    /// </code>
    /// </example>
    public override string ToString() => ToString(2);

    /// <summary>
    /// Returns the raw scaled value followed by a percent symbol (%).
    /// The value is truncated, not rounded, to four decimal places.
    /// </summary>
    /// <returns>
    /// The ScaledValue followed by a percent symbol.
    /// </returns>
    /// <example>
    /// <code>
    /// // For a Percentage with Value = 0.1234 (12.34%)
    /// var percentage = new Percentage(0.1234m);
    /// string result = percentage.ToStringRaw(); // result is "12.34%"
    ///
    /// // For a Percentage with Value = 0.123456 (truncated to 12.34%)
    /// var percentage2 = new Percentage(0.123456m);
    /// string result2 = percentage2.ToStringRaw(); // result2 is "12.34%"
    ///
    /// // For a Percentage with Value = 1.0 (100%)
    /// var percentage3 = new Percentage(1.0m);
    /// string result3 = percentage3.ToStringRaw(); // result3 is "100%"
    /// </code>
    /// </example>
    public string ToStringRaw() => $"{ScaledValue:0.00}%";

    /// <summary>
    /// Returns a formatted string representation of the percentage with the specified number of decimal places.
    /// Uses truncation to ensure consistent behavior across the type.
    /// </summary>
    /// <param name="decimals">The number of decimal places to include in the formatted string.</param>
    /// <returns>
    /// A string representing the percentage with the specified number of decimal places and a percent symbol.
    /// </returns>
    /// <example>
    /// <code>
    /// // For a Percentage with Value = 0.123456m (truncated to 0.1234)
    /// var percentage = new Percentage(0.123456m);
    /// string result0 = percentage.ToString(0); // result0 is "12%"
    /// string result1 = percentage.ToString(1); // result1 is "12.3%"
    /// string result2 = percentage.ToString(2); // result2 is "12.34%"
    /// string result3 = percentage.ToString(3); // result3 is "12.340%"
    ///
    /// // For a Percentage with Value = 0.005 (0.5%)
    /// var percentage2 = new Percentage(0.005m);
    /// string result = percentage2.ToString(2); // result is "0.50%"
    /// </code>
    /// </example>
    public string ToString(int decimals)
    {
        var scaledValue = ScaledValue;
        var multiplier = (decimal)Math.Pow(10, decimals);
        var truncatedValue = Math.Truncate(scaledValue * multiplier) / multiplier;

        var format = decimals > 0 ? "0." + new string('0', decimals) : "0";
        return $"{truncatedValue.ToString(format)}%";
    }

    public static Percentage? TryParse(string value)
    {
        return decimal.TryParse(value, CultureInfo.InvariantCulture, out var result) ? new Percentage(result) : null;
    }

    public static bool TryParse(string value, out Percentage id)
    {
        if (decimal.TryParse(value, CultureInfo.InvariantCulture, out var result))
        {
            id = new Percentage(result);
            return true;
        }

        id = default;
        return false;
    }

    public static Percentage? TryParseScaled(string value)
    {
        return decimal.TryParse(value, out var result) ? new Percentage(result / 100) : null;
    }

    public static bool TryParseScaled(string value, out Percentage id)
    {
        if (decimal.TryParse(value, out var result))
        {
            id = new Percentage(result / 100);
            return true;
        }

        id = default;
        return false;
    }

    public static Percentage Parse(string value) => new(decimal.Parse(value));

    public static Percentage ParseScaled(string value) => new(decimal.Parse(value) / 100);

    public static Percentage Parse(ReadOnlySpan<char> s, IFormatProvider provider) => new(decimal.Parse(s, provider));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, [MaybeNullWhen(false)] out Percentage result)
    {
        if (decimal.TryParse(s, provider, out var value))
        {
            result = new Percentage(value);
            return true;
        }

        result = default;
        return false;
    }

    public static Percentage Parse(string s, IFormatProvider provider) => new(decimal.Parse(s, provider));

    public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out Percentage result)
    {
        if (decimal.TryParse(s, provider, out var value))
        {
            result = new Percentage(value);
            return true;
        }

        result = default;
        return false;
    }

    public int CompareTo(Percentage other)
    {
        return Value.CompareTo(other.Value);
    }

    public int CompareTo(object obj)
    {
        if (obj is Percentage other)
        {
            return CompareTo(other);
        }

        throw new ArgumentException("Object is not a Percentage", nameof(obj));
    }

    public class PercentageJsonConverter : JsonConverter<Percentage>
    {
        public override Percentage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return Parse(reader.GetString());
            }

            return reader.TryGetDecimal(out var dec) ? new Percentage(dec) : throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Percentage value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value.ToString());
    }

    // TypeConverter for Percentage to and from string and decimal
    public class PercentageTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) || sourceType == typeof(decimal) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string) || destinationType == typeof(decimal) || base.CanConvertTo(context, destinationType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
            {
                return Parse(s);
            }

            if (value is decimal d)
            {
                return new Percentage(d);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Percentage percentage)
            {
                if (destinationType == typeof(string))
                {
                    return percentage.ScaledValue.ToString("#,##0.00;-#,##0.00", CultureInfo.InvariantCulture);
                }

                if (destinationType == typeof(decimal))
                {
                    return percentage.Value;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
