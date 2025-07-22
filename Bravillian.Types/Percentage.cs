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

namespace Bravillian;

using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a percentage value, always truncated to four decimal places.
/// </summary>
[JsonConverter(typeof(PercentageJsonConverter))]
[TypeConverter(typeof(PercentageTypeConverter))]
public readonly record struct Percentage
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

    /// <summary>
    /// Initializes a new instance of the <see cref="Percentage"/> struct from a double value.
    /// The value is stored as-is (not truncated).
    /// </summary>
    /// <param name="value">The percentage value as a double (e.g., 0.25 for 25%).</param>
    public Percentage(double value)
        : this(Convert.ToDecimal(value))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Percentage"/> struct from a decimal value.
    /// The value is stored as-is (not truncated).
    /// </summary>
    /// <param name="value">The percentage value as a decimal (e.g., 0.25 for 25%).</param>
    public Percentage(decimal value)
    {
        this.rawValue = value;
    }

    /// <summary>
    /// The underlying value of the percentage (e.g., 0.25 for 25%).
    /// Always truncated to four decimal places when accessed.
    /// </summary>
    public decimal Value => TruncateToFourDecimalPlaces(this.rawValue);

    /// <summary>
    /// The value scaled to percent (e.g., 25 for 25%).
    /// Always truncated to four decimal places before scaling.
    /// </summary>
    public decimal ScaledValue => TruncateToFourDecimalPlaces(this.rawValue) * 100;

    private static decimal TruncateToFourDecimalPlaces(decimal value)
    {
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
    /// // For a Percentage with Value = 0.1234 (12.34%)
    /// var percentage = new Percentage(0.1234m);
    /// string result = percentage.ToString(); // result is "12.34%"
    /// 
    /// // For a Percentage with Value = 0.5 (50%)
    /// var percentage2 = new Percentage(0.5m);
    /// string result2 = percentage2.ToString(); // result2 is "50.00%"
    /// </code>
    /// </example>
    public override string ToString() => this.ToString(2);

    /// <summary>
    /// Returns the raw scaled value followed by a percent symbol (%).
    /// Does not perform any rounding or formatting of decimal places.
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
    /// // For a Percentage with Value = 0.123456 (12.3456%)
    /// var percentage2 = new Percentage(0.123456m);
    /// string result2 = percentage2.ToStringRaw(); // result2 is "12.3456%"
    /// 
    /// // For a Percentage with Value = 1.0 (100%)
    /// var percentage3 = new Percentage(1.0m);
    /// string result3 = percentage3.ToStringRaw(); // result3 is "100%"
    /// </code>
    /// </example>
    public string ToStringRaw() => $"{this.ScaledValue}%";

    /// <summary>
    /// Returns a formatted string representation of the percentage with the specified number of decimal places.
    /// Uses the MidpointRounding.ToZero rounding mode to ensure consistent truncation of values.
    /// </summary>
    /// <param name="decimals">The number of decimal places to include in the formatted string.</param>
    /// <returns>
    /// A string representing the percentage with the specified number of decimal places and a percent symbol.
    /// </returns>
    /// <example>
    /// <code>
    /// // For a Percentage with Value = 0.1234 (12.34%)
    /// var percentage = new Percentage(0.1234m);
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
    public string ToString(int decimals) => $"{Math.Round(this.ScaledValue, decimals, MidpointRounding.ToZero).ToString($"0.{new string('0', decimals)}")}%";

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
