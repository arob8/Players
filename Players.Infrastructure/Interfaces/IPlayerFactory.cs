using Players.Domain.Entities;
using Players.Infrastructure.CbsSports.ResponseModel;

namespace Players.Infrastructure.Interfaces
{
    public interface IPlayerFactory
    {
        public List<Player> CreatePlayers(List<PlayerData> players, Dictionary<string, int> avgAgePointDifferences);

        public Player CreatePlayer(PlayerData player, int? avgPositionAgeDiff);

        public int? GetAveragePositionAgeDiff(string id, Dictionary<string, int> avgAgePointDifferences);
    }
}
