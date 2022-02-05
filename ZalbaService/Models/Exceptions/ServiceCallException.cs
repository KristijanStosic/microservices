using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ZalbaService.Models.Exceptions
{
    [Serializable]
    public class ServiceCallException : Exception
    {
        public ServiceCallException(string message) : base(message)
        {
        }

        protected ServiceCallException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }
    }
}
