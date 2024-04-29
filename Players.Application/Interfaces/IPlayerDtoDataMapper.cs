using Players.Application.Dtos;
using Players.Domain.Entities;

namespace Players.Application.Interfaces
{
    public interface IPlayerDtoDataMapper
    {
        PlayerDto MapToPlayerDto(Player player);
    }
}
