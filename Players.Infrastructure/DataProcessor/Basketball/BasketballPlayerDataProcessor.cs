using Players.Domain.Enums;
using Players.Domain.Interfaces;
using Players.Infrastructure.Factory.Basketball;
using Players.Infrastructure.Interfaces;
using Players.Infrastructure.Processor;

namespace Players.Infrastructure.DataProcessor.Basketball
{
    public class BasketballPlayerDataProcessor : PlayerDataProcessor
    {
        public BasketballPlayerDataProcessor(IClient client, IPlayerRepository playerRepo, IPlayerDataCalculator playerCalculator, BasketballPlayerFactory playerFactory) : base(client, playerRepo, playerCalculator, playerFactory) { }

        public override async Task ProcessPlayers()
        {
            var playerData = await _client.GetPlayers(SportType.football);

            Dictionary<string, int> playersAvgPointDiff = _playerCalculator.GetAverageAgeDifferences(playerData);

            var players = _playerFactory.CreatePlayers(playerData, playersAvgPointDiff);

            await _playerRepo.AddOrUpdatePlayersAsync(players);
        }
    }
}
