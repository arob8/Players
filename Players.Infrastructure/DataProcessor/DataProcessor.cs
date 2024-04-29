using Players.Domain.Interfaces;
using Players.Infrastructure.Factory;
using Players.Infrastructure.Interfaces;

namespace Players.Infrastructure.DataProcessor
{
    public abstract class DataProcessor : IDataProcessor
    {
        public readonly IClient _client;
        public readonly IPlayerRepository _playerRepository;
        public readonly IPlayerFactory _playerFactory;
        //private IPlayerDataMapper _dataMapper;
        //private IPlayerDataCalculator _playerDataCalculator;

        public DataProcessor(IClient client, IPlayerRepository playerRepository, IPlayerFactory playerFactory /*, /*IPlayerDataCalculator playerDataCalculator, IPlayerDataMapper dataMapper, IPlayerRepository playerRepo*/)
        {
            _client = client;
            _playerRepository = playerRepository;
            _playerFactory = playerFactory;
            //_playerDataCalculator = playerDataCalculator;
            //_dataMapper = dataMapper;
            //_playerRepo = playerRepo;
        }

        public abstract Task ProcessPlayers();
        //{

        //    List<Player> players = await GetPlayers(sport);

        //    await _playerRepo.AddOrUpdatePlayersAsync(players);
        //}

        //public Task<List<Player>> GetPlayers()
        //{
        //    var playerData = await _client.GetPlayers(sport);

        //    Dictionary<string, int> playersAvgPointDiff = _playerDataCalculator.GetAverageAgeDifferences(playerData);

        //    return _dataMapper.MapToSportsPlayers(sport, playerData, playersAvgPointDiff);
        //}
    }
}
