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
        }

        public Mock<IInstrumentor> Instrumentor { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(Services =>
            {
                Services.Add(ServiceDescriptor.Singleton(typeof(IInstrumentor), Instrumentor.Object));
            });
        }
    }
}
