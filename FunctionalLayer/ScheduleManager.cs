using SchedulerApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using SchedulerApi.ApiContract;
using SchedulerApi.Convertor;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Runtime.Serialization;

namespace SchedulerApi.FunctionalLayer
{
    /// <summary>
    /// This class is going to do all heavy lifting for managing a schedule.
    /// A typical schedule would have following functionalities
    /// 1. Create schedule
    /// 2. Update schedule
    /// 3. Get schedule
    /// 4. Delete schedule
    /// 5. Validate schedule
    /// 6. Generate events
    /// 7. Generate description
    /// 8. Filter schedule
    /// </summary>
    internal class ScheduleManager
    {
        #region Management functions

        /// <summary>
        /// This function would validate all values presently set in this schedule. If the schedule is valid, response object would have flag set to true else false.
        /// </summary>
        /// <param name="schedule">Schedule to be validated</param>
        internal static Response<Schedule> ValidateSchedule(Schedule schedule)
        {
            Response<Schedule> response = new()
            {
                Error = IsScheduleValid(schedule)
            };

            return response;
        }

        /// <summary>
        /// Generate all the events that can occur for schedule provided as input
        /// </summary>
        /// <param name="schedule">Schedule for which events have to be generated</param>
        internal static IEnumerable<ScheduleEvent> GenerateEvents(Schedule schedule)
        {
            return GenerateEventsHelper(schedule);
        }


        /// <summary>
        /// This function generates description of the schedule.
        /// </summary>
        /// <param name="schedule">Schedule for which description will be generated</param>
        /// <returns>Description of a schedule</returns>
        internal static Response<string> GenerateDescription(Schedule schedule)
        {
            Response<string> response = new();
            var isValid = IsScheduleValid(schedule);
            response.Error = isValid;

            //Invalid schedule found? return
            if (isValid.Count > 0 && !isValid.TryGetValue(0, out _))
                return response;

            //We have a valid schedule here. Let's generate it's description
            string scheduleDescription = GenerateScheduleDescription(schedule);

            response.Entity = scheduleDescription;

            return response;
        }

        internal static Response<Schedule> Add(Schedule input)
        {
            Response<Schedule> response = new();

            var i = input;
            var isValid = IsScheduleValid(input);

            if (!isValid.TryGetValue(0, out _))
            {
                foreach (var kv in isValid)
                    response.Error.Add(kv.Key, kv.Value);
                return response;
            }

            return response;
        }

        private static IDictionary<int, string> IsScheduleValid(Schedule input)
        {
            var i = input;
            Dictionary<int, string> errorList = new();

            if (i.ActiveStartDate > i.ActiveEndDate)
            {
                errorList.Add(1, "ActiveStartDate cannot be greater than ActiveEndDate");
            }

            string messagePrefix = "For schedule";
            switch (i.FreqType)
            {
                case (int)FreqType.OneTimeOnly:
                    messagePrefix = "For OneTime Schedule";
                    if (i.FreqInterval != 0)
                        errorList.Add(2, $"{messagePrefix}, FreqInterval must be 0");
                    break;
                case (int)FreqType.Daily:
                    messagePrefix = "For Daily Schedule";
                    break;
                case (int)FreqType.Weekly:
                    messagePrefix = "For Weekly Schedule";
                    if (!(0..127).In(i.FreqInterval))
                        errorList.Add(3, $"{messagePrefix}, FreqInterval(day of week(s) selection) must be valid weekdays(1-127)");
                    if (!(1..100).In(i.FreqRecurrenceFactor))
                        errorList.Add(4, $"{messagePrefix}, FreqRecurrenceFactor(nth week) must be between (1-100)");
                    break;
                case (int)FreqType.Monthly:
                    messagePrefix = "For Monthly Schedule";
                    if (!(1..31).In(i.FreqInterval))
                        errorList.Add(5, $"{messagePrefix}, FreqInterval(nth day) must be valid day of the month(1-31)");
                    if (!(1..60).In(i.FreqRecurrenceFactor))
                        errorList.Add(6, $"{messagePrefix}, FreqRecurrenceFactor(nth month) must be recurrance month(1-60)");
                    break;
                case (int)FreqType.MonthlyRelativeToFreqInterval:
                    messagePrefix = "For Monthly relative Schedule";
                    if (!Enum.IsDefined((FreqRelativeInterval)i.FreqRelativeInterval))
                        errorList.Add(7, $"{messagePrefix}, FreqRelativeInterval(1st, 2nd, 3rd...last) must be one of the valid value(1,2,4,8,16)");

                    if (!Enum.IsDefined((FreqIntervalMonthlyRelative)i.FreqInterval))
                        errorList.Add(8, $"{messagePrefix}, FreqInterval(Weekday/Weekend etc) must be one of the valid value(1-10)");

                    if (!(1..60).In(i.FreqRecurrenceFactor))
                        errorList.Add(9, $"{messagePrefix}, FreqRecurrenceFactor(nth month) must be recurrance month(1-60)");
                    break;
                default:
                    errorList.Add(15, $"{messagePrefix}, Invalid FreqTpe(Schedule type i.e. daily, weekly..). Valid values are (1,4,8,16,32)");
                    break;
            }

            if (i.FreqType != (int)FreqType.OneTimeOnly)
            {
                var freqErrorList = IsFrequencyScheduleValid(i, messagePrefix);
                return errorList.Union(freqErrorList).ToDictionary(kv => kv.Key, kv => kv.Value);

            }

            return errorList;
        }

