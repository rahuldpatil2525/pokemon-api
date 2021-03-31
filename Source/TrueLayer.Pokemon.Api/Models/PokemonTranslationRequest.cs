namespace TrueLayer.Pokemon.Api.Models
{
    public class PokemonTranslationRequest
    {
        public string TranslationText { get; set; }
        public string Name { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        public string TranslationProvider => GetTranslationProvider();

        //currently there is only two api are available for different translations, but if needs to extend for more API,
        //then this needs to refactor to use  either stratergy pattern or use the dependency injection to register different api at runtime and resolve 
        private string GetTranslationProvider()
        {
            if (Habitat.ToLower().Equals("cave") || IsLegendary)
                return "yoda";
            return "shakespeare";
        }
    }
}
