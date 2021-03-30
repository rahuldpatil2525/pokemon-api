using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Contract.PokeApi;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Services;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Services
{
    public class PokemonSpeciesServiceShould
    {
        private readonly PokemonSpeciesService _pokemonSpeciesService;
        private Moq.Mock<IPokeApiClient> _pokeApiClient;
        public PokemonSpeciesServiceShould()
        {
            _pokeApiClient = new();
            _pokemonSpeciesService = new PokemonSpeciesService(_pokeApiClient.Object);
        }

        [Fact]
        public async Task Throw_General_Exception_When_Unhandled_Error_Occoured()
        {
            var request = new PokemonSpeciesRequest("generalexception");

            _pokeApiClient.Setup(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            FluentActions.Invoking(async () => await _pokemonSpeciesService.GetPokemonSpeciesAsync(request, CancellationToken.None)).Should().Throw<Exception>();
        }

        [Fact]
        public async Task Return_Response_From_PokemonAPI_When_Pass_Valid_Request()
        {
            var request = new PokemonSpeciesRequest("Pokemon Name");

            _pokeApiClient.Setup(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Contract.PokeApi.PokeApiResponse()
            {
                Name = request.Name,
                Description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = false
            });
            
            var result = await _pokemonSpeciesService.GetPokemonSpeciesAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Habitat.Should().Be("rare");
            result.Description.Should().Be("It was created by scientist after years of horrific gene splicing and DNA engineering experiments.");
            result.IsLegendary.Should().BeFalse();
        }

        [Fact]
        public async Task Return_Translated_Response_From_PokemonAPI_When_Pass_Valid_Request()
        {
            var request = new PokemonSpeciesRequest("Pokemon Translated Name");
            var result = await _pokemonSpeciesService.GetTranslatedPokemonSpeciesAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Habitat.Should().Be("rare");
            result.Description.Should().Be("It was created by scientist after years of horrific gene splicing and DNA engineering experiments.");
            result.IsLegendary.Should().BeTrue();
        }
    }
}
