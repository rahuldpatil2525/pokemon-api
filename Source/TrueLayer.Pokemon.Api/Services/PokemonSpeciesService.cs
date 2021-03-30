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
        private readonly IPokeApiClient _pokeApiClient;
        private readonly ITranslationStratergy _translationStratergy;

        public PokemonSpeciesService(IPokeApiClient pokeApiClient, ITranslationStratergy translationStratergy)
        {
            _pokeApiClient = pokeApiClient;
            _translationStratergy = translationStratergy;
        }

        public async Task<PokemonSpeciesResult> GetPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            var result = await _pokeApiClient.GetResponseAsync(pokemonSpeciesRequest, ct);

            return result.ToPokemonSpeciesResult();
        }

        public async Task<PokemonSpeciesTranslatedResult> GetTranslatedPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            var pokemonSpices = await GetPokemonSpeciesAsync(pokemonSpeciesRequest, ct);

            if (pokemonSpices == null)
                return null;

            var translationRequest = new PokemonTranslationRequest()
            {
                Habitat = pokemonSpices.Habitat,
                IsLegendary = pokemonSpices.IsLegendary,
                TranslationText = pokemonSpices.Description
            };

            var translatedResult = await _translationStratergy.GetTranslationAsync(translationRequest, ct);

            return new PokemonSpeciesTranslatedResult()
            {
                Description = translatedResult.TranslatedDescription,
                Habitat = pokemonSpices.Habitat,
                IsLegendary = pokemonSpices.IsLegendary,
                Name = pokemonSpices.Name,
                TranslationProvider = translatedResult.TranslationProvider
            };
        }
    }
}
