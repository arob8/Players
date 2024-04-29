using Microsoft.AspNetCore.Mvc;
using Players.Application.Dtos;
using Players.Domain.Enums;
using Players.Application.Interfaces;

namespace Players.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
        {
            _playerService = playerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
        {
            var result = await _playerService.GetPlayerByIdAsync(id);
            if (!result.Success)
            {
                if (result.ErrorMessage == "Player not found")
                {
                    return NotFound(new { Message = result.ErrorMessage });
                }
                else
                {
                    return StatusCode(500, new { Message = result.ErrorMessage });
                }
            }
            return Ok(result.Data);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<PlayerDto>>> SearchPlayers([FromQuery] string? sport, [FromQuery] string? lastNameStartsWith, [FromQuery] int? age, [FromQuery] int? minAge, [FromQuery] int? maxAge, [FromQuery] string? position)
        {
            if (sport == null && lastNameStartsWith == null && age == null && minAge == null && maxAge == null && position == null)
            {
                return BadRequest("Please provide at least one search criteria.");
            }

            if (!string.IsNullOrEmpty(sport) && !Enum.IsDefined(typeof(SportType), sport))
            {
                return BadRequest("Invalid sport type provided.");
            }

            var result = await _playerService.SearchPlayersAsync(sport, lastNameStartsWith, age, minAge, maxAge, position);

            if (!result.Success)
            {
                _logger.LogError("Search failed: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }

            if (result.Data == null || !result.Data.Any())
            {
                return NotFound("No players found matching the criteria.");
            }

            return Ok(result.Data);
        }
    }
}
