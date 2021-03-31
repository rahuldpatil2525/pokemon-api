using System;
using System.Runtime.Serialization;

namespace TrueLayer.Pokemon.Api.Exceptions
{
    [Serializable]
    public class PokemonNotFoundException : Exception
    {
        public PokemonNotFoundException()
        {
        }

        public PokemonNotFoundException(string message) : base(message)
        {
        }

        public PokemonNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PokemonNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}