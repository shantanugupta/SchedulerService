using Microsoft.AspNetCore.Mvc;
using SchedulerApi.ApiContracts;


namespace SchedulerApi.Controllers
{
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
        public IEnumerable<Schedule> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Schedule
            {
               
            })
            .ToArray();
        }

        /// <summary>
        /// Saves a schedule into database
        /// </summary>
        /// <param name="schedule">schedule to save</param>
        /// <returns>Saved schedule object</returns>
        [HttpPost(Name = "SetSchedule")]
        public IEnumerable<Schedule> Set(Schedule schedule)
        {
            return Enumerable.Range(1, 5).Select(index => schedule)
            .ToArray();
        }
    }
}