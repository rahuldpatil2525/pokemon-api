using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using TrueLayer.Pokemon.Api.Contract.V1.Response;
using TrueLayer.Pokemon.Api.FunctionalTests.Extensions;
using Xunit;

namespace TrueLayer.Pokemon.Api.FunctionalTests
{
    public class PokemonShould:IClassFixture<Factories.PokemonWebApplicationFactory>
    {
        [Fact]
        public async Task Return_Expected_Pokemon_Result()
        {
            var client = new Factories.PokemonWebApplicationFactory().CreateClient();

            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/mewtwo");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var model = await response.Deserialize<PokemonResponse>();

            model.Should().NotBeNull();
            model.Name.Should().Be("mewtwo");
            model.Habitat.Should().Be("rare");
            model.Description.Should().Be("It was created by scientist after years of horrific gene splicing and DNA engineering experiments.");
            model.IsLegendary.Should().BeFalse();
        }

        [Fact]
        public async Task Return_Expected_Translated_Pokemon_Result()
        {
            var client = new Factories.PokemonWebApplicationFactory().CreateClient();

            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/translated/mewtwotranslated");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var model = await response.Deserialize<PokemonResponse>();

            model.Should().NotBeNull();
            model.Name.Should().Be("mewtwotranslated");
            model.Habitat.Should().Be("rare");
            model.Description.Should().Be("It was created by scientist after years of horrific gene splicing and DNA engineering experiments.");
            model.IsLegendary.Should().BeTrue();
        }
    }
}
