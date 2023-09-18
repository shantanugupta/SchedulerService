using SchedulerApi.DataAccesslayer;
using System.Runtime.Serialization;

namespace SchedulerApi.FunctionalLayer
{

    [Serializable]
    public class ScheduleException : Exception
    {
        public ScheduleException() { }

        public ScheduleException(string message)
            : base(message) { }

        public ScheduleException(string message, Exception inner)
            : base(message, inner) { }
    }
}
