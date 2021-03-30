using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrueLayer.Pokemon.Api.Configuration;

namespace TrueLayer.Pokemon.Api.Installer
{
    public static class ConfigurationInstaller
    {
        public static IServiceCollection ConfigureConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IRetryPolicyConfiguration>(x => configuration.GetSection("RetryPolicyConfiguration").Get<RetryPolicyConfiguration>());

            return services;
        }
    }
}
