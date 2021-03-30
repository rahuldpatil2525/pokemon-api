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
        Task<PokemonSpeciesResult> GetTranslatedPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct);
    }

    public class PokemonSpeciesService : IPokemonSpeciesService
    {
        private readonly IPokeApiClient _pokeApiClient;

        public PokemonSpeciesService(IPokeApiClient pokeApiClient)
        {
            _pokeApiClient = pokeApiClient;
        }
        public async Task<PokemonSpeciesResult> GetPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            var result = await _pokeApiClient.GetResponseAsync(pokemonSpeciesRequest, ct);

            return new PokemonSpeciesResult()
            {
                Name = result.Name,
                Description = result.Description,
                Habitat = result.Habitat,
                IsLegendary = result.IsLegendary
            };
        }

        public Task<PokemonSpeciesResult> GetTranslatedPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            return Task.FromResult(new PokemonSpeciesResult()
            {
                Name = pokemonSpeciesRequest.Name,
                Description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = true
            });
        }
    }
}
