using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Contract.V1.Response;
using TrueLayer.Pokemon.Api.FunctionalTests.Extensions;
using TrueLayer.Pokemon.Api.Middleware.ExceptionHandler;
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
        public async Task Return_Internal_Server_Error_When_Unhandled_Exception_Thrown()
        {
            var factory = new Factories.PokemonWebApplicationFactory();

            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/generalexception");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var error = await response.Deserialize<ErrorDetail[]>();

            error.Should().NotBeNull();
            error.Length.Should().Be(1);
            error[0].ErrorCode.Should().Be("UnknownError");
            error[0].ErrorMessage.Should().Be("UnKnown Error Occoured");

            factory.Instrumentor.Verify(x => x.LogException(It.IsAny<int>(), It.IsAny<Exception>(), It.IsAny<IDictionary<string,object>>()), Times.Once);
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
