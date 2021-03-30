using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TrueLayer.Pokemon.Api.FunctionalTests.Fakes
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpResponseMessage ResponseMessage { get; set; }

        public Action<HttpRequestMessage> RequestValidationFunction { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestValidationFunction?.Invoke(request);
            return await Task.FromResult(ResponseMessage);
        }
    }
}
