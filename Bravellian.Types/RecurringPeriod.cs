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

/// <summary>
/// This represents a period of time that is recurring, defined using a cron expression.
/// </summary>
[JsonConverter(typeof(RecurringPeriodJsonConverter))]
public readonly record struct RecurringPeriod
{
    public RecurringPeriod(Cronos.CronExpression expression)
    {
        this.Expression = expression;
    }

    public Cronos.CronExpression Expression { get; }

    public Period GetPeriod(DateTime startUtc)
    {
        DateTimeOffset startDate = this.Expression.GetOccurrences(startUtc.AddDays(-60), startUtc, TimeZoneInfo.Utc).Last();
        DateTimeOffset? endDate = this.Expression.GetNextOccurrence(DateTimeOffset.UtcNow, TimeZoneInfo.Utc);
        if (!endDate.HasValue)
        {
            return default;
        }

        return new(startDate, endDate.Value);
    }

    public override string ToString() => this.Expression.ToString();

    public static RecurringPeriod? TryParse(string value)
    {
        try
        {
            return new RecurringPeriod(Cronos.CronExpression.Parse(value));
        }
        catch (Cronos.CronFormatException)
        {
            return null;
        }
    }

    public static bool TryParse(string value, out RecurringPeriod result)
    {
        try
        {
            result = new RecurringPeriod(Cronos.CronExpression.Parse(value));
            return true;
        }
        catch (Cronos.CronFormatException)
        {
            result = default;
            return false;
        }
    }

    public static RecurringPeriod Parse(string value)
    {
        return new RecurringPeriod(Cronos.CronExpression.Parse(value));
    }

    public class RecurringPeriodJsonConverter : JsonConverter<RecurringPeriod>
    {
        public override RecurringPeriod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (TryParse(reader.GetString(), out RecurringPeriod recurringPeriod))
            {
                return recurringPeriod;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, RecurringPeriod value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}
