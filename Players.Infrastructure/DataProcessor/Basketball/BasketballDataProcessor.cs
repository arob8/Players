using Players.Domain.Enums;
using Players.Domain.Interfaces;
using Players.Infrastructure.Factory.Basketball;
using Players.Infrastructure.Interfaces;

namespace Players.Infrastructure.DataProcessor.Basketball
{
    public class BasketballDataProcessor : DataProcessor
    {
        private readonly IPlayerFactory _basketballPlayerFactory;
        public BasketballDataProcessor(IClient client, IPlayerRepository playerRepository, BasketballPlayerFactory basketballPlayerFactory) : base(client, playerRepository, basketballPlayerFactory)
        {
            _basketballPlayerFactory = basketballPlayerFactory;
        }

        public override async Task ProcessPlayers()
        {
            var playerData = await _client.GetPlayers(SportType.basketball);

            // Get Average Age differences..

            var players = _basketballPlayerFactory.CreatePlayers(playerData, null);

            await _playerRepository.AddOrUpdatePlayersAsync(players);
        }
    }
}
