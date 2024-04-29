using Players.Domain.Enums;

namespace Players.Domain.Entities.Baseball
{
    public class BaseballPlayer : Player
    {
        public BaseballPlayer(string id, string firstName, string lastName, string position, int? age, int? averagePositionAgeDiff) : base(id, firstName, lastName, position, age, averagePositionAgeDiff, SportType.baseball) { }
    }
}
