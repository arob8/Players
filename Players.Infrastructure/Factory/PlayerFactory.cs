using Players.Domain.Entities;
using Players.Domain.Utilities.NameFormatter;
using Players.Infrastructure.CbsSports.ResponseModel;

namespace Players.Infrastructure.Factory
{
    public abstract class PlayerFactory
    {
        public PlayerNameFormatter _nameFormatter { get; set; }

        public PlayerFactory(PlayerNameFormatter nameFormatter)
        {
            _nameFormatter = nameFormatter;
        }

        public abstract Player CreatePlayer(PlayerData player, int? avgPositionAgeDiff);

        public abstract List<Player> CreatePlayers(List<PlayerData> players, Dictionary<string, int> avgAgePointDifferences);

        public int? GetAveragePositionAgeDiff(string id, Dictionary<string, int> avgAgePointDifferences)
        {
            if (avgAgePointDifferences.TryGetValue(id, out int ageDiff))
            {
                return ageDiff;
            }
            return null;
        }
    }
}
