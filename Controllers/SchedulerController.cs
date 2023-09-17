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

        [HttpGet(Name = "GetSchedule")]
        public IEnumerable<Schedule> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Schedule
            {
               
            })
            .ToArray();
        }

        [HttpPost(Name = "SetSchedule")]
        public IEnumerable<Schedule> Set(Schedule schedule)
        {
            IEnumerable<Schedule> schedules = new Schedule[] { schedule };
            return schedules;
        }
    }
}