        private static IDictionary<int, string> IsFrequencyScheduleValid(Schedule input, string messagePrefix)
        {
            Dictionary<int, string> errorList = new();

            var i = input;

            if (i.ActiveStartDate > i.ActiveEndDate)
                errorList.Add(10, $"{messagePrefix}, ActiveStartDate must be less than to ActiveEndDate");
            if (i.OccuranceChoiceState == true && i.ActiveStartTime != i.ActiveEndTime)
                errorList.Add(11, $"{messagePrefix}, ActiveStartTime must be Equal to ActiveEndTime");
            if (i.OccuranceChoiceState == false)
            {
                int[] allowedSubdayType = { (int)FreqSubdayType.Hours, (int)FreqSubdayType.Minutes };

                if (!allowedSubdayType.Contains(i.FreqSubdayType)) // i.FreqSubdayInterval
                    errorList.Add(12, $"{messagePrefix}, FreqSubdayType(Type of occurance min/hour) must be (4/8)");
                else if (i.FreqSubdayType == (int)FreqSubdayType.Hours && !(1..24).In(i.FreqSubdayInterval))
                    errorList.Add(13, $"{messagePrefix}, FreqSubdayInterval(nth hour) must be between 1-24 hours");
                else if (i.FreqSubdayType == (int)FreqSubdayType.Minutes && !(1..60).In(i.FreqSubdayInterval))
                    errorList.Add(14, $"{messagePrefix}, FreqSubdayInterval(nth minute) must be between 1-60 minutes");
            }

            return errorList;
        }

        /// <summary>
        /// This function copies a logic of stored procedure written in sql server. Procedure name sp_get_schedule_description
        /// </summary>
        /// <returns>Message to be displayed in TextBox of UI</returns>
        private static string GenerateScheduleDescription(Schedule schedule)
        {
            string desc = string.Empty;

            var s = schedule;

            FreqType f = (FreqType)s.FreqType;

            switch (f)
            {
                case FreqType.OneTimeOnly:
                    desc = "Once on " + s.ActiveStartDate.ToString() + " at "
                        + s.ActiveStartTime.ToString();
                    break;
                case FreqType.Daily:
                    desc = "Every day ";
                    break;
                case FreqType.Weekly:
                    desc = "Every " + s.FreqRecurrenceFactor + " week(s) on ";
                    byte loop = 1;
                    while (loop <= 7)
                    {
                        int power = (int)System.Math.Pow(2, loop - 1);
                        if ((s.FreqInterval & power) == power)
                        {
                            desc = desc + DateTime.Parse("1996/12/0" + (loop + 1).ToString()).DayOfWeek.ToString();
                            loop++;
                        }
                    }
                    if (desc.EndsWith(", ") == true)
                    {
                        //Line 43 for reference, procedure name sp_get_schedule_description
                        //SELECT @schedule_description = SUBSTRING(@schedule_description, 1, (DATALENGTH(@schedule_description) / 2) - 2) + N' '
                        desc = desc.Substring(1, desc.Length - 2) + " ";
                    }
                    break;
                case FreqType.Monthly:
                    desc = "Every " + s.FreqRecurrenceFactor.ToString() + " months(s) on day " + s.FreqInterval.ToString() + " of that month ";
                    break;
                case FreqType.MonthlyRelativeToFreqInterval:
                    desc = "Every " + s.FreqRecurrenceFactor.ToString() + " months(s) on the ";

                    string freq_rel_intv = string.Empty;
                    FreqRelativeInterval fri = (FreqRelativeInterval)s.FreqRelativeInterval;
                    switch (fri)
                    {
                        case FreqRelativeInterval.First:
                            freq_rel_intv = freq_rel_intv + "first ";
                            break;
                        case FreqRelativeInterval.Second:
                            freq_rel_intv = freq_rel_intv + "second ";
                            break;
                        case FreqRelativeInterval.Third:
                            freq_rel_intv = freq_rel_intv + "third ";
                            break;
                        case FreqRelativeInterval.Fourth:
                            freq_rel_intv = freq_rel_intv + "fourth ";
                            break;
                        case FreqRelativeInterval.Last:
                            freq_rel_intv = freq_rel_intv + "last ";
                            break;
                    }

                    string freq_intv_str = string.Empty;
                    FreqIntervalMonthlyRelative fimr = (FreqIntervalMonthlyRelative)s.FreqInterval;

                    if (fimr > FreqIntervalMonthlyRelative.Sunday && fimr < FreqIntervalMonthlyRelative.Day)
                    {
                        freq_intv_str = DateTime.Parse("1996/12/0" + (s.FreqInterval + 1).ToString()).DayOfWeek.ToString();
                    }
                    else if (fimr == FreqIntervalMonthlyRelative.Day)
                    {
                        freq_intv_str = "day";
                    }
                    else if (fimr == FreqIntervalMonthlyRelative.Weekday)
                    {
                        freq_intv_str = "week day";
                    }
                    else if (fimr == FreqIntervalMonthlyRelative.Day)
                    {
                        freq_intv_str = "weekend day";
                    }
                    desc = desc + freq_intv_str + freq_intv_str + " of that month ";
                    break;
            }//END SWITCH FreqType variations

            string FreqSubdayType_str = string.Empty;
            FreqSubdayType fst = (FreqSubdayType)s.FreqSubdayType;
            switch (fst)
            {
                case FreqSubdayType.AtTheSpecifiedTime:
                    FreqSubdayType_str = "at " + s.ActiveStartTime.ToString();
                    break;
                case FreqSubdayType.Seconds:
                    FreqSubdayType_str = "every " + s.FreqSubdayInterval.ToString() + " second(s)";
                    break;
                case FreqSubdayType.Minutes:
                    FreqSubdayType_str = "every " + s.FreqSubdayInterval.ToString() + " minute(s)";
                    break;
                case FreqSubdayType.Hours:
                    FreqSubdayType_str = "every " + s.FreqSubdayInterval.ToString() + " hour(s)";
                    break;
            }

            desc = desc + FreqSubdayType_str;
            if (fst == FreqSubdayType.Hours || fst == FreqSubdayType.Minutes || fst == FreqSubdayType.Seconds)
                desc = desc + " between " + s.ActiveStartTime + " and " + s.ActiveEndTime;

            return desc;
        }

