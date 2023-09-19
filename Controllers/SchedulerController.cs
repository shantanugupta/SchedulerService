using Microsoft.AspNetCore.Mvc;
using SchedulerApi.DataAccesslayer;
using SchedulerApi.DataContract;
using SchedulerApi.FunctionalLayer;

namespace SchedulerApi.Controllers
{
    /// <summary>
    /// This controller exposes various methods to manage a schedule.
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
    [ApiController]
    [Route("[controller]")]
    public class SchedulerController : ControllerBase
    {
        private readonly ILogger<SchedulerController> _logger;

        public SchedulerController(ILogger<SchedulerController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns a default schedule
        /// </summary>
        /// <returns>Returns a list of blank schedules</returns>
        [HttpGet(Name = "GetSchedule")]
        public IEnumerable<ScheduleContract> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new ScheduleContract
            {
               
            })
            .ToArray();
        }

        /// <summary>
        /// Saves a schedule into data store
        /// </summary>
        /// <param name="schedule">schedule to save</param>
        /// <returns>Saved schedule object</returns>
        [HttpPost(Name = "SetSchedule")]
        public IEnumerable<ScheduleContract> Set(ScheduleContract schedule)
        {
            return Enumerable.Range(1, 5).Select(index => schedule)
            .ToArray();
        }

        /// <summary>
        /// Generates events
        /// </summary>
        /// <param name="schedule">schedule to use</param>
        /// <returns>Generated events from server</returns>
        [HttpPost]
        [Route("GenerateEvents")]
        public Response<ScheduleEvent> GenerateEvents(ScheduleContract schedule)
        {
            Response<ScheduleEvent> response = new()
            {
                Error = new KeyValuePair<int, string>(1, "Not yet implemented")
            };

            var model = schedule.ConvertTo<Schedule>();

            ScheduleManager.GenerateEvents(model);

            return response;
            //var convertedScheduleDal = ScheduleManager.ConvertFrom(schedule);

            //ScheduleManager.GenerateEvents(convertedScheduleDal);
            //return Enumerable.Range(1, 5).Select(index => schedule)
            //.ToArray();
        }
    }
}