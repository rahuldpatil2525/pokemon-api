using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Contract.PokeApi;
using TrueLayer.Pokemon.Api.Exceptions;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Provider;

namespace TrueLayer.Pokemon.Api.Clients
{
    public interface IPokeApiClient
    {
        Task<PokeApiResponse> GetResponseAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct);
    }

    public class PokeApiClient : IPokeApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRetryPolicyProvider _retryPolicyProvider;
        public PokeApiClient(IHttpClientFactory httpClientFactory, IRetryPolicyProvider retryPolicyProvider)
        {
            _httpClientFactory = httpClientFactory;
            _retryPolicyProvider = retryPolicyProvider;
        }

        public async Task<PokeApiResponse> GetResponseAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("PokeApi");

            var request = new HttpRequestMessage(HttpMethod.Get, pokemonSpeciesRequest.Name);

            var response = await _retryPolicyProvider.AsyncPolicy.ExecuteAsync(async () => await client.SendAsync(request, ct));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new PokemonNotFoundException($"Pokemon Not Found: {pokemonSpeciesRequest.Name}");

            if (response.Content == null || !response.IsSuccessStatusCode)
            {
                throw new PokeApiResponseException("Failed to retrieve response from Poke API.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PokeApiResponse>(responseContent);
        }
    }
}
