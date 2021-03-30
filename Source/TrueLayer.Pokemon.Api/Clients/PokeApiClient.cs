using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Contract.PokeApi;
using TrueLayer.Pokemon.Api.Exceptions;
using TrueLayer.Pokemon.Api.Models;

namespace TrueLayer.Pokemon.Api.Clients
{
    public interface IPokeApiClient
    {
        Task<PokeApiResponse> GetResponseAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct);
    }

    public class PokeApiClient : IPokeApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PokeApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PokeApiResponse> GetResponseAsync(PokemonSpeciesRequest pokemonSpeciesRequest, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("PokeApi");

            var request = new HttpRequestMessage(HttpMethod.Get, pokemonSpeciesRequest.Name);

            var response = await client.SendAsync(request, ct);

            if (response.Content == null || !response.IsSuccessStatusCode)
            {
                throw new PokeApiResponseException("Failed to retrieve response from Poke API.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PokeApiResponse>(responseContent);
        }
    }
}
