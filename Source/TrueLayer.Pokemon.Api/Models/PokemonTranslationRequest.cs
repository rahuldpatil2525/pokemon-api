namespace TrueLayer.Pokemon.Api.Models
{
    public class PokemonTranslationRequest
    {
        public string TranslationText { get; set; }
        public string Name { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        public string TranslationProvider => GetTranslationProvider();

        private string GetTranslationProvider()
        {
            if (Habitat.ToLower().Equals("cave") || IsLegendary)
                return "yoda";
            return "shakespeare";
        }
    }
}
