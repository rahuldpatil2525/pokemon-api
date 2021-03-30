using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public Task<PokemonSpeciesResult> GetPokemonSpeciesAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            if (pokemonSpeciesRequest.Name == "generalexception")
                throw new System.Exception("General UnHandled Exception");

            return Task.FromResult(new PokemonSpeciesResult()
            {
                Name = pokemonSpeciesRequest.Name,
                Description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = false
            });
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
