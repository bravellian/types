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
using Humanizer;

namespace Bravellian;

/// <summary>
/// Represents a monetary value, always truncated to two decimal places.
/// </summary>
[DebuggerDisplay("{ToAccounting(false, true)}")]
[JsonConverter(typeof(MoneyJsonConverter))]
[TypeConverter(typeof(MoneyTypeConverter))]
public readonly record struct Money
        : IComparable,
          IComparable<Money>,
          ISpanParsable<Money>,
          IDecimalBackedType<Money>
{
    private readonly decimal rawValue;

    /// <summary>
    /// Represents a zero monetary value ($0.00).
    /// </summary>
    public static readonly Money Zero = new(0);

    /// <summary>
    /// Number format info for currency formatting with group separators.
    /// </summary>
    public static readonly NumberFormatInfo MoneyFormatInfo = new()
    {
        CurrencyDecimalDigits = 2,
        CurrencyDecimalSeparator = ".",
        CurrencyGroupSeparator = ",",
        CurrencyGroupSizes = [3],
        CurrencyPositivePattern = 0, // $n
        CurrencyNegativePattern = 1, // -$n

        NumberDecimalDigits = 2,
        NumberGroupSizes = [3],
        NumberDecimalSeparator = ".",
        NumberGroupSeparator = ",",
        NumberNegativePattern = 1, // -n

        PercentDecimalDigits = 2,
        PercentGroupSizes = [3],
        PercentDecimalSeparator = ".",
        PercentGroupSeparator = ",",
        PercentPositivePattern = 1, // n%
        PercentNegativePattern = 1, // -n%

        PositiveInfinitySymbol = "∞",
        NegativeInfinitySymbol = "-∞",
        CurrencySymbol = "$",
        PercentSymbol = "%",
    };

    /// <summary>
    /// Number format info for currency formatting without group separators.
    /// </summary>
    public static readonly NumberFormatInfo MoneyNumericFormatInfo = new()
    {
        CurrencyDecimalDigits = 2,
        CurrencyDecimalSeparator = ".",
        CurrencyGroupSeparator = string.Empty,
        CurrencyGroupSizes = [3],
        CurrencyPositivePattern = 0, // $n
        CurrencyNegativePattern = 1, // -$n

        NumberDecimalDigits = 2,
        NumberGroupSizes = [3],
        NumberDecimalSeparator = ".",
        NumberGroupSeparator = string.Empty,
        NumberNegativePattern = 1, // -n

        PercentDecimalDigits = 2,
        PercentGroupSizes = [3],
        PercentDecimalSeparator = ".",
        PercentGroupSeparator = string.Empty,
        PercentPositivePattern = 1, // n%
        PercentNegativePattern = 1, // -n%

        PositiveInfinitySymbol = "∞",
        NegativeInfinitySymbol = "-∞",
        CurrencySymbol = "$",
        PercentSymbol = "%",
    };

    /// <summary>
    /// Number format info for accounting-style formatting (parentheses for negatives).
    /// </summary>
    public static readonly NumberFormatInfo MoneyAccountingFormatInfo = new()
    {
        CurrencyDecimalDigits = 2,
        CurrencyDecimalSeparator = ".",
        CurrencyGroupSeparator = ",",
        CurrencyGroupSizes = [3],
        CurrencyPositivePattern = 0, // $n
        CurrencyNegativePattern = 0, // ($n)

        NumberDecimalDigits = 2,
        NumberGroupSizes = [3],
        NumberDecimalSeparator = ".",
        NumberGroupSeparator = ",",
        NumberNegativePattern = 0, // (n)

        PercentDecimalDigits = 2,
        PercentGroupSizes = [3],
        PercentDecimalSeparator = ".",
        PercentGroupSeparator = ",",
        PercentPositivePattern = 1, // n%
        PercentNegativePattern = 1, // -n%

        PositiveInfinitySymbol = "∞",
        NegativeInfinitySymbol = "-∞",
        CurrencySymbol = "$",
        PercentSymbol = "%",
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="Money"/> struct, truncating the value to two decimal places.
    /// </summary>
    /// <param name="value">The monetary value as a decimal.</param>
    public Money(decimal value)
    {
        rawValue = TruncateToTwoDecimalPlaces(value);
    }

    public static Money From(decimal value) => new Money(value);
    public static Money? From(decimal? value) => value.HasValue ? new Money(value.Value) : null;

    private static decimal TruncateToTwoDecimalPlaces(decimal value)
    {
        return Math.Truncate(value * 100m) / 100m;
    }

    public static Money operator +(Money a, Money b) => new(a.rawValue + b.rawValue);

    public static Money operator +(Money a, decimal b) => new(a.rawValue + b);

    public static Money operator +(decimal a, Money b) => new(a + b.rawValue);

    public static Money operator +(Money a, int b) => new(a.rawValue + b);

    public static Money operator +(int a, Money b) => new(a + b.rawValue);

    public static bool operator ==(Money a, decimal b) => a.rawValue == b;

    public static bool operator ==(decimal a, Money b) => a == b.rawValue;

    public static bool operator !=(Money a, decimal b) => a.rawValue != b;

    public static bool operator !=(decimal a, Money b) => a != b.rawValue;

    public static bool operator ==(Money a, int b) => a.rawValue == b;

    public static bool operator ==(int a, Money b) => a == b.rawValue;

    public static bool operator !=(Money a, int b) => a.rawValue != b;

    public static bool operator !=(int a, Money b) => a != b.rawValue;

    public static bool operator >(Money a, Money b) => a.rawValue > b.rawValue;

    public static bool operator >(Money a, int b) => a.rawValue > b;

    public static bool operator >(Money a, decimal b) => a.rawValue > b;

    public static bool operator >=(Money a, Money b) => a.rawValue >= b.rawValue;

    public static bool operator >=(Money a, int b) => a.rawValue >= b;

    public static bool operator >=(Money a, decimal b) => a.rawValue >= b;

    public static bool operator <(Money a, Money b) => a.rawValue < b.rawValue;

    public static bool operator <(Money a, int b) => a.rawValue < b;

    public static bool operator <(Money a, decimal b) => a.rawValue < b;

    public static bool operator <=(Money a, Money b) => a.rawValue <= b.rawValue;

    public static bool operator <=(Money a, int b) => a.rawValue <= b;

    public static bool operator <=(Money a, decimal b) => a.rawValue <= b;

    public static Money operator *(Money a, Percentage b) => new(a.rawValue * b.Value);

    public static Money operator *(Percentage a, Money b) => new(a.Value * b.rawValue);

    public static Money operator *(Money a, int b) => new(a.rawValue * b);

    public static Money operator *(int a, Money b) => new(a * b.rawValue);

    public static Money operator *(Money a, decimal b) => new(a.rawValue * b);

    public static Money operator *(decimal a, Money b) => new(a * b.rawValue);

    public static Money operator /(Money a, int b) => new(a.rawValue / b);

    public static Money operator /(Money a, decimal b) => new(a.rawValue / b);

    public static decimal operator /(Money a, Money b) => a.rawValue / b.rawValue;

    public static Money operator %(Money a, decimal b) => new(a.rawValue % b);

    public static Money operator %(Money a, Money b) => new(a.rawValue % b.rawValue);

    public static Money operator -(Money a, Money b) => new(a.rawValue - b.rawValue);

    public static Money operator -(Money a, decimal b) => new(a.rawValue - b);

    public static Money operator -(decimal a, Money b) => new(a - b.rawValue);

    public static Money operator -(Money a, int b) => new(a.rawValue - b);

    public static Money operator -(int a, Money b) => new(a - b.rawValue);

    public static Money operator -(Money a) => new(-a.rawValue);

    public static Money operator +(Money a) => a;

    public static Percentage CalculateRelativePercentage(Money? numerator, Money? denominator)
    {
        if (!denominator.HasValue || denominator == Zero)
        {
            if (!numerator.HasValue || numerator == Zero)
            {
                return Percentage.Hundred;
            }

            return Percentage.Zero;
        }

        return new Percentage((numerator?.rawValue ?? 0) / denominator.Value.rawValue);
    }

    public static Money Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return new Money(decimal.Parse(s, provider));
    }

    public static Money Parse(string s, IFormatProvider? provider)
    {
        return new Money(decimal.Parse(s, provider));
    }

    public static Money Parse(string value) => new(decimal.Parse(value));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Money result)
    {
        if (decimal.TryParse(s, provider, out var id))
        {
            result = new Money(id);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Money result)
    {
        if (decimal.TryParse(s, provider, out var id))
        {
            result = new Money(id);
            return true;
        }

        result = default;
        return false;
    }

    public static Money? TryParse(string value)
    {
        if (decimal.TryParse(value, out var result))
        {
            return new Money(result);
        }

        return null;
    }

    public static bool TryParse(string value, out Money id)
    {
        if (decimal.TryParse(value, out var result))
        {
            id = new Money(result);
            return true;
        }

        id = default;
        return false;
    }

    public decimal ToDecimal() => TruncateToTwoDecimalPlaces(rawValue);

    public Money Normalize() => new(ToDecimal());

    /// <summary>
    /// Calculates the relative percentage where the current instance is considered the numerator, and the provided denominator is the denominator.
    /// </summary>
    /// <param name="denominator"></param>
    /// <returns>The relative percentage.</returns>
    public Percentage CalculateRelativePercentageFrom(Money denominator)
    {
        if (denominator == Zero)
        {
            return Percentage.Zero;
        }

        return new Percentage(rawValue / denominator.rawValue);
    }

    /// <summary>
    /// Calculates the relative percentage where the current instance is considered the denominator and the provided numerator is the numerator.
    /// </summary>
    /// <param name="numerator"></param>
    /// <returns>The relative percentage.</returns>
    public Percentage CalculateRelativePercentageTo(Money numerator)
    {
        if (this == Zero)
        {
            return Percentage.Zero;
        }

        return new Percentage(numerator.rawValue / rawValue);
    }

    public int CompareTo(Money other)
    {
        return rawValue.CompareTo(other.rawValue);
    }

    public int CompareTo(object? obj)
    {
        if (obj is Money id)
        {
            return rawValue.CompareTo(id.rawValue);
        }

        return rawValue.CompareTo(obj);
    }

    /// <summary>
    /// Gets returns the fractional (cents) part of the value as a long.
    /// </summary>
    public long FractionalValue => (long)((ToDecimal() % 1.0m) * 100);

    /// <summary>
    /// Gets returns the integer (dollars) part of the value as a long.
    /// </summary>
    public long IntegerValue => (long)ToDecimal();

    /// <summary>
    /// Gets returns the integer (dollars) part of the value as a long.
    /// </summary>
    public decimal Value => ToDecimal();

    /// <summary>
    /// Returns the value formatted in accounting style (optionally flipping sign and including currency symbol).
    /// </summary>
    public string ToAccounting(bool flipSign = false, bool includeCurrencySymbol = true)
    {
        var amount = ToDecimal();
        if (flipSign)
        {
            amount *= -1;
        }

        if (includeCurrencySymbol)
        {
            return amount.ToString("C", MoneyAccountingFormatInfo);
        }
        else
        {
            return amount.ToString("N", MoneyAccountingFormatInfo);
        }
    }

    /// <summary>
    /// Returns the value formatted as currency (e.g., $1,234.56).
    /// </summary>
    public string ToCurrency()
    {
        return ToDecimal().ToString("C", MoneyFormatInfo);
    }

    /// <summary>
    /// Returns the value formatted as a number string with group separators.
    /// </summary>
    public string ToNumberString() => ToDecimal().ToString("N", MoneyFormatInfo);

    /// <summary>
    /// No group separators.
    /// </summary>
    /// <returns>Returns the value formatted without group separators.</returns>
    /// <summary>
    /// Returns the value formatted as a plain number string (no group separators).
    /// </summary>
    public string ToPlainNumberString() => ToDecimal().ToString("N", MoneyNumericFormatInfo);

    /// <summary>
    /// Returns the value as a string.
    /// </summary>
    public override string ToString() => ToDecimal().ToString();

    /// <summary>
    /// Returns the value in words (e.g., "one hundred dollars and fifty cents").
    /// </summary>
    public string ToWords()
    {
        var dollars = IntegerValue;
        var cents = FractionalValue;

        var dollarsStr = dollars.ToWords();
        var centsStr = cents.ToWords();

        var result = dollarsStr + " dollars";
        if (cents > 0)
        {
            result += " and " + centsStr + " cents";
        }

        return result;
    }

    public class MoneyJsonConverter : JsonConverter<Money>
    {
        public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return Parse(reader.GetString());
            }

            if (reader.TryGetDecimal(out var dec))
            {
                return new Money(dec);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.rawValue.ToString());
    }

    // TypeConverter for Money to and from string and decimal
    public class MoneyTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
            sourceType == typeof(string) || sourceType == typeof(decimal) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
            destinationType == typeof(string) || destinationType == typeof(decimal) || base.CanConvertTo(context, destinationType);

        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                return Parse(s);
            }

            if (value is decimal d)
            {
                return new Money(d);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is Money money)
            {
                if (destinationType == typeof(string))
                {
                    return money.ToNumberString();
                }

                if (destinationType == typeof(decimal))
                {
                    return money.rawValue;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

public static class MoneyExtensions
{
    public static Money Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector) => new(source.Sum(s => selector(s).ToDecimal()));

    public static Money? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money?> selector)
    {
        decimal? result = source.Sum(s => selector(s)?.ToDecimal());
        if (result.HasValue)
        {
            return new Money(result.Value);
        }

        return null;
    }

    public static Money Sum(this IEnumerable<Money> source)
    {
        return source.Sum(s => s);
    }

    public static Money Sum(this IEnumerable<Money?> source)
    {
        return source.Sum(s => s ?? Money.Zero);
    }
}
