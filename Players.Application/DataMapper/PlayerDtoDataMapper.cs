using Players.Application.Dtos;
using Players.Application.Interfaces;
using Players.Domain.Entities;

namespace Players.Application.DataMapper
{
    public class PlayerDtoDataMapper : IPlayerDtoDataMapper
    {
        public PlayerDto MapToPlayerDto(Player player)
        {
            return new PlayerDto
            {
                id = player.Id,
                name_brief = player.NameBrief != null ? player.NameBrief.ToString() : null,
                first_name = player.FirstName != null ? player.FirstName.ToString() : null,
                last_name = player.LastName,
                position = player.Position,
                age = player.Age != null ? player.Age.ToString() : null,
                average_position_age_diff = player.AveragePositionAgeDiff != null ? player.AveragePositionAgeDiff.ToString() : null
            };
        }
    }
}
