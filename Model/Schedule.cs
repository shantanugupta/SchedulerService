using SchedulerApi.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SchedulerApi.Model
{
    public class Schedule:BaseModel
    {
        /// <summary>
        /// Schedule ID <br></br>
        /// Unique identifier of the schedule. This value is used to identify a schedule for distributed schedules.
        /// </summary>
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Schedule verion <br></br>
        /// Current version number of the schedule. For example, if a schedule has been modified 10 times, the version_number is 10.
        /// </summary>
        [JsonPropertyName("version_number")]
        public int VersionNumber { get; set; }

        //[Required]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        //[Required]
        [JsonPropertyName("freq_type")]
        public int FreqType { get; set; }

        //[Required]
        [JsonPropertyName("freq_interval")]
        public int FreqInterval { get; set; }

        //[Required]
        [JsonPropertyName("freq_relative_interval")]
        public int FreqRelativeInterval { get; set; }

        //[Required]
        [JsonPropertyName("freq_recurrence_factor")]
        public int FreqRecurrenceFactor { get; set; }

        //[Required]
        [JsonPropertyName("active_start_date")]
        public DateOnly ActiveStartDate { get; set; }

        //[Required]
        [JsonPropertyName("active_end_date")]
        public DateOnly ActiveEndDate { get; set; }

        //[Required]
        [JsonPropertyName("active_start_time")]
        public TimeOnly ActiveStartTime { get; set; }

        [JsonPropertyName("active_end_time")]
        public TimeOnly ActiveEndTime { get; set; }

        [JsonPropertyName("freq_subday_type")]
        public int FreqSubdayType { get; set; }

        [JsonPropertyName("freq_subday_interval")]
        public int FreqSubdayInterval { get; set; }

        [JsonPropertyName("duration_subday_type")]
        public int DurationSubdayType { get; set; }

        [JsonPropertyName("duration_interval")]
        public int DurationInterval { get; set; }

        [JsonPropertyName("occurance_choice_state")]
        public bool OccuranceChoiceState { get; set; }
    }
}