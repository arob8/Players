using Players.Application.Dtos;
using Players.Application.Common;

namespace Players.Application.Interfaces
{
    public interface IPlayerService
    {
        Task<ServiceResult<PlayerDto>> GetPlayerByIdAsync(int id);

        Task<ServiceResult<List<PlayerDto>>> SearchPlayersAsync(string? sport, string? lastNameStartsWith, int? age, int? minAge, int? maxAge, string? position);
    }
}