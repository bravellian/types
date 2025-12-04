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
using System.Text.RegularExpressions;

namespace Bravellian.Types;
/// <summary>
/// Represents dates with values ranging from January, 0001 Anno Domini (Common Era) through December, 9999 A.D. (C.E.) in the Gregorian calendar.
/// </summary>
[JsonConverter(typeof(MonthOnlyJsonConverter))]
[TypeConverter(typeof(MonthOnlyTypeConverter))]
public readonly partial struct MonthOnly
    : IComparable,
      IComparable<MonthOnly>,
      IEquatable<MonthOnly>,
      IParsable<MonthOnly>,
      IBvParsable<MonthOnly>
{
    public const int MonthsInYear = 12;

    // Maps to Jan 1st year 1
    private const int MinMonthNumber = 0;

    // Maps to December 31 year 9999. The value calculated from "MonthNumberFromDateOnly(new DateOnly(9999, 12, 31))"
    private const int MaxMonthNumber = 119_987;

    private static readonly Regex MonthNumberRegex = MonthParseRegex();

    private readonly int monthNumber;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonthOnly"/> struct.
    /// Creates a new instance of the MonthOnly structure to the specified year, month, and day.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    public MonthOnly(int year, int month) => monthNumber = MonthNumberFromDateOnly(new DateOnly(year, month, 1));

    private MonthOnly(int monthNumber)
    {
        this.monthNumber = monthNumber;
    }

    /// <summary>
    /// Gets the earliest possible date that can be created.
    /// </summary>
    public static MonthOnly MinValue => new(MinMonthNumber);

    /// <summary>
    /// Gets the latest possible date that can be created.
    /// </summary>
    public static MonthOnly MaxValue => new(MaxMonthNumber);

    /// <summary>
    /// Gets the year component of the date represented by this instance.
    /// </summary>
    public int Year => GetEquivalentDateOnly().Year;

    /// <summary>
    /// Gets the month component of the date represented by this instance.
    /// </summary>
    public int Month => GetEquivalentDateOnly().Month;

    /// <summary>
    /// Gets the number of months since January 1, 0001 in the Proleptic Gregorian calendar represented by this instance.
    /// </summary>
    public int MonthNumber => monthNumber;

    /// <summary>
    /// Determines whether two specified instances of MonthOnly are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if left and right represent the same date; otherwise, false.</returns>
    public static bool operator ==(MonthOnly left, MonthOnly right) => left.monthNumber == right.monthNumber;

    /// <summary>
    /// Determines whether two specified instances of MonthOnly are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if left and right do not represent the same date; otherwise, false.</returns>
    public static bool operator !=(MonthOnly left, MonthOnly right) => left.monthNumber != right.monthNumber;

    /// <summary>
    /// Determines whether one specified MonthOnly is later than another specified DateTime.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if left is later than right; otherwise, false.</returns>
    public static bool operator >(MonthOnly left, MonthOnly right) => left.monthNumber > right.monthNumber;

    /// <summary>
    /// Determines whether one specified MonthOnly represents a date that is the same as or later than another specified MonthOnly.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if left is the same as or later than right; otherwise, false.</returns>
    public static bool operator >=(MonthOnly left, MonthOnly right) => left.monthNumber >= right.monthNumber;

    /// <summary>
    /// Determines whether one specified MonthOnly is earlier than another specified MonthOnly.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if left is earlier than right; otherwise, false.</returns>
    public static bool operator <(MonthOnly left, MonthOnly right) => left.monthNumber < right.monthNumber;

    /// <summary>
    /// Determines whether one specified MonthOnly represents a date that is the same as or earlier than another specified MonthOnly.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if left is the same as or earlier than right; otherwise, false.</returns>
    public static bool operator <=(MonthOnly left, MonthOnly right) => left.monthNumber <= right.monthNumber;

    /// <summary>
    /// Creates a new instance of the MonthOnly structure to the specified number of days.
    /// </summary>
    /// <param name="monthNumber">The number of days since January 1, 0001 in the Proleptic Gregorian calendar.</param>
    /// <returns>The MonthOnly type from the month number.</returns>
    public static MonthOnly FromMonthNumber(int monthNumber)
    {
        return (uint)monthNumber > MaxMonthNumber
            ? throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month number is not within the acceptable range.")
            : new MonthOnly(monthNumber);
    }

    /// <summary>
    /// Returns a MonthOnly instance that is set to the date part of the specified dateTime.
    /// </summary>
    /// <param name="dateTime">The DateTime instance.</param>
    /// <returns>The MonthOnly instance composed of the date part of the specified input time dateTime instance.</returns>
    public static MonthOnly FromDateTime(DateTime dateTime) => new(dateTime.Year, dateTime.Month);

    /// <summary>
    /// Returns a MonthOnly instance that is set to the date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The MonthOnly instance composed of the date.</returns>
    public static MonthOnly FromDate(DateOnly date) => new(MonthNumberFromDateOnly(date));

    public static MonthOnly? TryParse(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Match match = MonthNumberRegex.Match(value);

            if (match?.Success ?? false)
            {
                var year = TryParseGroup(match, "year");
                var month = TryParseGroup(match, "month");

                if (year.HasValue && month.HasValue)
                {
                    return new MonthOnly(year.Value, month.Value);
                }
            }
            else if (DateTime.TryParse(value, DateTimeFormatInfo.InvariantInfo, out DateTime parsedDate))
            {
                return FromDateTime(parsedDate);
            }
        }

        return null;
    }

    public static bool TryParse(string? value, out MonthOnly id) => TryParse(value, null, out id);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out MonthOnly id)
    {
        MonthOnly? result = TryParse(s);
        if (result.HasValue)
        {
            id = result.Value;
            return true;
        }

        id = default;
        return false;
    }

    public static MonthOnly Parse(string value) => Parse(value, null);

    public static MonthOnly Parse(string s, IFormatProvider? provider) => TryParse(s) ?? throw new FormatException("Invalid MonthOnly format");

    /// <summary>
    /// Adds the specified number of days to the value of this instance.
    /// </summary>
    /// <param name="value">The number of days to add. To subtract days, specify a negative number.</param>
    /// <returns>An instance whose value is the sum of the date represented by this instance and the number of days represented by value.</returns>
    public MonthOnly AddMonths(int value)
    {
        var newMonthNumber = monthNumber + value;
        return (uint)newMonthNumber > MaxMonthNumber
            ? throw new ArgumentOutOfRangeException(nameof(value), "Total months exceeds the acceptable value.")
            : new MonthOnly(newMonthNumber);
    }

    /// <summary>
    /// Adds the specified number of years to the value of this instance.
    /// </summary>
    /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
    /// <returns>An object whose value is the sum of the date represented by this instance and the number of years represented by value.</returns>
    public MonthOnly AddYears(int value) => new(MonthNumberFromDateOnly(GetEquivalentDateOnly().AddYears(value)));

    /// <summary>
    /// Returns a DateTime that is set to the date of this MonthOnly instance and the time of specified input time.
    /// </summary>
    /// <param name="time">The time of the day.</param>
    /// <returns>The DateTime instance composed of the date of the current MonthOnly instance and the time specified by the input time.</returns>
    public DateTime ToDateTime(TimeOnly time) => GetEquivalentDateOnly().ToDateTime(time);

    /// <summary>
    /// Returns a DateTime instance with the specified input kind that is set to the date of this MonthOnly instance and the time of specified input time.
    /// </summary>
    /// <param name="time">The time of the day.</param>
    /// <param name="kind">One of the enumeration values that indicates whether ticks specifies a local time, Coordinated Universal Time (UTC), or neither.</param>
    /// <returns>The DateTime instance composed of the date of the current MonthOnly instance and the time specified by the input time.</returns>
    public DateTime ToDateTime(TimeOnly time, DateTimeKind kind) => GetEquivalentDateOnly().ToDateTime(time, kind);

    /// <summary>
    /// Compares the value of this instance to a specified MonthOnly value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified DateTime value.
    /// </summary>
    /// <param name="obj">The object to compare to the current instance.</param>
    /// <returns>Less than zero if this instance is earlier than value. Greater than zero if this instance is later than value. Zero if this instance is the same as value.</returns>
    public int CompareTo(MonthOnly obj) => monthNumber.CompareTo(obj.monthNumber);

    /// <summary>
    /// Compares the value of this instance to a specified object that contains a specified MonthOnly value, and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified MonthOnly value.
    /// </summary>
    /// <param name="obj">A boxed object to compare, or null.</param>
    /// <returns>Less than zero if this instance is earlier than value. Greater than zero if this instance is later than value. Zero if this instance is the same as value.</returns>
    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        return obj is not MonthOnly monthOnly ? throw new ArgumentException("Argument must be MonthOnly", nameof(obj)) : CompareTo(monthOnly);
    }

    /// <summary>
    /// Returns a value indicating whether the value of this instance is equal to the value of the specified MonthOnly instance.
    /// </summary>
    /// <param name="other">The object to compare to this instance.</param>
    /// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
    public bool Equals(MonthOnly other) => monthNumber == other.monthNumber;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">The object to compare to this instance.</param>
    /// <returns>true if value is an instance of MonthOnly and equals the value of this instance; otherwise, false.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is MonthOnly monthOnly && monthNumber == monthOnly.monthNumber;

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => monthNumber;

    public override string ToString() => GetEquivalentDateOnly().ToString("yyyy-MM");

    private static int MonthNumberFromDateOnly(DateOnly dt) => ((dt.Year - 1) * MonthsInYear) + dt.Month - 1;

    private static int? TryParseGroup(Match match, string groupName)
    {
        return match?.Groups[groupName]?.Success is true && int.TryParse(match.Groups[groupName].Value, out var val) ? val : (int?)default;
    }

    [GeneratedRegex("^(?<year>\\d{2}|\\d{4})-(?<month>\\d{1,2})$", RegexOptions.Compiled, 2000)]
    private static partial Regex MonthParseRegex();

    private DateOnly GetEquivalentDateOnly() => new DateOnly((monthNumber / MonthsInYear) + 1, (monthNumber % MonthsInYear) + 1, 1);

    public class MonthOnlyJsonConverter : JsonConverter<MonthOnly>
    {
        public override MonthOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return Parse(reader.GetString());
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, MonthOnly value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }

    // TypeConverter for MonthOnly to and from string and decimal
    public class MonthOnlyTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
            sourceType == typeof(string) || sourceType == typeof(decimal) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
            destinationType == typeof(string) || destinationType == typeof(decimal) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                return Parse(s, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is MonthOnly monthOnly && destinationType == typeof(string))
            {
                return monthOnly.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
