using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Contract.TranslationApi;
using TrueLayer.Pokemon.Api.Logging;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Services;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Services
{
    public class TranslationStratergyShould
    {
        private readonly Mock<ITranslationApiClient> _translationApiClient;
        private readonly Mock<IInstrumentor> _instrumentor;
        private TranslationStratergy _translationStratergy;

        public TranslationStratergyShould()
        {
            _translationApiClient = new();
            _instrumentor = new();

            _translationStratergy = new(_translationApiClient.Object, _instrumentor.Object);
        }

        [Fact]
        public async Task Returns_Translated_Text_When_Call_To_Translation_Api()
        {
            _translationApiClient.Setup(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new TranslationApiResponse()
            {
                Success = new()
                {
                    Total = 1
                },
                Contents = new()
                {
                    Text = "This is english text",
                    Translated = "This is translated text",
                    Translation = "Yoda"
                }
            });

            var request = new PokemonTranslationRequest()
            {
                Habitat = "rare",
                IsLegendary = false,
                TranslationText = "This is translation text"
            };

            var result = await _translationStratergy.GetTranslationAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsTranslated.Should().BeTrue();
            result.TranslationProvider.Should().Be("Yoda");
            result.TranslatedDescription.Should().Be("This is translated text");

            _translationApiClient.Verify(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>()), Times.Once);

            _instrumentor.Verify(x => x.LogException(It.IsAny<int>(), It.IsAny<Exception>(), It.IsAny<IDictionary<string, object>>()), Times.Never);
        }

        [Fact]
        public async Task Returns_Non_Translated_Text_When_TranslationApi_Failed()
        {
            _translationApiClient.Setup(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var request = new PokemonTranslationRequest()
            {
                Habitat = "rare",
                IsLegendary = false,
                TranslationText = "This is translation text"
            };

            var result = await _translationStratergy.GetTranslationAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsTranslated.Should().BeFalse();
            result.TranslationProvider.Should().BeNullOrEmpty();
            result.TranslatedDescription.Should().Be("This is translation text");

            _translationApiClient.Verify(x => x.GetTranslationAsync(It.IsAny<PokemonTranslationRequest>(), It.IsAny<CancellationToken>()), Times.Once);

            _instrumentor.Verify(x => x.LogException(It.IsAny<int>(), It.IsAny<Exception>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}
