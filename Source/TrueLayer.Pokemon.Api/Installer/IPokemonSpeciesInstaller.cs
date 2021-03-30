using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrueLayer.Pokemon.Api.Services;

namespace TrueLayer.Pokemon.Api.Installer
{
    public static class IPokemonSpeciesInstaller
    {
        public static IServiceCollection ConfigurePokemonSpeciesServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IPokemonSpeciesService, PokemonSpeciesService>();

            return services;
        }
    }
}
