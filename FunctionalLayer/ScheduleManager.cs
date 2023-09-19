﻿using SchedulerApi.DataAccesslayer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using SchedulerApi.DataContract;

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
    public class ScheduleManager
    {
        /// <summary>
        /// This function would validate all values presently set in this schedule. If the schedule is valid, response object would have flag set to true else false.
        /// </summary>
        /// <param name="schedule">Schedule to be validated</param>
        public static Response<Schedule> ValidateSchedule(Schedule schedule)
        {
            Response<Schedule> response = new Response<Schedule>();
            response.Error = IsScheduleValid(schedule);

            return response;
        }

        /// <summary>
        /// Generate all the events that can occur for schedule provided as input
        /// </summary>
        /// <param name="schedule">Schedule for which events have to be generated</param>
        public static IEnumerable<ScheduleEvent> GenerateEvents(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function generates description of the schedule.
        /// </summary>
        /// <param name="schedule">Schedule for which description will be generated</param>
        /// <returns>Description of a schedule</returns>
        public static Response<string> GenerateDescription(Schedule schedule)
        {
            Response<string> response = new Response<string>();
            var isValid = IsScheduleValid(schedule);
            response.Error = isValid;

            //Invalid schedule found? return
            if (isValid.Key != 0) {
                var scheduleException = new ScheduleException("Schedule parsing failure");
                scheduleException.Data.Add(isValid.Key, isValid.Value);
                //throw scheduleException;
                return response;
            }

            //We have a valid schedule here. Let's generate it's description
            string scheduleDescription = GenerateScheduleDescription(schedule);

            response.Entity = scheduleDescription;

            return response;
        }

        public static Response<Schedule> Add(Schedule input)
        {
            Response<Schedule> response = new Response<Schedule>();

            var i = input;
            var isValid = IsScheduleValid(input);

            if (isValid.Key > 0)
            {
                response.Error = isValid;
                return response;
            }

            var s = new Schedule
            {
                ActiveEndDate = int.Parse(i.ActiveEndDate.ToString("yyyyMMdd")),
                ActiveEndTime = int.Parse(i.ActiveEndTime.ToString("HHmmss")),
                ActiveStartDate = int.Parse(i.ActiveStartDate.ToString("yyyyMMdd")),
                ActiveStartTime = int.Parse(i.ActiveStartTime.ToString("HHmmss")),
                Description = i.Description,
                DurationInterval = i.DurationInterval,
                DurationSubdayType = i.DurationSubdayType,
                FreqInterval = i.FreqInterval,
                FreqRecurrenceFactor = i.FreqRecurrenceFactor,
                FreqRelativeInterval = i.FreqRelativeInterval,
                FreqSubdayInterval = i.FreqSubdayInterval,
                FreqSubdayType = i.FreqSubdayType,
                FreqType = i.FreqType,
                Name = i.Name
            };

            return response;
        }

        private static KeyValuePair<int, string> IsScheduleValid(Schedule input)
        {
            var i = input;


            //if (i.ActiveStartDate > i.ActiveEndDate)
            //{
            //    return new KeyValuePair<int, string>(81, "ActiveStartDate cannot be greater than ActiveEndDate");
            //}

            //if (i.ActiveStartDate < DateTime.UtcNow.Date)
            //{
            //    return new KeyValuePair<int, string>(82, "ActiveStartDate should be greater than current date");
            //}

            //if (i.ActiveEndDate < DateTime.UtcNow)
            //{
            //    return new KeyValuePair<int, string>(83, "ActiveEndDate should be greater than current date");
            //}

            return new KeyValuePair<int, string>(0, "Success!!");
        }

        /// <summary>
        /// This function copies a logic of stored procedure written in sql server. Procedure name sp_get_schedule_description
        /// </summary>
        /// <returns>Message to be displayed in TextBox of UI/returns>
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

        public static Schedule ConvertFrom(ScheduleContract from)
        {
            throw new NotImplementedException();
        }
    }
}