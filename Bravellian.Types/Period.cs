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

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Diagnostics;

/// <summary>
/// This represents an ISO 8601 Time Interval, of the format {DateTime}/{Duration}.
/// </summary>
[JsonConverter(typeof(PeriodJsonConverter))]
public readonly record struct Period
{
    public Period(DateTimeOffset start, DateTimeOffset end)
    {
        this.StartInclusive = start;
        this.EndExclusive = end;
        if (this.EndExclusive < this.StartInclusive)
        {
            throw new ArgumentException("The end of the period cannot be before the start.", nameof(end));
        }

        this.EndInclusive = this.EndExclusive - TimeSpan.FromTicks(1);

        TimeSpan difference = end - start;
        this.Duration = new Duration(null, null, null, difference.Days, difference.Hours, difference.Minutes, difference.Seconds);
    }

    public Period(DateTimeOffset start, Duration duration)
    {
        this.StartInclusive = start;
        this.Duration = duration;
        this.EndExclusive = duration.Calculate(start);

        if (this.EndExclusive < this.StartInclusive)
        {
            throw new ArgumentException("The duration cannot result in the end of the period before the start.", nameof(duration));
        }

        this.EndInclusive = this.EndExclusive - TimeSpan.FromTicks(1);
    }

    public Duration Duration { get; }

    public DateTimeOffset EndExclusive { get; }

    public DateTimeOffset EndInclusive { get; }

    public DateTimeOffset StartInclusive { get; }

    public static Period Parse(string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);

        var split = value.Split('/');
        if (split.Length != 2)
        {
            throw new FormatException($"The value '{value}' is not a valid ISO 8601 Time Interval.");
        }
        else
        {
            if (DateTimeOffset.TryParse(split[0], DateTimeFormatInfo.InvariantInfo, out DateTimeOffset start) &&
                Duration.TryParse(split[1], out Duration duration))
            {
                return new Period(start, duration);
            }
            else
            {
                throw new FormatException($"The value '{value}' is not a valid ISO 8601 Time Interval.");
            }
        }
    }

    public static Period? TryParse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var split = value.Split('/');
        if (split.Length != 2)
        {
            return null;
        }
        else
        {
            if (DateTimeOffset.TryParse(split[0], DateTimeFormatInfo.InvariantInfo, out DateTimeOffset start) &&
                Duration.TryParse(split[1], out Duration duration))
            {
                return new Period(start, duration);
            }
            else
            {
                return null;
            }
        }
    }

    public static bool TryParse(string value, out Period result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        var split = value.Split('/');
        if (split.Length != 2)
        {
            result = default;
            return false;
        }

        if (DateTimeOffset.TryParse(split[0], DateTimeFormatInfo.InvariantInfo, out DateTimeOffset start) && Duration.TryParse(split[1], out Duration duration))
        {
            result = new Period(start, duration);
            return true;
        }

        result = default;
        return false;
    }

    public DateOnly ClampDate(bool inclusive = false)
    {
        DateTimeOffset date = inclusive ? this.EndInclusive : this.EndExclusive;
        return new DateOnly(date.Year, date.Month, date.Day);
    }

    public bool Contains(DateTimeOffset date) => date >= this.StartInclusive && date <= this.EndExclusive;

    public bool Overlaps(in Period other) => this.Contains(other.StartInclusive) || this.Contains(other.EndExclusive) || other.Contains(this.StartInclusive) || other.Contains(this.EndExclusive);

    public override string ToString() => $"{this.StartInclusive:O}/{this.Duration}";

    public class PeriodJsonConverter : JsonConverter<Period>
    {
        public override Period Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TryParse(reader.GetString(), out Period period) ? period : throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Period value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());

        public override void WriteAsPropertyName(
            Utf8JsonWriter writer,
            Period value,
            JsonSerializerOptions options) =>
                writer.WritePropertyName(value.ToString());

        public override Period ReadAsPropertyName(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                this.Read(ref reader, typeToConvert, options);
    }
}
