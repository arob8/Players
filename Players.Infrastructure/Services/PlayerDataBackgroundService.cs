using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Players.Domain.Enums;
using Players.Infrastructure.DataProcessor.Baseball;
using Players.Infrastructure.DataProcessor.Basketball;
using Players.Infrastructure.DataProcessor.Football;
using Players.Infrastructure.Processor;

namespace Players.Infrastructure.Services
{
    public class PlayerDataBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<PlayerDataBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer? _timer = null;

        public PlayerDataBackgroundService(ILogger<PlayerDataBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Player background Hosted Service running.");
            // Set the initial delay
            var initialDelay = TimeSpan.FromHours(2);
            // Set the interval
            var interval = TimeSpan.FromDays(7);
            _timer = new Timer(async _ => await DoWork(), null, TimeSpan.Zero, TimeSpan.FromDays(7));
            return Task.CompletedTask;
        }

        private async Task DoWork()
        {
            try
            {
                _logger.LogInformation("Player background is working");
                using var scope = _serviceScopeFactory.CreateScope();

                try
                {
                    _logger.LogInformation("Player background is working.");
                    var sports = new[] { SportType.football, SportType.baseball, SportType.basketball };
                    foreach (var sport in sports)
                    {
                        await ProcessSport(sport, scope.ServiceProvider);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing player data.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing player data.");
            }
        }

        private async Task ProcessSport(SportType sport, IServiceProvider serviceProvider)
        {
            try
            {
                PlayerDataProcessor playerDataProcessor;

                switch (sport)
                {
                    case SportType.football:
                        playerDataProcessor = serviceProvider.GetRequiredService<FootballPlayerDataProcessor>();
                        break;
                    case SportType.baseball:
                        playerDataProcessor = serviceProvider.GetRequiredService<BaseballPlayerDataProcessor>();
                        break;
                    case SportType.basketball:
                        playerDataProcessor = serviceProvider.GetRequiredService<BasketballPlayerDataProcessor>();
                        break;
                    default:
                        throw new ArgumentException("Invalid sport type.");
                }

                await playerDataProcessor.ProcessPlayers();

                _logger.LogInformation($"Processed {sport} players data successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process {sport} players data.");
                throw;  // Rethrow to be caught by the general handler in DoWork if needed.
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}