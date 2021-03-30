using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrueLayer.Pokemon.Api.Contract.PokeApi
{
    public class PokeApiResponse
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("habitat")]
        public Habitat Habitat { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public List<FlavourText> FlavourTexts { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }

    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }

    public class FlavourText
    {
        [JsonPropertyName("flavor_text")]
        public string FlavourDescriptionText { get; set; }

        [JsonPropertyName("language")]
        public Language Language { get; set; }

    }

    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
