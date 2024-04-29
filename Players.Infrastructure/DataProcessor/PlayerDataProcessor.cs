using Players.Domain.Interfaces;
using Players.Infrastructure.Factory;
using Players.Infrastructure.Interfaces;

namespace Players.Infrastructure.Processor
{
    public abstract class PlayerDataProcessor
    {
        public IClient _client { get; set; }

        public IPlayerRepository _playerRepo { get; set;}

        public IPlayerDataCalculator _playerCalculator { get; set; }

        public PlayerFactory _playerFactory { get; set; }

        public PlayerDataProcessor(IClient client, IPlayerRepository playerRepo, IPlayerDataCalculator playerCalculator, PlayerFactory playerFactory)
        {
            _client = client;
            _playerRepo = playerRepo;
            _playerCalculator = playerCalculator;
            _playerFactory = playerFactory;
        }

        public abstract Task ProcessPlayers();
    }
}
