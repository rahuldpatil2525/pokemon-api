using System;
using System.Runtime.Serialization;

namespace TrueLayer.Pokemon.Api.Clients
{
    [Serializable]
    internal class PokeApiResponseException : Exception
    {
        public PokeApiResponseException()
        {
        }

        public PokeApiResponseException(string message) : base(message)
        {
        }

        public PokeApiResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PokeApiResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
