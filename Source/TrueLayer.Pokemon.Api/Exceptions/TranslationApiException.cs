using System;
using System.Runtime.Serialization;

namespace TrueLayer.Pokemon.Api.Exceptions
{
    [Serializable]
    internal class TranslationApiException : Exception
    {
        public TranslationApiException()
        {
        }

        public TranslationApiException(string message) : base(message)
        {
        }

        public TranslationApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TranslationApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}