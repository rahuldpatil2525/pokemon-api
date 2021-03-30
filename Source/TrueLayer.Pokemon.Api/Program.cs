using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TrueLayer.Pokemon.Api.Configuration;

namespace TrueLayer.Pokemon.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                throw new ApplicationException("Failed to retrieve environment from environment variable.");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var kestralEncryptionOptions = configuration.GetSection("KestralEncryptionOptions").Get<KestralEncryptionOptions>();

            hostBuilder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                //Add Azure Keyvault configuration
            });

            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                if (kestralEncryptionOptions != null && kestralEncryptionOptions.Enabled)
                {
                    //Add and manage certificate
                    webBuilder.UseKestrel(options => { options.ListenAnyIP(kestralEncryptionOptions.ContainerPort); });
                }

                webBuilder.UseStartup<Startup>();
            });
            return hostBuilder;
        }
    }
}
