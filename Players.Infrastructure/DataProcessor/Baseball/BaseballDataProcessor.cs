using Players.Domain.Enums;
using Players.Domain.Interfaces;
using Players.Infrastructure.Factory.Baseball;
using Players.Infrastructure.Interfaces;

namespace Players.Infrastructure.DataProcessor.Baseball
{
    public class BaseballDataProcessor : DataProcessor
    {
        private readonly IPlayerFactory _baseballPlayerFactory;
        public BaseballDataProcessor(IClient client, IPlayerRepository playerRepository, BaseballPlayerFactory baseballPlayerFactory) : base(client, playerRepository, baseballPlayerFactory)
        {
            _baseballPlayerFactory = baseballPlayerFactory;
        }

        public override async Task ProcessPlayers()
        {
            var playerData = await _client.GetPlayers(SportType.football);

            // Get Average Age differences..

            var players = _baseballPlayerFactory.CreatePlayers(playerData, null);

            await _playerRepository.AddOrUpdatePlayersAsync(players);
        }
    }
}
