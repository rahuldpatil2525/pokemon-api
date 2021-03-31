using System;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Exceptions;
using TrueLayer.Pokemon.Api.Logging;
using TrueLayer.Pokemon.Api.Models;

namespace TrueLayer.Pokemon.Api.Services
{
    public interface IPokemonSpeciesRetriever
    {
        Task<PokemonSpecies> GetPokemonAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct);
    }

    public class PokemonSpeciesRetriever : IPokemonSpeciesRetriever
    {
        private readonly IPokeApiClient _pokeApiClient;
        private readonly IInstrumentor _instrumentor;

        public PokemonSpeciesRetriever(IPokeApiClient pokeApiClient, IInstrumentor instrumentor)
        {
            _instrumentor = instrumentor;
            _pokeApiClient = pokeApiClient;
        }
        public async Task<PokemonSpecies> GetPokemonAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            try
            {
                var result = await _pokeApiClient.GetResponseAsync(pokemonSpeciesRequest, ct);

                return result?.ToPokemonSpecies();
            }
            catch (PokemonNotFoundException ex)
            {
                _instrumentor.LogException(EventIds.PokemonNotFoundException, ex);
                return null;
            }
        }
    }
}
