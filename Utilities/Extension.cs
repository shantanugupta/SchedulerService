using SchedulerApi.Model;
using SchedulerApi.ApiContract;
using SchedulerApi.FunctionalLayer;

namespace SchedulerApi.Convertor
{

    internal static class RangeExtensions
    {
        public static bool In(this Range range, int value) => range.Start.Value <= value && value <= range.End.Value;
    }

    internal static class DateTimeHelper
    {
        public static DateTime Add(this DateTime input, int interval, FreqSubdayType type)
        {
            switch (type)
            {
                case FreqSubdayType.Seconds:
                    return input.AddSeconds(interval);
                case FreqSubdayType.Minutes:
                    return input.AddMinutes(interval);
                case FreqSubdayType.Hours:
                    return input.AddHours(interval);
                default:
                    throw new ScheduleException("FreqSubdayType not supported");
            }
        }

        public static DateTime Add(this DateTime input, int interval, MomentTimeValue type)
        {
            switch (type)
            {
                case MomentTimeValue.Second:
                    return input.AddSeconds(interval);
                case MomentTimeValue.Minutes:
                    return input.AddMinutes(interval);
                case MomentTimeValue.Hours:
                    return input.AddHours(interval);
                default:
                    throw new ScheduleException("MomentTimeValue not supported");
            }
        }

        public static DateTime To(this DateTime input, DateTo type)
        {
            switch (type)
            {
                case DateTo.StartOfDay:
                    return input.Date;
                case DateTo.EndOfDay:
                    return input.Date.AddDays(1).AddTicks(-1);
                case DateTo.StartOfMonth:
                    return new DateTime(input.Year, input.Month, 1);
                case DateTo.EndOfMonth:
                    return new DateTime(input.Year, input.Month, 1).AddMonths(1).AddSeconds(-1);
                case DateTo.StartOfYear:
                    return new DateTime(input.Year, 1, 1);
                case DateTo.EndOfYear:
                    return new DateTime(input.Year, 1, 1).AddYears(1).AddSeconds(-1);
                default:
                    throw new ScheduleException("Date conversion not supported");
            }
        }

    }

    internal static class Extension
    {
        internal static T ConvertTo<T>(this ScheduleContract from) where T : BaseModel
        {
            BaseModel to = null;
            Type type = typeof(T);

            if (nameof(Schedule) == typeof(T).Name)
                to = from.Convert();
            else if (nameof(ScheduleEvent) == typeof(T).Name)
                to = from.Convert();

            return (T)to;
        }

        private static Schedule Convert(this ScheduleContract from)
        {
            var to = new Schedule
            {
                //ScheduleId = from.ScheduleId,
                //VersionNumber = from.VersionNumber,
                Name = from.Name,
                Description = from.Description,
                FreqType = from.FreqType,
                FreqInterval = from.FreqInterval,
                FreqSubdayType = from.FreqSubdayType,
                FreqSubdayInterval = from.FreqSubdayInterval,
                FreqRelativeInterval = from.FreqRelativeInterval,
                FreqRecurrenceFactor = from.FreqRecurrenceFactor,
                DurationSubdayType = from.DurationSubdayType,
                DurationInterval = from.DurationInterval,
                ActiveStartDate = DateOnly.Parse(from.ActiveStartDate),
                ActiveEndDate = DateOnly.Parse(from.ActiveEndDate),
                ActiveStartTime = TimeOnly.Parse(from.ActiveStartTime),
                ActiveEndTime = TimeOnly.Parse(from.ActiveEndTime),
                OccuranceChoiceState = from.OccuranceChoiceState
            };

            return to;
        }

        private static ScheduleContract Convert(this Schedule from)
        {
            var to = new ScheduleContract
            {
                //ScheduleId = from.ScheduleId,
                //VersionNumber = from.VersionNumber,
                Name = from.Name,
                Description = from.Description,
                FreqType = from.FreqType,
                FreqInterval = from.FreqInterval,
                FreqSubdayType = from.FreqSubdayType,
                FreqSubdayInterval = from.FreqSubdayInterval,
                FreqRelativeInterval = from.FreqRelativeInterval,
                FreqRecurrenceFactor = from.FreqRecurrenceFactor,
                DurationSubdayType = from.DurationSubdayType,
                DurationInterval = from.DurationInterval,
                ActiveStartDate = from.ActiveStartDate.ToString("yyyymmdd"),
                ActiveEndDate = from.ActiveEndDate.ToString("yyyymmdd"),
                ActiveStartTime = from.ActiveStartTime.ToString("HHMM"),
                ActiveEndTime = from.ActiveEndTime.ToString("HHMM"),
                OccuranceChoiceState = from.OccuranceChoiceState
            };

            return to;
        }
    }
}

