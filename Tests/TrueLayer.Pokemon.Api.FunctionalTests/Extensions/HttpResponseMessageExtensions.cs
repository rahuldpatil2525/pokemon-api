using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrueLayer.Pokemon.Api.FunctionalTests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static async Task<T> Deserialize<T>(this HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(data, options);
        }
    }
}
