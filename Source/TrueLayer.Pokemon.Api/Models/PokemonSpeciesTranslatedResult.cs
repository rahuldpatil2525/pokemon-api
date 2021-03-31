using System;

namespace TrueLayer.Pokemon.Api.Models
{
    public class PokemonSpeciesTranslatedResult
    {
        public PokemonSpeciesTranslatedResult(PokemonTranslatedResult pokemonTranslatedResult)
        {
            TranslatedResult = pokemonTranslatedResult;
        }

        public PokemonSpeciesTranslatedResult(Exception exception)
        {
            Exception = exception;
            HasError = true;
        }

        public PokemonSpeciesTranslatedResult(int errorCode, string message)
        {
            HasError = true;
            ErrorCode = errorCode;
            ErrorMessage = message;
        }
        public PokemonTranslatedResult TranslatedResult { get; }

        public bool HasError { get; }

        public int ErrorCode { get; }

        public string ErrorMessage { get; }

        public Exception Exception { get; }

    }
}
