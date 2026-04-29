using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Convertor;
using SchedulerApi.Model;
using SchedulerApi.ApiContract;
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
        /// Returns service health status
        /// </summary>
        /// <returns>200 OK with "Healthy" when service is running</returns>
        [HttpGet("health", Name = "HealthCheck")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy" });
        }

        /// <summary>
        /// Returns all saved schedules
        /// </summary>
        /// <returns>Stored schedules wrapped in a Response object</returns>
        [HttpGet(Name = "GetSchedule")]
        public Response<IEnumerable<ScheduleContract>> Get()
        {
            var schedules = ScheduleManager.GetAll().Select(s => s.ToContract());
            return new Response<IEnumerable<ScheduleContract>>(schedules);
        }

        /// <summary>
        /// Saves a schedule into the data store
        /// </summary>
        /// <param name="schedule">Schedule to save</param>
        /// <returns>Saved schedule wrapped in a Response object, or validation errors</returns>
        [HttpPost(Name = "SetSchedule")]
        public Response<ScheduleContract> Set(ScheduleContract schedule)
        {
            Response<ScheduleContract> response = new();

            Schedule model;
            try
            {
                model = schedule.ConvertTo<Schedule>();
            }
            catch (FormatException ex)
            {
                response.Error.Add(1, ex.Message);
                return response;
            }

            var saveResult = ScheduleManager.Save(model);
            if (saveResult.Error.Count > 0)
            {
                response.Error = saveResult.Error;
                return response;
            }

            response.Entity = saveResult.Entity.ToContract();
            return response;
        }

        /// <summary>
        /// Generates events
        /// </summary>
        /// <param name="schedule">Schedule to use</param>
        /// <returns>Generated events from server</returns>
        [HttpPost]
        [Route("GenerateEvents")]
        public Response<IEnumerable<ScheduleEvent>> GenerateEvents(ScheduleContract schedule)
        {
            Response<IEnumerable<ScheduleEvent>> response = new();

            Schedule model;
            try
            {
                model = schedule.ConvertTo<Schedule>();
            }
            catch (FormatException ex)
            {
                response.Error.Add(new KeyValuePair<int, string>(1, ex.Message));
                return response;
            }
            

            response.Entity = ScheduleManager.GenerateEvents(model);

            return response;
        }

        /// <summary>
        /// Generates description
        /// </summary>
        /// <param name="schedule">Schedule to use</param>
        /// <returns>Generates schedule description from server</returns>
        [HttpPost]
        [Route("GenerateDescription")]
        public Response<string> GenerateDescription(ScheduleContract schedule)
        {
            Response<string> response = new();

            Schedule model;
            try
            {
                model = schedule.ConvertTo<Schedule>();
            }
            catch (FormatException ex)
            {
                response.Error.Add(new KeyValuePair<int, string>(1, ex.Message));
                return response;
            }


            response = ScheduleManager.GenerateDescription(model);

            return response;
        }
    }
}
