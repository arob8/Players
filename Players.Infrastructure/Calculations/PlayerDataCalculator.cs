using Players.Infrastructure.CbsSports.ResponseModel;
using Players.Infrastructure.Interfaces;

namespace Players.Infrastructure.Calculations
{
    public class PlayerDataCalculator :  IPlayerDataCalculator
    {
        /// <summary>
        /// Calculates the average age per position.
        /// </summary>
        /// <param name="players">List of player data.</param>
        /// <returns>A dictionary of positions and their corresponding average ages.</returns>
        public Dictionary<string, int> CalculateAverageAges(List<PlayerData> players)
        {
            if (players == null)
            {
                throw new ArgumentNullException(nameof(players), "Player list cannot be null.");
            }

            return players
                .Where(p => p.Age.HasValue)
                .GroupBy(p => p.Position)
                .Select(g => new
                {
                    Position = g.Key,
                    AverageAge = (int)Math.Round(g.Average(p => p.Age.Value))
                })
                .ToDictionary(g => g.Position, g => g.AverageAge);
        }

        /// <summary>
        /// Gets the average age difference for each player from the average age of their position.
        /// </summary>
        /// <param name="players">List of player data.</param>
        /// <returns>A dictionary with player IDs and their age difference from the average.</returns>
        public Dictionary<string, int> GetAverageAgeDifferences(List<PlayerData> players)
        {
            if (players == null)
            {
                throw new ArgumentNullException(nameof(players), "Player list cannot be null.");
            }

            var averageAges = CalculateAverageAges(players);

            return players
                 .Where(p => p.Age.HasValue && averageAges.ContainsKey(p.Position))
                 .ToDictionary(
                     p => p.Id,
                     p => (int)Math.Round((double)(p.Age.Value - averageAges[p.Position]))
                 );
        }
    }
}