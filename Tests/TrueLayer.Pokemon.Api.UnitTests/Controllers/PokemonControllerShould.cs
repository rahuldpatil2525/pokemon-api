using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Controllers;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Services;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Controllers
{
    public class PokemonControllerShould
    {
        private readonly Mock<IPokemonSpeciesService> _pokemonSpeciesService;
        PokemonController pokemonController;

        public PokemonControllerShould()
        {
            _pokemonSpeciesService = new();
            pokemonController = new(_pokemonSpeciesService.Object);
        }

        [Fact]
        public async Task Return_PokemonSpecies_Ok_Response()
        {
            var pokemonSpecies = new PokemonSpecies()
            {
                Description = "Description",
                Habitat = "rare",
                IsLegendary = false,
                Name = "Name of the Pokemon"
            };
            
            _pokemonSpeciesService.Setup(x => x.GetPokemonSpeciesAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonSpeciesResult(pokemonSpecies));

            var response=await pokemonController.Get(pokemonSpecies.Name, CancellationToken.None);

            response.Should().NotBeNull();

            _pokemonSpeciesService.Verify(x => x.GetPokemonSpeciesAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_PokemonSpecies_Translation_Ok_Response()
        {
            var pokemonSpecies = new PokemonTranslatedResult()
            {
                Description = "Description",
                Habitat = "rare",
                IsLegendary = false,
                Name = "Name of the Pokemon",
                TranslatedDescription="This is translated Text",
                TranslationProvider="Translation Provider"
            };

            _pokemonSpeciesService.Setup(x => x.GetTranslatedPokemonSpeciesAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonSpeciesTranslatedResult(pokemonSpecies));

            var response = await pokemonController.GetTranslation(pokemonSpecies.Name, CancellationToken.None);

            response.Should().NotBeNull();

            _pokemonSpeciesService.Verify(x => x.GetTranslatedPokemonSpeciesAsync(It.IsAny<PokemonSpeciesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
