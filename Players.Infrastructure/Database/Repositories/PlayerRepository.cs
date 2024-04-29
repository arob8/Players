using Microsoft.EntityFrameworkCore;
using Players.Domain.Entities;
using Players.Domain.Enums;
using Players.Domain.Exceptions;
using Players.Domain.Interfaces;
using Players.Infrastructure.Database.Context;

namespace Players.Infrastructure.Database.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly PlayersContext _context;

        public PlayerRepository(PlayersContext context)
        {
            _context = context;
        }

        // Get a player by ID
        public async Task<Player> FindPlayerByIdAsync(int id)
        {
            var player = await _context.Players.FindAsync(id.ToString());
            if (player == null)
            {
                throw new PlayerNotFoundException(id);
            }
            return player;
        }

        public async Task<List<Player>> FindPlayersByCriteriaAsync(string? sport, string? lastNameStartsWith, int? age, int? minAge, int? maxAge, string? position)
        {
            try
            {
                var query = _context.Players.AsQueryable();

                if (Enum.TryParse<SportType>(sport, out SportType sportType))
                {
                    query = query.Where(p => p.Sport == sportType);
                }

                if (!string.IsNullOrEmpty(lastNameStartsWith))
                    query = query.Where(p => p.LastName.StartsWith(lastNameStartsWith));

                if (age.HasValue)
                    query = query.Where(p => p.Age == age.Value);

                if (minAge.HasValue && maxAge.HasValue)
                {
                    query = query.Where(p => p.Age >= minAge.Value && p.Age <= maxAge.Value);
                }
                else if (minAge.HasValue)
                {
                    query = query.Where(p => p.Age >= minAge.Value);
                }
                else if (maxAge.HasValue)
                {
                    query = query.Where(p => p.Age <= maxAge.Value);
                }

                if (!string.IsNullOrEmpty(position))
                    query = query.Where(p => p.Position == position);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred during the search operation.", ex);
            }
        }


        // Add or update players
        public async Task AddOrUpdatePlayersAsync(List<Player> players)
        {
            foreach (var player in players)
            {
                var existingPlayer = await _context.Players.FindAsync(player.Id);
                if (existingPlayer != null)
                {
                    // Map properties from Player to the existing Player entity
                    _context.Entry(existingPlayer).CurrentValues.SetValues(player);
                }
                else
                {
                    _context.Players.Add(player);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred during add or update player operation.", ex);
            }
        }
    }
}