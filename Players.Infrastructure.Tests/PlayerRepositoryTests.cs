using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Players.Domain.Entities;
using Players.Domain.Entities.Football;
using Players.Infrastructure.Database.Context;
using Players.Infrastructure.Database.Repositories;

namespace Players.Infrastructure.Tests
{
    [TestFixture]
    public class PlayerRepositoryTests
    {
        private PlayersContext _context;
        private PlayerRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PlayersContext>()
                 .UseInMemoryDatabase(databaseName: "TestPlayerDb")
                 .Options;

            _context = new PlayersContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _repository = new PlayerRepository(_context);
        }

        [Test]
        public async Task AddOrUpdatePlayersAsync_AddsNewPlayers_WhenTheyDoNotExist()
        {
            // Arrange
            var newPlayers = new List<Player>
            {
                new FootballPlayer("1", "A. Rodgers", "Aaron", "Rodgers", "QB", 42, 10),
                new FootballPlayer("2", "J. Love", "Jordan", "Love", "QB", 25, -3)
            };

            // Act
            await _repository.AddOrUpdatePlayersAsync(newPlayers);
            await _context.SaveChangesAsync();

            // Assert
            var playersInDb = await _context.Players.ToListAsync();
            Assert.That(playersInDb, Has.Count.EqualTo(2));
            Assert.That(playersInDb[0].FirstName, Is.EqualTo("Aaron"));
            Assert.That(playersInDb[1].FirstName, Is.EqualTo("Jordan"));
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}