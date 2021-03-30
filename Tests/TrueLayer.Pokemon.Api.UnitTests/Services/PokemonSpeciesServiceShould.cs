using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Services;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Services
{
    public class PokemonSpeciesServiceShould
    {
        private readonly PokemonSpeciesService _pokemonSpeciesService;
        private Moq.Mock<IPokeApiClient> _pokeApiClient;
        private Mock<ITranslationStratergy> _translationStratergy;

        public PokemonSpeciesServiceShould()
        {
            _pokeApiClient = new();
            _translationStratergy = new();
            _pokemonSpeciesService = new PokemonSpeciesService(_pokeApiClient.Object, _translationStratergy.Object);
        }

        [Fact]
        public void Throw_General_Exception_When_Unhandled_Error_Occoured()
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
                FlavourTexts=new System.Collections.Generic.List<Contract.PokeApi.FlavourText>() 
                {
                    new Contract.PokeApi.FlavourText()
                    {
                        FlavourDescriptionText="It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                        Language = new()
                        {
                            Name="en"
                        }
                    }
                },
                Habitat = new() 
                {
                    Name="rare"
                },
                IsLegendary = false
            });

            var result = await _pokemonSpeciesService.GetPokemonSpeciesAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Habitat.Should().Be("rare");
            result.Description.Should().Be("It was created by scientist after years of horrific gene splicing and DNA engineering experiments.");
            result.IsLegendary.Should().BeFalse();

            _pokeApiClient.Verify(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_Translated_Response_From_TranslationAPI_When_Pass_Valid_Request()
        {
            var request = new PokemonSpeciesRequest("Pokemon Translation Name");

            _pokeApiClient.Setup(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Contract.PokeApi.PokeApiResponse()
            {
                Name = request.Name,
                FlavourTexts = new System.Collections.Generic.List<Contract.PokeApi.FlavourText>()
                {
                    new Contract.PokeApi.FlavourText()
                    {
                        FlavourDescriptionText="It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                        Language = new()
                        {
                            Name="en"
                        }
                    }
                },
                Habitat = new()
                {
                    Name = "rare"
                },
                IsLegendary = true
            });

            _translationStratergy.Setup(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonTranslatedResult()
            {
                Description= "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                TranslatedDescription="This is translated Description from Translation Provider.",
                TranslationProvider= "yoda"            
            });

            var result = await _pokemonSpeciesService.GetTranslatedPokemonSpeciesAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Habitat.Should().Be("rare");
            result.Description.Should().Be("This is translated Description from Translation Provider.");
            result.IsLegendary.Should().BeTrue();
            result.TranslationProvider.Should().Be("yoda");

            _translationStratergy.Verify(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>()), Times.Once);

            _pokeApiClient.Verify(x => x.GetResponseAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
