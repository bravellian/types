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

namespace Bravellian.Types;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;

/// <summary>
/// This represents an ISO 8601 Duration.
/// </summary>
[JsonConverter(typeof(DurationJsonConverter))]
public readonly partial record struct Duration
    : IStringBackedType<Duration>
{
    private readonly string yearsString;
    private readonly string monthsString;
    private readonly string weeksString;
    private readonly string daysString;
    private readonly string hoursString;
    private readonly string minutesString;
    private readonly string secondsString;
    private readonly string valueString;

    private const string PeriodTag = "P";
    private const string TimeTag = "T";
    private const string YearsTag = "Y";
    private const string MonthsTag = "M";
    private const string WeeksTag = "W";
    private const string DaysTag = "D";
    private const string HoursTag = "H";
    private const string MinutesTag = "M";
    private const string SecondsTag = "S";

    private const int MillisecondsInSecond = 1000;
    private const int HoursInDay = 24;
    private const int MinutesInHour = 60;
    private const int SecondsInMinute = 60;
    private const int DaysInWeek = 7;
    private const int DaysInYear = 365;

    private const double Epsilon = 0.000001;

    /// <summary>
    /// Regex for matching the ISO 8601 Duration format. Adds the ability to have negative values, which is not explicity accounted for.
    /// </summary>
    private static readonly Regex ParserRegex = DurationRegexGen();

    public Duration(double? years, double? months, double? weeks, double? days, double? hours, double? minutes, double? seconds)
    {
        this.Years = years;
        this.Months = months;
        this.Weeks = weeks;
        this.Days = days;
        this.Hours = hours;
        this.Minutes = minutes;
        this.Seconds = seconds;

        this.yearsString = this.Years > 0 ? $"{this.Years.Value}{YearsTag}" : string.Empty;
        this.monthsString = this.Months > 0 ? $"{this.Months.Value}{MonthsTag}" : string.Empty;
        this.weeksString = this.Weeks > 0 ? $"{this.Weeks.Value}{WeeksTag}" : string.Empty;
        this.daysString = this.Days > 0 ? $"{this.Days.Value}{DaysTag}" : string.Empty;
        this.hoursString = this.Hours > 0 ? $"{this.Hours.Value}{HoursTag}" : string.Empty;
        this.minutesString = this.Minutes > 0 ? $"{this.Minutes.Value}{MinutesTag}" : string.Empty;
        this.secondsString = this.Seconds > 0 ? $"{this.Seconds.Value}{SecondsTag}" : string.Empty;

        var dateString = this.yearsString + this.monthsString + this.weeksString + this.daysString;
        var timeString = this.hoursString + this.minutesString + this.secondsString;
        this.valueString = PeriodTag + dateString + (string.IsNullOrWhiteSpace(timeString) ? string.Empty : TimeTag + timeString);
    }

    public double? Years { get; }

    public double? Months { get; }

    public double? Weeks { get; }

    public double? Days { get; }

    public double? Hours { get; }

    public double? Minutes { get; }

    public double? Seconds { get; }

    public override string ToString() => this.valueString;

    public DateTimeOffset Calculate(DateTimeOffset start)
    {
        DateTimeOffset calulated = start;
        if (this.Years.HasValue)
        {
            calulated = calulated.AddYears(Integral(this.Years.Value));
            var frac = Fractional(this.Years.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddDays(Integral(DaysInYear * frac));
            }
        }

        if (this.Months.HasValue)
        {
            calulated = calulated.AddMonths(Integral(this.Months.Value));
            var frac = Fractional(this.Months.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddDays(Integral(30 * frac));
            }
        }

        if (this.Weeks.HasValue)
        {
            var weeksAsDays = this.Weeks.Value * DaysInWeek;
            calulated = calulated.AddDays(Integral(weeksAsDays));
            var frac = Fractional(weeksAsDays);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddHours(Integral(HoursInDay * frac));
            }
        }

        if (this.Days.HasValue)
        {
            calulated = calulated.AddDays(Integral(this.Days.Value));
            var frac = Fractional(this.Days.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddHours(Integral(HoursInDay * frac));
            }
        }

        if (this.Hours.HasValue)
        {
            calulated = calulated.AddHours(Integral(this.Hours.Value));
            var frac = Fractional(this.Hours.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddMinutes(Integral(MinutesInHour * frac));
            }
        }

        if (this.Minutes.HasValue)
        {
            calulated = calulated.AddMinutes(Integral(this.Minutes.Value));
            var frac = Fractional(this.Minutes.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddSeconds(Integral(SecondsInMinute * frac));
            }
        }

        if (this.Seconds.HasValue)
        {
            calulated = calulated.AddSeconds(Integral(this.Seconds.Value));
            var frac = Fractional(this.Seconds.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddMilliseconds(Integral(MillisecondsInSecond * frac));
            }
        }

        return calulated;
    }

    public static Duration? TryParse(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Match match = ParserRegex.Match(value);

            if (match?.Success ?? false)
            {
                var years = TryParseGroup(match, "year");
                var months = TryParseGroup(match, "month");
                var weeks = TryParseGroup(match, "week");
                var days = TryParseGroup(match, "day");
                var hours = TryParseGroup(match, "hour");
                var minutes = TryParseGroup(match, "minute");
                var seconds = TryParseGroup(match, "second");

                return new Duration(years, months, weeks, days, hours, minutes, seconds);
            }
        }

        return null;
    }

    public static bool TryParse(string value, out Duration id)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Match match = ParserRegex.Match(value);

            if (match?.Success ?? false)
            {
                var years = TryParseGroup(match, "year");
                var months = TryParseGroup(match, "month");
                var weeks = TryParseGroup(match, "week");
                var days = TryParseGroup(match, "day");
                var hours = TryParseGroup(match, "hour");
                var minutes = TryParseGroup(match, "minute");
                var seconds = TryParseGroup(match, "second");

                id = new Duration(years, months, weeks, days, hours, minutes, seconds);
                return true;
            }
        }

        id = default;
        return false;
    }

    public static Duration Parse(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Match match = ParserRegex.Match(value);

            if (match?.Success ?? false)
            {
                var years = TryParseGroup(match, "year");
                var months = TryParseGroup(match, "month");
                var weeks = TryParseGroup(match, "week");
                var days = TryParseGroup(match, "day");
                var hours = TryParseGroup(match, "hour");
                var minutes = TryParseGroup(match, "minute");
                var seconds = TryParseGroup(match, "second");

                return new Duration(years, months, weeks, days, hours, minutes, seconds);
            }
        }

        throw new FormatException($"The value '{value}' is not a valid ISO 8601 Duration.");
    }

    private static double? TryParseGroup(Match match, string groupName)
    {
        if (match?.Groups[groupName]?.Success is true && double.TryParse(match.Groups[groupName].Value, out var val))
        {
            return val;
        }

        return default;
    }

    private static double Fractional(double x) => x - Math.Floor(x);

    private static int Integral(double x) => (int)Math.Floor(x);

    [GeneratedRegex(@"^P(?!$)(?>(?<year>-?\d+(?:\.\d+)?)Y)?(?>(?<month>-?\d+(?:\.\d+)?)M)?(?>(?<week>-?\d+(?:\.\d+)?)W)?(?>(?<day>-?\d+(?:\.\d+)?)D)?(?>T(?>(?<hour>-?\d+(?:\.\d+)?)H)?(?>(?<minute>-?\d+(?:\.\d+)?)M)?(?>(?<second>-?\d+(?:\.\d+)?)S)?)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled, 1000, "en-US")]
    private static partial Regex DurationRegexGen();

    private static bool AboutNotEqual(double x, double y)
    {
#pragma warning disable S1244 // Floating point numbers should not be tested for equality
        return (x != y) && (Math.Abs(x - y) >= Epsilon);
#pragma warning restore S1244 // Floating point numbers should not be tested for equality
    }

    public int CompareTo(object? obj)
    {
        if (obj is Duration duration)
        {
            return this.CompareTo(duration);
        }

        return 0;
    }

    public int CompareTo(Duration other)
    {
        return string.Compare(this.ToString(), other.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    public class DurationJsonConverter : JsonConverter<Duration>
    {
        public override Duration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (TryParse(reader.GetString(), out Duration duration))
            {
                return duration;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Duration value, JsonSerializerOptions options)
        {
            Guard.IsNotNull(writer);

            writer.WriteStringValue(value.ToString());
        }
    }
}
