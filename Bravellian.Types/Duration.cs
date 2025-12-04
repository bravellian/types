// CONFIDENTIAL - Copyright (c) PayeWaive LLC. All rights reserved.
// See NOTICE.md for full restrictions and usage terms.


using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;

namespace Bravellian;
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
        Years = years;
        Months = months;
        Weeks = weeks;
        Days = days;
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;

        yearsString = Years > 0 ? $"{Years.Value}{YearsTag}" : string.Empty;
        monthsString = Months > 0 ? $"{Months.Value}{MonthsTag}" : string.Empty;
        weeksString = Weeks > 0 ? $"{Weeks.Value}{WeeksTag}" : string.Empty;
        daysString = Days > 0 ? $"{Days.Value}{DaysTag}" : string.Empty;
        hoursString = Hours > 0 ? $"{Hours.Value}{HoursTag}" : string.Empty;
        minutesString = Minutes > 0 ? $"{Minutes.Value}{MinutesTag}" : string.Empty;
        secondsString = Seconds > 0 ? $"{Seconds.Value}{SecondsTag}" : string.Empty;

        var dateString = yearsString + monthsString + weeksString + daysString;
        var timeString = hoursString + minutesString + secondsString;
        valueString = PeriodTag + dateString + (string.IsNullOrWhiteSpace(timeString) ? string.Empty : TimeTag + timeString);
    }

    public double? Years { get; }

    public double? Months { get; }

    public double? Weeks { get; }

    public double? Days { get; }

    public double? Hours { get; }

    public double? Minutes { get; }

    public double? Seconds { get; }

    public string Value => ToString();

    public static Duration From(string value) => Duration.Parse(value);

    public override string ToString() => valueString;

    public DateTimeOffset Calculate(DateTimeOffset start)
    {
        DateTimeOffset calulated = start;
        if (Years.HasValue)
        {
            calulated = calulated.AddYears(Integral(Years.Value));
            var frac = Fractional(Years.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddDays(Integral(DaysInYear * frac));
            }
        }

        if (Months.HasValue)
        {
            calulated = calulated.AddMonths(Integral(Months.Value));
            var frac = Fractional(Months.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddDays(Integral(30 * frac));
            }
        }

        if (Weeks.HasValue)
        {
            var weeksAsDays = Weeks.Value * DaysInWeek;
            calulated = calulated.AddDays(Integral(weeksAsDays));
            var frac = Fractional(weeksAsDays);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddHours(Integral(HoursInDay * frac));
            }
        }

        if (Days.HasValue)
        {
            calulated = calulated.AddDays(Integral(Days.Value));
            var frac = Fractional(Days.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddHours(Integral(HoursInDay * frac));
            }
        }

        if (Hours.HasValue)
        {
            calulated = calulated.AddHours(Integral(Hours.Value));
            var frac = Fractional(Hours.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddMinutes(Integral(MinutesInHour * frac));
            }
        }

        if (Minutes.HasValue)
        {
            calulated = calulated.AddMinutes(Integral(Minutes.Value));
            var frac = Fractional(Minutes.Value);
            if (AboutNotEqual(frac, 0))
            {
                calulated = calulated.AddSeconds(Integral(SecondsInMinute * frac));
            }
        }

        if (Seconds.HasValue)
        {
            calulated = calulated.AddSeconds(Integral(Seconds.Value));
            var frac = Fractional(Seconds.Value);
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
            return CompareTo(duration);
        }

        return 0;
    }

    public int CompareTo(Duration other)
    {
        return string.Compare(ToString(), other.ToString(), StringComparison.OrdinalIgnoreCase);
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
