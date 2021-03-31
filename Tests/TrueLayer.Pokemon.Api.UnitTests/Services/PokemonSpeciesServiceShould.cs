using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Services;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Services
{
    public class PokemonSpeciesServiceShould
    {
        private readonly PokemonSpeciesService _pokemonSpeciesService;
        private Moq.Mock<IPokemonSpeciesRetriever> _pokemonSpeciesRetriever;
        private Mock<ITranslationStratergy> _translationStratergy;

        public PokemonSpeciesServiceShould()
        {
            _pokemonSpeciesRetriever = new();
            _translationStratergy = new();
            _pokemonSpeciesService = new PokemonSpeciesService(_pokemonSpeciesRetriever.Object, _translationStratergy.Object);
        }

        [Fact]
        public void Throw_General_Exception_When_Unhandled_Error_Occoured()
        {
            var request = new PokemonSpeciesRequest("generalexception");

            _pokemonSpeciesRetriever.Setup(x => x.GetPokemonAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            FluentActions.Invoking(async () => await _pokemonSpeciesService.GetPokemonSpeciesAsync(request, CancellationToken.None)).Should().Throw<Exception>();
        }

        [Fact]
        public async Task Return_Response_From_PokemonAPI_When_Pass_Valid_Request()
        {
            var request = new PokemonSpeciesRequest("Pokemon Name");

            _pokemonSpeciesRetriever.Setup(x => x.GetPokemonAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonSpecies()
            {
                Name=request.Name,
                Description= "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat="rare",
                IsLegendary=false
            });

            var result = await _pokemonSpeciesService.GetPokemonSpeciesAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.PokemonSpecies.Name.Should().Be(request.Name);
            result.PokemonSpecies.Habitat.Should().Be("rare");
            result.PokemonSpecies.Description.Should().Be("It was created by scientist after years of horrific gene splicing and DNA engineering experiments.");
            result.PokemonSpecies.IsLegendary.Should().BeFalse();

            _pokemonSpeciesRetriever.Verify(x => x.GetPokemonAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_Translated_Response_From_TranslationAPI_When_Pass_Valid_Request()
        {
            var request = new PokemonSpeciesRequest("Pokemon Translation Name");

            _pokemonSpeciesRetriever.Setup(x => x.GetPokemonAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonSpecies()
            {
                Name = request.Name,
                Description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = true
            });

            _translationStratergy.Setup(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonTranslatedResult()
            {
                Description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                TranslatedDescription = "This is translated Description from Translation Provider.",
                TranslationProvider = "yoda",
                Name = request.Name,
                Habitat = "rare",
                IsLegendary = true
            });

            var result = await _pokemonSpeciesService.GetTranslatedPokemonSpeciesAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.TranslatedResult.Name.Should().Be(request.Name);
            result.TranslatedResult.Habitat.Should().Be("rare");
            result.TranslatedResult.TranslatedDescription.Should().Be("This is translated Description from Translation Provider.");
            result.TranslatedResult.IsLegendary.Should().BeTrue();
            result.TranslatedResult.TranslationProvider.Should().Be("yoda");

            _translationStratergy.Verify(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>()), Times.Once);

            _pokemonSpeciesRetriever.Verify(x => x.GetPokemonAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_ErrorCode_And_Message_From_PokeApi_When_Name_Is_Not_Found()
        {
            var request = new PokemonSpeciesRequest("Pokemon Translation Name");
            PokemonSpecies pokemonSpecies = null;
            _pokemonSpeciesRetriever.Setup(x => x.GetPokemonAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(pokemonSpecies);

            var result = await _pokemonSpeciesService.GetTranslatedPokemonSpeciesAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.HasError.Should().BeTrue();
            result.ErrorCode.Should().Be(404);

            _translationStratergy.Verify(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            _pokemonSpeciesRetriever.Verify(x => x.GetPokemonAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
