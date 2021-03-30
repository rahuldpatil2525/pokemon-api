using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrueLayer.Pokemon.Api.Clients;
using TrueLayer.Pokemon.Api.Services;

namespace TrueLayer.Pokemon.Api.Installer
{
    public static class PokemonSpeciesInstaller
    {
        public static IServiceCollection ConfigurePokemonSpeciesServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IPokemonSpeciesService, PokemonSpeciesService>();
            services.TryAddSingleton<IPokeApiClient, PokeApiClient>();
            services.TryAddSingleton<ITranslationApiClient, TranslationApiClient>();
            services.TryAddSingleton<ITranslationStratergy, TranslationStratergy>();

            services.AddHttpClient("PokeApi", x =>
            {
                x.BaseAddress = new System.Uri("https://pokeapi.co/api/v2/pokemon-species/");
                x.DefaultRequestHeaders.Add("User-Agent", "TrueLayer.Pokemon.Api");
            });

            services.AddHttpClient("TranslationApi", x =>
            {
                x.BaseAddress = new System.Uri("https://api.funtranslations.com/translate/");
                x.DefaultRequestHeaders.Add("User-Agent", "TrueLayer.Pokemon.Api");
            });

            services.AddHttpClient("Yoda", x =>
            {
                x.BaseAddress = new System.Uri("https://api.funtranslations.com/translate/");
                x.DefaultRequestHeaders.Add("User-Agent", "TrueLayer.Pokemon.Api");
            });

            return services;
        }
    }
}
