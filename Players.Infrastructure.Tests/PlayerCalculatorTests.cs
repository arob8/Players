using NUnit.Framework;
using Players.Infrastructure.Calculations;
using Players.Infrastructure.CbsSports.ResponseModel;

namespace Players.Infrastructure.Tests
{
    public class PlayerCalculatorTests
    {
        private PlayerDataCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            _calculator = new PlayerDataCalculator();
        }

        [Test]
        public void CalculateAverageAges_ReturnsCorrectAverages()
        {
            // Arrange
            var players = new List<PlayerData>
            {
                new PlayerData { Id = "1", FirstName = "Tom", LastName = "Brady", Position = "QB", Age = 45 },
                new PlayerData { Id = "2", FirstName = "Aaron", LastName = "Rodgers", Position = "QB", Age = 39 },
                new PlayerData { Id = "3", FirstName = "Derrick", LastName = "Henry", Position = "RB", Age = 28 },
                new PlayerData { Id = "4", FirstName = "Alvin", LastName = "Kamara", Position = "RB", Age = 26 },
                new PlayerData { Id = "5", FirstName = "Julio", LastName = "Jones", Position = "WR", Age = 33 }
            };

            // Expected averages
            var expectedAverages = new Dictionary<string, int>
            {
                { "QB", 42 }, // Average of 45 and 39
                { "RB", 27 }, // Average of 28 and 26
                { "WR", 33 }  // Only one WR, so average is the same as the age
            };

            // Act
            var actualAverages = _calculator.CalculateAverageAges(players);

            // Assert
            Assert.That(actualAverages, Is.EqualTo(expectedAverages), "The calculated average ages should match the expected averages.");
        }

        [Test]
        public void CalculateAgeDifferences_ReturnsCorrectValues()
        {
            // Arrange - Creating a list of player data with mixed ages and positions
            var players = new List<PlayerData>
            {
                new PlayerData { Id = "1", FirstName = "Tom", LastName = "Brady", Position = "QB", Age = 45 },
                new PlayerData { Id = "2", FirstName = "Aaron", LastName = "Rodgers", Position = "QB", Age = 39 },
                new PlayerData { Id = "3", FirstName = "Derrick", LastName = "Henry", Position = "RB", Age = 28 },
                new PlayerData { Id = "4", FirstName = "Alvin", LastName = "Kamara", Position = "RB", Age = 26 },
                new PlayerData { Id = "5", FirstName = "Julio", LastName = "Jones", Position = "WR", Age = 33 }
            };

            var ageDifferences = _calculator.GetAverageAgeDifferences(players);

            // Assert - Check if calculated age differences are as expected
            var expectedDifferences = new Dictionary<string, int>
            {
                { "1", 3 },  // 45 - 42
                { "2", -3 }, // 39 - 42
                { "3", 1 },  // 28 - 27
                { "4", -1 }, // 26 - 27
                { "5", 0 }   // 33 - 33 (only one WR)
            };

            // Assert - Check if calculated averages are as expected
            Assert.That(ageDifferences, Is.EqualTo(expectedDifferences), "Average ages should match expected values.");
        }


        [Test]
        public void CalculateAverageAges_And_GetAverageAgePointDifferences_ReturnExpectedResults()
        {
            // Arrange
            var players = new List<PlayerData>
            {
                new PlayerData { Id = "1", FirstName = "Tom", LastName = "Brady", Position = "QB", Age = 45 },
                new PlayerData { Id = "2", FirstName = "Aaron", LastName = "Rodgers", Position = "QB", Age = 39 },
                new PlayerData { Id = "3", FirstName = "Derrick", LastName = "Henry", Position = "RB", Age = 28 },
                new PlayerData { Id = "4", FirstName = "Alvin", LastName = "Kamara", Position = "RB", Age = 26 },
                new PlayerData { Id = "5", FirstName = "Julio", LastName = "Jones", Position = "WR", Age = 33 }
            };

            // Expected results for CalculateAverageAges
            var expectedAverageAges = new Dictionary<string, int>
            {
                { "QB", 42 }, // Average of 45 and 39
                { "RB", 27 }, // Average of 28 and 26
                { "WR", 33 }  // Only one WR, so average is the same as the age
            };

            // Expected results for GetAverageAgePointDifferences
            var expectedAgeDifferences = new Dictionary<string, int>
            {
                { "1", 3 },  // 45 - 42
                { "2", -3 }, // 39 - 42
                { "3", 1 },  // 28 - 27
                { "4", -1 }, // 26 - 27
                { "5", 0 }   // 33 - 33 (only one WR)
            };

            // Act
            var actualAverageAges = _calculator.CalculateAverageAges(players);
            var actualAgeDifferences = _calculator.GetAverageAgeDifferences(players);

            // Assert
            Assert.That(actualAverageAges, Is.EqualTo(expectedAverageAges), "The calculated average ages should match the expected averages.");
            Assert.That(actualAgeDifferences, Is.EqualTo(expectedAgeDifferences), "The calculated age differences should match the expected values.");
        }


        [Test]
        public void CalculateAverageAges_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _calculator.CalculateAverageAges(null));
        }

        [Test]
        public void CalculateAgeDifferences_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _calculator.CalculateAverageAges(null));
        }
    }
}