using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Exceptions;
using TrueLayer.Pokemon.Api.Logging;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Services;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Services
{
    public class PokemonSpeciesRetrieverShould
    {
        private readonly Mock<IPokeApiClient> _pokeApiClient;
        private readonly Mock<IInstrumentor> _instrumentor;
        private PokemonSpeciesRetriever _pokemonSpeciesRetriever;

        public PokemonSpeciesRetrieverShould()
        {
            _pokeApiClient = new();
            _instrumentor = new();

            _pokemonSpeciesRetriever = new PokemonSpeciesRetriever(_pokeApiClient.Object, _instrumentor.Object);
        }

        [Fact]
        public async Task Returns_Null_When_No_Pokemon_Found()
        {
            var request = new PokemonSpeciesRequest("pokemon not found");
            _pokeApiClient.Setup(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new PokemonNotFoundException());

            var result = await _pokemonSpeciesRetriever.GetPokemonAsync(request, CancellationToken.None);

            result.Should().BeNull();
            _pokeApiClient.Verify(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Returns_Valid_Response_When_Pokemon_Found()
        {
            var request = new PokemonSpeciesRequest("pokemon found");
            _pokeApiClient.Setup(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Contract.PokeApi.PokeApiResponse()
            {
                Name = request.Name,
                Habitat = new()
                {
                    Name = "rare"
                },
                IsLegendary = true,
                FlavourTexts = new System.Collections.Generic.List<Contract.PokeApi.FlavourText>()
                {
                    new Contract.PokeApi.FlavourText()
                    {
                        FlavourDescriptionText="Description",
                        Language = new()
                        {
                            Name="en"
                        }
                    }
                }
            });

            var result = await _pokemonSpeciesRetriever.GetPokemonAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.Habitat.Should().Be("rare");
            result.Name.Should().Be(request.Name);
            result.Description.Should().Be("Description");
            result.IsLegendary.Should().BeTrue();

            _pokeApiClient.Verify(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
