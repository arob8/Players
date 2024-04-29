using Players.Domain.Enums;

namespace Players.Domain.Entities.Basketball
{
    public class BasketballPlayer : Player
    {
        public BasketballPlayer(string id, string? firstName, string lastName, string position, int? age, int? averagePositionAgeDiff) : base(id, firstName, lastName, position, age, averagePositionAgeDiff, SportType.basketball) { }
    }
}
