using SchedulerApi.Model;

namespace SchedulerApi.Model
{
    /// <summary>
    /// Represents one event 
    /// </summary>
    public class ScheduleEvent : BaseModel
    {
        /// <summary>
        /// Event start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Event end date
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Unique event id(system generated)
        /// </summary>
        public int ScheduleEventId { get; set; }
    }
}
