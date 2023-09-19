using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace SchedulerApi.DataContract
{
    /// <summary>
    /// Schedule contract
    /// See <a href="https://learn.microsoft.com/en-us/sql/relational-databases/system-tables/dbo-sysschedules-transact-sql?view=sql-server-ver16">this link</a> for more information.
    /// </summary>
    public class ScheduleContract
    {
        /// <summary>
        /// Schedule ID
        /// Unique identifier of the schedule. This value is used to identify a schedule for distributed schedules.
        /// </summary>
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Schedule verion
        /// Current version number of the schedule. For example, if a schedule has been modified 10 times, the version_number is 10.
        /// </summary>
        [JsonPropertyName("version_number")]
        public int VersionNumber { get; set; }

        /// <summary>
        /// name property
        /// Allowed up to 40 uppercase and lowercase characters.
        /// </summary>
        [Required]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// description property
        /// Allowed up to 40 uppercase and lowercase characters.
        /// </summary>
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{0,1024}$", ErrorMessage = "Invalid description. Allowed up to 1024 characters")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// freq_type property
        /// Valid values must be either of {1|4|8|16|32|64|128}
        /// How frequently a schedule event runs for this schedule.
        /// 1 = One time only
        /// 4 = Daily
        /// 8 = Weekly
        /// 16 = Monthly
        /// 32 = Monthly, relative to freq_interval
        /// 64 = Yearly
        /// 128 = Year long
        /// </summary>
        [Required]
        [RegularExpression(pattern: @"^(1|4|8|16|32|64|128)$", ErrorMessage = "Invalid freq_type. Valid values must be either of {1|4|8|16|32|64|128}")]
        [JsonPropertyName("freq_type")]
        public int FreqType { get; set; }

        /// <summary>
        /// freq_interval property.
        /// Days that the event is executed. Depends on the value of freq_type. The default value is 0, which indicates that freq_interval is unused.
        /// Valid values must be within 0-127
        /// </summary>
        [Required]
        [Range(0, 127, ErrorMessage = "Invalid freq_interval. Valid values must be within 0-127")]
        [JsonPropertyName("freq_interval")]
        public int FreqInterval { get; set; }

        /// <summary>
        /// Units for the freq_subday_interval. The following are the possible values and their descriptions.
        /// 1 : At the specified time
        /// 2 : hours
        /// 4 : minutes
        /// </summary>
        [Required]
        [RegularExpression(pattern: @"^(1|2|4)$", ErrorMessage = "Invalid freq_subday_type. Valid values must be either of {1|2|4}")]
        [JsonPropertyName("freq_subday_type")]
        public int FreqSubdayType { get; set; }

        /// <summary>
        /// Number of freq_subday_type periods to occur between each execution of the event.
        /// Valid values are between 0-59
        /// </summary>
        [Required]
        [Range(0, 59, ErrorMessage = "Invalid freq_subday_interval. Valid values are 0-59")]
        [JsonPropertyName("freq_subday_interval")]
        public int FreqSubdayInterval { get; set; }

        /// <summary>
        /// When freq_interval occurs in each month, if freq_type is 32 (monthly relative). Can be one of the following values:
        /// 0 = freq_relative_interval is unused
        /// 1 = First
        /// 2 = Second
        /// 4 = Third
        /// 8 = Fourth
        /// 16 = Last
        /// </summary>
        [Required]
        [RegularExpression(pattern: @"^(0|1|2|4|8|16)$", ErrorMessage = "freq_relative_interval freq_type. Valid values must be either of {0|1|2|4|8|16}")]
        [JsonPropertyName("freq_relative_interval")]
        public int FreqRelativeInterval { get; set; }

        /// <summary>
        /// Number of weeks or months between the scheduled execution of an event. freq_recurrence_factor is used only if freq_type is 8, 16, or 32. If this field contains 0, freq_recurrence_factor is unused.
        /// Valid values between 0-60
        /// </summary>
        [Required]
        [JsonPropertyName("freq_recurrence_factor")]
        [Range(0, 60, ErrorMessage = "freq_recurrence_factor freq_type. Valid values must be between 0-60")]
        public int FreqRecurrenceFactor { get; set; }

        [Required]
        [JsonPropertyName("duration_subday_type")]
        public int DurationSubdayType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [JsonPropertyName("duration_interval")]
        public int DurationInterval { get; set; }

        /// <summary>
        /// Date on which execution of an event can begin. The date is formatted as YYYYMMDD. NULL indicates today's date.
        /// </summary>
        [Required]
        [JsonPropertyName("active_start_date")]
        public string ActiveStartDate { get; set; }

        /// <summary>
        /// Date on which execution of an event can stop. The date is formatted YYYYMMDD.
        /// </summary>
        [Required]
        [JsonPropertyName("active_end_date")]
        public string ActiveEndDate { get; set; }

        /// <summary>
        /// Time on any day between active_start_date and active_end_date that event begins executing. Time is formatted HHMM, using a 24-hour clock.
        /// </summary>
        [Required]
        [JsonPropertyName("active_start_time")]
        public string ActiveStartTime { get; set; }

        /// <summary>
        /// Time on any day between active_start_date and active_end_date that event stops. Time is formatted HHMM, using a 24-hour clock.
        /// </summary>
        [Required]
        [JsonPropertyName("active_end_time")]
        public string ActiveEndTime { get; set; }

        /// <summary>
        /// Occurance selection. Occurs at or Occurs every
        /// </summary>
        [Required]
        [JsonPropertyName("occurance_choice_state")]
        public bool OccuranceChoiceState { get; set; }
    }
}
