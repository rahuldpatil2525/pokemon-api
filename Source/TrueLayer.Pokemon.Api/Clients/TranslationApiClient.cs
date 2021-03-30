using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Builders;
using TrueLayer.Pokemon.Api.Contract.TranslationApi;
using TrueLayer.Pokemon.Api.Exceptions;
using TrueLayer.Pokemon.Api.Models;

namespace TrueLayer.Pokemon.Api.Clients
{
    public interface ITranslationApiClient
    {
        Task<TranslationApiResponse> GetTranslationAsync(PokemonTranslationRequest pokemonTranslationRequest, CancellationToken ct);
    }

    public class TranslationApiClient : ITranslationApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TranslationApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TranslationApiResponse> GetTranslationAsync(PokemonTranslationRequest pokemonTranslationRequest, CancellationToken ct)
        {
            var queryString = new TranslationRequestQueryStringBuilder()
                .WithTranslationProvider(pokemonTranslationRequest.TranslationProvider)
                .WithTranslationText(pokemonTranslationRequest.TranslationText)
                .Build();

            var client = _httpClientFactory.CreateClient("TranslationApi");

            var request = new HttpRequestMessage(HttpMethod.Get, queryString);

            var response = await client.SendAsync(request, ct);

            if (response.Content == null || !response.IsSuccessStatusCode)
            {
                throw new TranslationApiException("Failed to retrieve response from Translation API.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TranslationApiResponse>(responseContent);
        }
    }
}
