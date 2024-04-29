using Players.Domain.Entities;
using Players.Domain.Entities.Football;
using Players.Infrastructure.CbsSports.ResponseModel;
using Players.Infrastructure.Interfaces;

namespace Players.Infrastructure.Factory.Football
{
    public class FootballPlayerFactory : IPlayerFactory
    {
        public List<Player> CreatePlayers(List<PlayerData> players, Dictionary<string, int> avgAgePointDifferences)
        {
            var result = new List<Player>();
            foreach (PlayerData player in players)
            {
                int? avgPositionAgeDiff = GetAveragePositionAgeDiff(player.Id, avgAgePointDifferences);
                Player playerInfo = CreatePlayer(player, avgPositionAgeDiff);
                result.Add(playerInfo);
            }
            return result;
        }

        public Player CreatePlayer(PlayerData player, int? avgPositionAgeDiff)
        {
            return new FootballPlayer(player.Id, player.FirstName != "" ? player.FirstName : null, player.LastName, player.Position, player.Age, avgPositionAgeDiff);
        }

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
