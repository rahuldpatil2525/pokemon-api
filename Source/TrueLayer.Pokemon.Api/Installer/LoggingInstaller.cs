using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using TrueLayer.Pokemon.Api.Logging;
using TrueLayer.Pokemon.Api.Middleware.ExceptionHandler;

namespace TrueLayer.Pokemon.Api.Installer
{
    public static class LoggingInstaller
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(options =>
            {
                options.EnableDependencyTrackingTelemetryModule = false;
            });

            services.TryAddSingleton<IInstrumentor, ApplicationInsightsInstrumentor>();
            services.TryAddSingleton<IExceptionToErrorResponseMapping, ExceptionToErrorResponseMapping>();

            services.AddLogging(builder => 
            {
                builder.AddConfiguration(configuration);
                builder.AddConsole();
            });

            return services;
        }
    }
}
