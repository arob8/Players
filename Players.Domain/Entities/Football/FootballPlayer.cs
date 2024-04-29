using Players.Domain.Enums;

namespace Players.Domain.Entities.Football
{
    public class FootballPlayer : Player
    {
        public FootballPlayer(string id, string? nameBrief, string? firstName, string lastName, string position, int? age, int? averagePositionAgeDiff) : base(id, nameBrief, firstName, lastName, position, age, averagePositionAgeDiff, SportType.football){ }
    }
}
