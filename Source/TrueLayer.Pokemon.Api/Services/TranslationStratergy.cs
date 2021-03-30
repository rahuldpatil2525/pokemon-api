using System;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Logging;
using TrueLayer.Pokemon.Api.Models;

namespace TrueLayer.Pokemon.Api.Services
{
    public interface ITranslationStratergy
    {
        Task<PokemonTranslatedResult> GetTranslationAsync(PokemonTranslationRequest pokemonTranslationRequest, CancellationToken ct);
    }

    public class TranslationStratergy : ITranslationStratergy
    {
        private readonly ITranslationApiClient _translationApiClient;
        private readonly IInstrumentor _instrumentor;
        public TranslationStratergy(ITranslationApiClient translationApiClient, IInstrumentor instrumentor)
        {
            _translationApiClient = translationApiClient;
            _instrumentor = instrumentor;
        }

        public async Task<PokemonTranslatedResult> GetTranslationAsync(PokemonTranslationRequest pokemonTranslationRequest, CancellationToken ct)
        {
            try
            {
                var translatedResponse = await _translationApiClient.GetTranslationAsync(pokemonTranslationRequest, ct);

                return new PokemonTranslatedResult()
                {
                    Description = translatedResponse.Contents.Text,
                    TranslatedDescription = translatedResponse.Contents.Translated,
                    TranslationProvider=translatedResponse.Contents.Translation
                };
            }
            catch(Exception ex)
            {
                _instrumentor.LogException(EventIds.TranslationApiFailedException, ex);
                return new PokemonTranslatedResult()
                {
                    Description=pokemonTranslationRequest.TranslationText,
                    TranslatedDescription=pokemonTranslationRequest.TranslationText
                };
            }
        }
    }
}
