using Players.Domain.Entities;
using Players.Domain.Entities.Football;
using Players.Domain.Utilities.NameFormatter.Football;
using Players.Infrastructure.CbsSports.ResponseModel;

namespace Players.Infrastructure.Factory.Football
{
    public class FootballPlayerFactory : PlayerFactory
    {
        public FootballPlayerFactory(FootballPlayerNameFormatter nameFormatter) : base(nameFormatter)
        {
        }

        public override FootballPlayer CreatePlayer(PlayerData player, int? avgPositionAgeDiff)
        {
            return new FootballPlayer(player.Id, _nameFormatter.GenerateNameBrief(player.FirstName, player.LastName), player.FirstName != "" ? player.FirstName : null, player.LastName, player.Position, player.Age, avgPositionAgeDiff);
        }

        public override List<Player> CreatePlayers(List<PlayerData> players, Dictionary<string, int> avgAgePointDifferences)
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
    }
}
