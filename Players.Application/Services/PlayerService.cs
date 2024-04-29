using Players.Application.Common;
using Players.Application.Dtos;
using Players.Application.Interfaces;
using Players.Domain.Exceptions;
using Players.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Players.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepo;
        private IPlayerDtoDataMapper _playerDtoMapper;
        private ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository playerRepo, IPlayerDtoDataMapper playerDtoMapper, ILogger<PlayerService> logger)
        {
            _playerRepo = playerRepo;
            _playerDtoMapper = playerDtoMapper;
            _logger = logger;
        }


        public async Task<ServiceResult<PlayerDto>> GetPlayerByIdAsync(int id)
        {
            try
            {
                var player = await _playerRepo.FindPlayerByIdAsync(id);
                var playerDto = _playerDtoMapper.MapToPlayerDto(player);
                return new ServiceResult<PlayerDto> { Success = true, Data = playerDto };
            }
            catch (PlayerNotFoundException ex)
            {
                _logger.LogError(ex, "Player with ID {Id} was not found", id);
                return new ServiceResult<PlayerDto> { Success = false, ErrorMessage = "Player not found" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching player with ID {Id}", id);
                return new ServiceResult<PlayerDto> { Success = false, ErrorMessage = "An unexpected error occurred" };
            }
        }


        public async Task<ServiceResult<List<PlayerDto>>> SearchPlayersAsync(string? sport, string? lastNameStartsWith, int? age, int? minAge, int? maxAge, string? position)
        {
            try
            {
                if (age.HasValue && age < 0)
                    throw new ArgumentException("Age cannot be negative.", nameof(age));

                if (minAge.HasValue && minAge < 0)
                    throw new ArgumentException("Minimum age cannot be negative.", nameof(minAge));

                if (maxAge.HasValue && minAge < 0)
                    throw new ArgumentException("Minimum age cannot be negative.", nameof(maxAge));

                var players = await _playerRepo.FindPlayersByCriteriaAsync(sport, lastNameStartsWith, age, minAge, maxAge, position);
                if (players == null || !players.Any())
                {
                    return new ServiceResult<List<PlayerDto>>
                    {
                        Success = true,  // Successful query but no results
                        Data = new List<PlayerDto>(),
                        ErrorMessage = "No players found matching the criteria."
                    };
                }

                // Map the entities to DTOs
                var playerDtos = players.Select(player => _playerDtoMapper.MapToPlayerDto(player)).ToList();
                return new ServiceResult<List<PlayerDto>>
                {
                    Success = true,
                    Data = playerDtos
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument provided while searching players.");
                return new ServiceResult<List<PlayerDto>>
                {
                    Success = false,
                    ErrorMessage = "Invalid input argument: " + ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while searching players.");
                return new ServiceResult<List<PlayerDto>>
                {
                    Success = false,
                    ErrorMessage = "An unexpected error occurred: " + ex.Message
                };
            }
        }
    }

}