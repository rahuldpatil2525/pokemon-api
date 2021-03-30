using System.Text.Json.Serialization;

namespace TrueLayer.Pokemon.Api.Contract.TranslationApi
{
    public class TranslationApiResponse
    {
        [JsonPropertyName("success")]
        public Success Success { get; set; }
        
        [JsonPropertyName("contents")]
        public Contents Contents { get; set; }
    }

    public class Success
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Contents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; }
       
        [JsonPropertyName("text")]
        public string Text { get; set; }
        
        [JsonPropertyName("translation")]
        public string Translation { get; set; }

    }
}
