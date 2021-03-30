using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace TrueLayer.Pokemon.Api.Builders
{
    public class TranslationRequestQueryStringBuilder
    {
        private readonly NameValueCollection _parameters;
        private string _provider;
        public TranslationRequestQueryStringBuilder()
        {
            _parameters = HttpUtility.ParseQueryString(string.Empty, Encoding.Unicode);
        }

        public string Build()
        {
            return $"{_provider}.json?{_parameters}";
        }

        public TranslationRequestQueryStringBuilder WithTranslationText(string translationText)
        {
            _parameters.Add("text", translationText);
            return this;
        }

        public TranslationRequestQueryStringBuilder WithTranslationProvider(string provider)
        {
            _provider = provider;
            return this;
        }
    }
}
