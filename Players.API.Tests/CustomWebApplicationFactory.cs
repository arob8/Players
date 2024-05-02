using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Players.Application.Interfaces;
using Players.Application.Services;

namespace Players.API.Tests
{
    public class CustomWebApplicationFactory<TStartup>
       : WebApplicationFactory<TStartup> where TStartup : class
    {
        public Mock<PlayerService> PlayerServiceMock { get; } = new Mock<PlayerService>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real registration of these services
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IPlayerService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Use the mock set up in the factory, allowing test methods more control
                services.AddScoped<IPlayerService>(_ => PlayerServiceMock.Object);

                // Configure logging
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            });

            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Modify the configuration sources if needed
                config.AddJsonFile("appsettings.Testing.json", optional: true);
                config.AddEnvironmentVariables(prefix: "TESTING_");
            });
        }
    }
}
