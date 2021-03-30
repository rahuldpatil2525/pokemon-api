using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Contract.TranslationApi;

namespace TrueLayer.Pokemon.Api.FunctionalTests.Builders
{
    public class TranslationApiResponseBuilder
    {
        private string _translated = "This is Translated.";

        private string _text = "This is not Translated.";

        private string _translation = "yoda";

        public TranslationApiResponseBuilder WithTranslated(string translated)
        {
            _translated = translated;
            return this;
        }

        public TranslationApiResponseBuilder WithText(string text)
        {
            _text = text;
            return this;
        }

        public TranslationApiResponseBuilder WithTranslation(string translation)
        {
            _translation = translation;
            return this;
        }

        public TranslationApiResponse Create()
        {
            return new()
            {
                Success = new()
                {
                    Total = 1
                },
                Contents = new()
                {
                    Text = _text,
                    Translated = _translated,
                    Translation = _translation
                }
            };
        }
    }
}
