﻿using SchedulerApi.Model;
using System.Runtime.Serialization;

namespace SchedulerApi.Model
{
    /// <summary>
    /// Unified response object
    /// </summary>
    /// <typeparam name="T">Entity to be returned as a response</typeparam>
    [DataContract]
    public class Response<T> : BaseModel
    {
        /// <summary>
        /// Error entity if there is any error
        /// </summary>
        [DataMember]
        public IDictionary<int, string> Error { get; set; }

        /// <summary>
        /// Response entity if processing is successful.
        /// </summary>
        [DataMember]
        public T Entity { get; set; }

        /// <summary>
        /// Default response. Set error code as 0 and error message as success by default.
        /// </summary>
        public Response()
        {
            Error = new Dictionary<int, string>();
        }

        /// <summary>
        /// Response entity
        /// </summary>
        /// <param name="entity">Entity to be returned</param>
        public Response(T entity) : this()
        {
            Entity = entity;
        }
    }
}
