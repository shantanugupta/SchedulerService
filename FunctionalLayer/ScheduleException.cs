using SchedulerApi.Model;
using System.Runtime.Serialization;

namespace SchedulerApi.FunctionalLayer
{

    /// <summary>
    /// Wrapped exception for all schedule related events
    /// </summary>
    [Serializable]
    public class ScheduleException : Exception
    {
        internal ScheduleException() { }

        internal ScheduleException(string message)
            : base(message) { }

        internal ScheduleException(string message, Exception inner)
            : base(message, inner) { }
    }
}
