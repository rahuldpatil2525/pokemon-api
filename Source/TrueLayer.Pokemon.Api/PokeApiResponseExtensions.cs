using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Contract.PokeApi;
using TrueLayer.Pokemon.Api.Models;

namespace TrueLayer.Pokemon.Api
{
    public static class PokeApiResponseExtensions
    {
        public static PokemonSpeciesResult ToPokemonSpeciesResult(this PokeApiResponse pokeApiResponse)
        {
            return new PokemonSpeciesResult()
            {
                Name = pokeApiResponse.Name,
                Description = GetDescription(pokeApiResponse),
                Habitat = pokeApiResponse.Habitat?.Name,
                IsLegendary = pokeApiResponse.IsLegendary
            };
        }

        private static string GetDescription(PokeApiResponse pokeApiResponse)
        {
            return pokeApiResponse.FlavourTexts?.FirstOrDefault(x => x.Language.Name == "en")?.FlavourDescriptionText;
        }
    }
}
