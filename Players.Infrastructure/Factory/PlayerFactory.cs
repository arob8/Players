//using Players.Domain.Entities;
//using Players.Infrastructure.CbsSports.ResponseModel;
//using Players.Infrastructure.Interfaces;

//namespace Players.Infrastructure.Factory
//{
//    public abstract class PlayerFactory : IPlayerFactory
//    {
//        public abstract List<Player> CreatePlayers(List<PlayerData> players, Dictionary<string, int> avgAgePointDifferences);

//        public abstract Player CreatePlayer(PlayerData player, int? avgPositionAgeDiff);

//        public int? GetAveragePositionAgeDiff(string id, Dictionary<string, int> avgAgePointDifferences)
//        {
//            if (avgAgePointDifferences.TryGetValue(id, out int ageDiff))
//            {
//                return ageDiff;
//            }
//            return null;
//        }
//    }
//}
