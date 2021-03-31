using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Provider;
using TrueLayer.Pokemon.Api.Services;

namespace TrueLayer.Pokemon.Api.Installer
{
    public static class PokemonSpeciesInstaller
    {
        public static IServiceCollection ConfigurePokemonSpeciesServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IPokemonSpeciesService, PokemonSpeciesService>();
            services.TryAddSingleton<IPokeApiClient, PokeApiClient>();
            services.TryAddSingleton<ITranslationApiClient, TranslationApiClient>();
            services.TryAddSingleton<ITranslationStratergy, TranslationStratergy>();
            services.TryAddSingleton<IRetryPolicyProvider, RetryPolicyProvider>();
            services.TryAddSingleton<IPokemonSpeciesRetriever, PokemonSpeciesRetriever>();
            services.AddHttpClient("PokeApi", x =>
            {
                x.BaseAddress = new System.Uri(configuration.GetSection("ApiClientConfiguration:PokeApiBaseUrl").Get<string>());
                x.DefaultRequestHeaders.Add("User-Agent", "TrueLayer.Pokemon.Api");
            });

            services.AddHttpClient("TranslationApi", x =>
            {
                x.BaseAddress = new System.Uri(configuration.GetSection("ApiClientConfiguration:TranslationApiBaseUrl").Get<string>());
                x.DefaultRequestHeaders.Add("User-Agent", "TrueLayer.Pokemon.Api");
            });

            return services;
        }
    }
}
