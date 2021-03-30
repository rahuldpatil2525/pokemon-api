using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TrueLayer.Pokemon.Api.Logging;

namespace TrueLayer.Pokemon.Api.FunctionalTests.Factories
{
    public class PokemonWebApplicationFactory : WebApplicationFactory<Startup>
    {

        public PokemonWebApplicationFactory()
        {
            Instrumentor = new();
            HttpClientFactory = new();
        }

        public Mock<IInstrumentor> Instrumentor { get; }

        public Mock<IHttpClientFactory> HttpClientFactory { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.Add(ServiceDescriptor.Singleton(typeof(IInstrumentor), Instrumentor.Object));
                services.Add(ServiceDescriptor.Singleton(typeof(IHttpClientFactory), HttpClientFactory.Object));
            });            
        }
    }
}
