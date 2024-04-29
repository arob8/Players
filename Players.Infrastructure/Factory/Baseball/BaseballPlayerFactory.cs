using Players.Domain.Entities;
using Players.Infrastructure.CbsSports.ResponseModel;
using Players.Domain.Entities.Baseball;
using Players.Domain.Utilities.NameFormatter.Baseball;

namespace Players.Infrastructure.Factory.Baseball
{
    public class BaseballPlayerFactory : PlayerFactory
    {
        public BaseballPlayerFactory(BaseballPlayerNameFormatter nameFormatter) : base(nameFormatter)
        {
        }

        public override Player CreatePlayer(PlayerData player, int? avgPositionAgeDiff)
        {
            return new BaseballPlayer(player.Id, _nameFormatter.GenerateNameBrief(player.FirstName, player.LastName), player.FirstName != "" ? player.FirstName : null, player.LastName, player.Position, player.Age, avgPositionAgeDiff);
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
