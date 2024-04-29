using Players.Domain.Enums;

namespace Players.Domain.Entities.Baseball
{
    public class BaseballPlayer : Player
    {
        public BaseballPlayer(string id, string? nameBrief, string? firstName, string lastName, string position, int? age, int? averagePositionAgeDiff) : base(id, nameBrief, firstName, lastName, position, age, averagePositionAgeDiff, SportType.baseball) { }
    }
}
