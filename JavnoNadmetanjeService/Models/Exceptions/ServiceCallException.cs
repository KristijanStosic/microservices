using System;
using System.Runtime.Serialization;

namespace JavnoNadmetanjeService.Models.Exceptions
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