        #endregion

        private static IEnumerable<ScheduleEvent> GenerateEventsHelper(Schedule schedule)
        {
            var sch = schedule;
            var events = new Stack<ScheduleEvent>();
            #region Logic for events generation

            var timeFormat = "hh:mm A";
            var dateFormat = "YYYY/MM/DD";
            var dateTimeFormat = dateFormat + ' ' + timeFormat;


            // var active_start_time_string = sch.ActiveStartTime == undefined ? "[start time not provided]" : moment(sch.ActiveStartTime).format(timeFormat);
            // var active_end_time_string = sch.ActiveEndTime == undefined ? "[end time not provided]" : moment(sch.ActiveEndTime).format(timeFormat);
            // var active_start_date_string = sch.ActiveStartDate == undefined ? "[start date not provided]" : moment(sch.ActiveStartDate).format(dateFormat);
            // var active_end_date_string = sch.ActiveEndDate == undefined ? "[end date not provided]" : moment(sch.ActiveEndDate).format(dateFormat);

            var initialDate = "1900-01-01";
            var defaultDate = DateTime.Parse(initialDate);

            var moment_active_start_time = DateTime.Parse(initialDate + " " + sch.ActiveStartTime);

            //if end time is smaller than start time, this means event is spill over to day 2 e.g. event starting at 9 pm and would end by 3 am
            //var isBefore = moment(sch.ActiveEndTime, "HH:mm").isBefore(moment(sch.ActiveStartTime, "HH:mm"));
            var isBefore = sch.ActiveEndTime - sch.ActiveStartTime;
            if (isBefore < TimeSpan.Zero)
            {
                defaultDate = DateTime.Parse(initialDate).AddDays(1);
            }
            var moment_active_end_time = defaultDate + sch.ActiveEndTime.ToTimeSpan();

            var endTimeInSeconds = (moment_active_end_time - defaultDate).TotalSeconds;
            var startTimeInSeconds = (moment_active_start_time - defaultDate).TotalSeconds;

            var f = sch.FreqType;

            switch (f)
            {
                case 1: //FreqType.OneTimeOnly:

                    var startDate = sch.ActiveStartDate.ToDateTime(sch.ActiveStartTime);
                    var endDate = sch.ActiveEndDate.ToDateTime(sch.ActiveEndTime);


                    if (sch.DurationInterval > 0)
                    {
                        var a = (FreqSubdayType)sch.DurationSubdayType;

                        if (a > FreqSubdayType.AtTheSpecifiedTime)
                            endDate = startDate.Add(sch.DurationInterval, a);
                    }
                    events.Push(new ScheduleEvent { StartDate = startDate, EndDate = endDate });

                    break;
            }

            #endregion


            return events;
        }
    }
}
