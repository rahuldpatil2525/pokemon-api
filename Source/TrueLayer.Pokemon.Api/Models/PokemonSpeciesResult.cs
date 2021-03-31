namespace TrueLayer.Pokemon.Api.Models
{
    public class PokemonSpeciesResult
    {
        public PokemonSpeciesResult(PokemonSpecies pokemonSpecies)
        {
            PokemonSpecies = pokemonSpecies;
        }

        public PokemonSpeciesResult(int errorCode, string errorMessage)
        {
            HasError = true;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public PokemonSpecies PokemonSpecies { get; }
        public bool HasError { get; }

        public int ErrorCode { get; }

        public string ErrorMessage { get; }
    }

    public class PokemonSpecies
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Habitat { get; set; }

        public bool IsLegendary { get; set; }
    }
}
