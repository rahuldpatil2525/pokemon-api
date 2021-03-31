using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Contract.V1.Response;
using TrueLayer.Pokemon.Api.FunctionalTests.Builders;
using TrueLayer.Pokemon.Api.FunctionalTests.Extensions;
using TrueLayer.Pokemon.Api.Middleware.ExceptionHandler;
using Xunit;

namespace TrueLayer.Pokemon.Api.FunctionalTests
{
    public class PokemonShould : IClassFixture<Factories.PokemonWebApplicationFactory>
    {
        [Fact]
        public async Task Return_Expected_Pokemon_Result()
        {
            var factory = new Factories.PokemonWebApplicationFactory();

            var fakeMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeHttpClient = new HttpClient(fakeMessageHandler)
            {
                BaseAddress = new Uri("https://test-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("PokeApi")).Returns(fakeHttpClient);

            fakeMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(new PokeApiResponseBuilder().Create().ToJson())
            };

            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/mewtwo");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var model = await response.Deserialize<PokemonResponse>();

            model.Should().NotBeNull();
            model.Name.Should().Be("mewtwo");
            model.Habitat.Should().Be("rare");
            model.Description.Should().Be("It was created by scientist after years of horrific gene splicing and DNA engineering experiments.");
            model.IsLegendary.Should().BeFalse();

            factory.HttpClientFactory.Verify(x => x.CreateClient("PokeApi"), Times.Once);
        }

        [Fact]
        public async Task Return_Internal_Server_Error_When_Unhandled_Exception_Thrown()
        {
            var factory = new Factories.PokemonWebApplicationFactory();

            var fakeMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeHttpClient = new HttpClient(fakeMessageHandler)
            {
                BaseAddress = new Uri("https://test-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("PokeApi")).Returns(fakeHttpClient);

            fakeMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/generalexception");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var error = await response.Deserialize<ErrorDetail[]>();

            error.Should().NotBeNull();
            error.Length.Should().Be(1);
            error[0].ErrorCode.Should().Be("UnknownError");
            error[0].ErrorMessage.Should().Be("UnKnown Error Occoured");

            factory.Instrumentor.Verify(x => x.LogException(It.IsAny<int>(), It.IsAny<Exception>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            factory.HttpClientFactory.Verify(x => x.CreateClient("PokeApi"), Times.Once);
        }

        [Fact]
        public async Task Return_Expected_Translated_From_Shakespeare_Api_Pokemon_Result()
        {
            var factory = new Factories.PokemonWebApplicationFactory();

            var fakeMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeHttpClient = new HttpClient(fakeMessageHandler)
            {
                BaseAddress = new Uri("https://test-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("PokeApi")).Returns(fakeHttpClient);

            fakeMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(new PokeApiResponseBuilder()
                .WithName("mewtwotranslated")
                .WithDescription("This is not Translated.")
                .Create().ToJson())
            };

            var fakeTranslationMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeTranslationHttpClient = new HttpClient(fakeTranslationMessageHandler)
            {
                BaseAddress = new Uri("https://test-translation-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("TranslationApi")).Returns(fakeTranslationHttpClient);

            fakeTranslationMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(new TranslationApiResponseBuilder()
                .WithTranslation("shakespeare")
                .Create()
                .ToJson())
            };

            var client = factory.CreateClient();


            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/translated/mewtwotranslated");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var model = await response.Deserialize<TranslatedPokemonResponse>();

            model.Should().NotBeNull();
            model.Name.Should().Be("mewtwotranslated");
            model.Habitat.Should().Be("rare");
            model.Description.Should().Be("This is Translated.");
            model.IsLegendary.Should().BeFalse();
            model.TranslationProvider.Should().Be("shakespeare");

            factory.HttpClientFactory.Verify(x => x.CreateClient("PokeApi"), Times.Once);
            factory.HttpClientFactory.Verify(x => x.CreateClient("TranslationApi"), Times.Once);
        }

        [Fact]
        public async Task Return_Expected_Translated_From_Yoda_Api_Pokemon_Result()
        {
            var factory = new Factories.PokemonWebApplicationFactory();

            var fakeMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeHttpClient = new HttpClient(fakeMessageHandler)
            {
                BaseAddress = new Uri("https://test-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("PokeApi")).Returns(fakeHttpClient);

            fakeMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(new PokeApiResponseBuilder()
                .WithName("mewtwotranslated")
                .WithDescription("This is not Translated.")
                .WithIsLegendary(true)
                .Create().ToJson())
            };

            var fakeTranslationMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeTranslationHttpClient = new HttpClient(fakeTranslationMessageHandler)
            {
                BaseAddress = new Uri("https://test-translation-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("TranslationApi")).Returns(fakeTranslationHttpClient);

            fakeTranslationMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(new TranslationApiResponseBuilder().Create().ToJson())
            };

            var client = factory.CreateClient();


            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/translated/mewtwotranslated");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var model = await response.Deserialize<TranslatedPokemonResponse>();

            model.Should().NotBeNull();
            model.Name.Should().Be("mewtwotranslated");
            model.Habitat.Should().Be("rare");
            model.Description.Should().Be("This is Translated.");
            model.IsLegendary.Should().BeTrue();
            model.TranslationProvider.Should().Be("yoda");

            factory.HttpClientFactory.Verify(x => x.CreateClient("PokeApi"), Times.Once);
            factory.HttpClientFactory.Verify(x => x.CreateClient("TranslationApi"), Times.Once);
        }

        [Fact]
        public async Task Return_Default_Description_When_Translated_Api_Failed_To_Translate()
        {
            var factory = new Factories.PokemonWebApplicationFactory();

            var fakeMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeHttpClient = new HttpClient(fakeMessageHandler)
            {
                BaseAddress = new Uri("https://test-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("PokeApi")).Returns(fakeHttpClient);

            fakeMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(new PokeApiResponseBuilder()
                .WithName("mewtwotranslated")
                .WithDescription("This is not Translated.")
                .WithIsLegendary(true)
                .Create().ToJson())
            };

            var fakeTranslationMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeTranslationHttpClient = new HttpClient(fakeTranslationMessageHandler)
            {
                BaseAddress = new Uri("https://test-translation-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("TranslationApi")).Returns(fakeTranslationHttpClient);

            fakeTranslationMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/translated/mewtwotranslated");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var model = await response.Deserialize<TranslatedPokemonResponse>();

            model.Should().NotBeNull();
            model.Name.Should().Be("mewtwotranslated");
            model.Habitat.Should().Be("rare");
            model.Description.Should().Be("This is not Translated.");
            model.IsLegendary.Should().BeTrue();
            model.TranslationProvider.Should().BeNullOrEmpty();

            factory.HttpClientFactory.Verify(x => x.CreateClient("PokeApi"), Times.Once);
            factory.HttpClientFactory.Verify(x => x.CreateClient("TranslationApi"), Times.Once);
        }

        [Fact]
        public async Task Return_NotFound_When_Pokemon_Is_Not_Found()
        {
            var factory = new Factories.PokemonWebApplicationFactory();

            var fakeMessageHandler = new Fakes.FakeHttpMessageHandler();
            var fakeHttpClient = new HttpClient(fakeMessageHandler)
            {
                BaseAddress = new Uri("https://test-api-pokemon.com")
            };

            factory.HttpClientFactory.Setup(x => x.CreateClient("PokeApi")).Returns(fakeHttpClient);

            fakeMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("Not Found")
            };

           var client = factory.CreateClient();

            var response = await client.GetAsync("/api/Pokemon/v1/pokemon/translated/mewtwotranslated");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var model = await response.Deserialize<ApiErrorResponse>();

            model.ErrorCode.Should().Be(404);

            factory.HttpClientFactory.Verify(x => x.CreateClient("PokeApi"), Times.Once);
            factory.HttpClientFactory.Verify(x => x.CreateClient("TranslationApi"), Times.Never);
        }
    }
}
