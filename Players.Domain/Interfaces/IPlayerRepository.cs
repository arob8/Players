using Players.Domain.Entities;

namespace Players.Domain.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> FindPlayerByIdAsync(int id);

        Task<List<Player>> FindPlayersByCriteriaAsync(string? sport, string? lastNameStartsWith, int? age, int? minAge, int? maxAge, string? position);

        Task AddOrUpdatePlayersAsync(List<Player> players);
    }
}
