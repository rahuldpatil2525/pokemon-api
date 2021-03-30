using System.Text.Json;

namespace TrueLayer.Pokemon.Api.FunctionalTests.Extensions
{
    public static class JsonSerializationExtension
    {
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        public static string ToJson<T>(this T value)
        {
            return JsonSerializer.Serialize<T>(value, options);
        }
    }
}
