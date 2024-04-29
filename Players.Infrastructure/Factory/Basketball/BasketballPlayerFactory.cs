using Players.Domain.Entities;
using Players.Infrastructure.CbsSports.ResponseModel;
using Players.Domain.Entities.Basketball;
using Players.Domain.Utilities.NameFormatter.Basketball;

namespace Players.Infrastructure.Factory.Basketball
{
    public class BasketballPlayerFactory : PlayerFactory
    {
        public BasketballPlayerFactory(BasketballPlayerNameFormatter nameFormatter) : base(nameFormatter)
        {
        }

        public override Player CreatePlayer(PlayerData player, int? avgPositionAgeDiff)
        {
            return new BasketballPlayer(player.Id, _nameFormatter.GenerateNameBrief(player.FirstName, player.LastName), player.FirstName != "" ? player.FirstName : null, player.LastName, player.Position, player.Age, avgPositionAgeDiff);
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
