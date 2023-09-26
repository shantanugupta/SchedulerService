using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace SchedulerApi.FunctionalLayer
{
    #region Enum for scheduler

    internal enum FreqType
    {
        [Display(Name = "One time only")]
        [Description("Task will be scheduled one time only.")]
        OneTimeOnly = 1,
        [Display(Name = "Daily")]
        [Description("Recurring daily.")]
        Daily = 4,
        [Display(Name = "Weekly")]
        [Description("Recurring weekly.")]
        Weekly = 8,
        [Display(Name = "Monthly")]
        [Description("Recurring monthly.")]
        Monthly = 16,
        [Display(Name = "Monthly relative")]
        [Description("Recurring monthly relatively.")]
        MonthlyRelativeToFreqInterval = 32,
    };

    internal enum FreqIntervalWeekly
    {
        [Display(Name = "Sunday")]
        Sunday = 1,
        [Display(Name = "Monday")]
        Monday = 2,
        [Display(Name = "Tuesday")]
        Tuesday = 4,
        [Display(Name = "Wednesday")]
        Wednesday = 8,
        [Display(Name = "Thursday")]
        Thursday = 16,
        [Display(Name = "Friday")]
        Friday = 32,
        [Display(Name = "Saturday")]
        Saturday = 64
    };

    internal enum FreqIntervalMonthlyRelative
    {
        [Display(Name = "Sunday")]
        Sunday = 1,
        [Display(Name = "Monday")]
        Monday = 2,
        [Display(Name = "Tuesday")]
        Tuesday = 3,
        [Display(Name = "Wednesday")]
        Wednesday = 4,
        [Display(Name = "Thursday")]
        Thursday = 5,
        [Display(Name = "Friday")]
        Friday = 6,
        [Display(Name = "Saturday")]
        Saturday = 7,
        [Display(Name = "Day")]
        Day = 8,
        [Display(Name = "Weekday")]
        Weekday = 9,
        [Display(Name = "Weekend")]
        WeekendDay = 10
    };

    internal enum FreqSubdayType
    {
        [Display(Name = "At the specified time")]
        AtTheSpecifiedTime = 1,
        [Display(Name = "Seconds")]
        Seconds = 2,
        [Display(Name = "Minutes")]
        Minutes = 4,
        [Display(Name = "Hours")]
        Hours = 8,
    };

    internal enum FreqRelativeInterval
    {
        [Display(Name = "First")]
        First = 1,
        [Display(Name = "Second")]
        Second = 2,
        [Display(Name = "Third")]
        Third = 4,
        [Display(Name = "Fourth")]
        Fourth = 8,
        [Display(Name = "Last")]
        Last = 16
    };


    internal enum MomentTimeValue
    {
        Hours = 2,
        Minutes = 4,
        Second = 8
    };

    internal enum MomentWeek
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    };

    internal enum MomentfreqRelativeInterval
    {
        First = 1
        , Second = 2
        , Third = 3
        , Fourth = 4
        , Last = 5
    }

    internal enum DateTo
    {
        StartOfDay,
        EndOfDay,
        StartOfMonth,
        EndOfMonth,
        StartOfYear,
        EndOfYear
    }

    #endregion
}
