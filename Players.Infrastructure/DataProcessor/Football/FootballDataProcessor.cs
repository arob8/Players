using Players.Domain.Enums;
using Players.Domain.Interfaces;
using Players.Infrastructure.Factory.Football;
using Players.Infrastructure.Interfaces;

namespace Players.Infrastructure.DataProcessor.Football
{
    public class FootballDataProcessor : DataProcessor
    {
        private readonly IPlayerFactory _footballPlayerFactory;
        public FootballDataProcessor(IClient client, IPlayerRepository playerRepository, FootballPlayerFactory footballPlayerFactory) : base(client, playerRepository, footballPlayerFactory)
        {
            _footballPlayerFactory = footballPlayerFactory;
        }

        public override async Task ProcessPlayers()
        {
            var playerData = await _client.GetPlayers(SportType.football);

            // Get Average Age differences..

            var players = _footballPlayerFactory.CreatePlayers(playerData, null);

            await _playerRepository.AddOrUpdatePlayersAsync(players);
        }
    }
}
