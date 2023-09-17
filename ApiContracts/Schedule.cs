using System.Text.Json.Serialization;

namespace SchedulerApi.ApiContracts
{
    public class Schedule
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("freq_type")]
        public int FreqType { get; set; }

        [JsonPropertyName("freq_interval")]
        public int FreqInterval { get; set; }

        [JsonPropertyName("freq_relative_interval")]
        public int FreqRelativeInterval { get; set; }

        [JsonPropertyName("freq_recurrence_factor")]
        public int FreqRecurrenceFactor { get; set; }

        [JsonPropertyName("active_start_date")]
        public string? ActiveStartDate { get; set; }

        [JsonPropertyName("active_end_date")]
        public string? ActiveEndDate { get; set; }

        [JsonPropertyName("active_start_time")]
        public string? ActiveStartTime { get; set; }

        [JsonPropertyName("active_end_time")]
        public string? ActiveEndTime { get; set; }

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