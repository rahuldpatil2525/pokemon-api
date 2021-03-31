using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Models;

namespace TrueLayer.Pokemon.Api.Services
{
    public interface IPokemonSpeciesService
    {
        Task<PokemonSpeciesResult> GetPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct);
        Task<PokemonSpeciesTranslatedResult> GetTranslatedPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct);
    }

    public class PokemonSpeciesService : IPokemonSpeciesService
    {
        private readonly IPokemonSpeciesRetriever _pokemonSpeciesRetriever;
        private readonly ITranslationStratergy _translationStratergy;

        public PokemonSpeciesService(IPokemonSpeciesRetriever pokemonSpeciesRetriever, ITranslationStratergy translationStratergy)
        {
            _pokemonSpeciesRetriever = pokemonSpeciesRetriever;
            _translationStratergy = translationStratergy;
        }

        public async Task<PokemonSpeciesResult> GetPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            var result = await _pokemonSpeciesRetriever.GetPokemonAsync(pokemonSpeciesRequest, ct);

            if (result == null)
                return new PokemonSpeciesResult(404, "Pokemon Not found.");

            return new PokemonSpeciesResult(result);
        }

        public async Task<PokemonSpeciesTranslatedResult> GetTranslatedPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {

            var pokemonSpices = await _pokemonSpeciesRetriever.GetPokemonAsync(pokemonSpeciesRequest, ct);

            if (pokemonSpices == null)
                return new PokemonSpeciesTranslatedResult(404, "Pokemon Not found.");

            var translationRequest = new PokemonTranslationRequest()
            {
                Habitat = pokemonSpices.Habitat,
                IsLegendary = pokemonSpices.IsLegendary,
                TranslationText = pokemonSpices.Description,
                Name=pokemonSpices.Name
            };

            var translatedResult = await _translationStratergy.GetTranslationAsync(translationRequest, ct);

            return new PokemonSpeciesTranslatedResult(translatedResult);

        }
    }
}
