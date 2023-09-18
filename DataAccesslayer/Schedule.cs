using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SchedulerApi.DataAccesslayer
{
    public class Schedule
    {
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
        public int ActiveStartDate { get; set; }

        //[Required]
        [JsonPropertyName("active_end_date")]
        public int ActiveEndDate { get; set; }

        //[Required]
        [JsonPropertyName("active_start_time")]
        public int ActiveStartTime { get; set; }

        [JsonPropertyName("active_end_time")]
        public int ActiveEndTime { get; set; }

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