using NUnit.Framework;
using Moq;
using Players.Application.Dtos;
using Players.Application.Common;
using System.Text.Json;
using System.Net;
using System.Net.Http.Json;

namespace Players.API.Tests
{
    public class PlayerControllerTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Startup> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();

            // Setup mock behavior for each test
            _factory.PlayerServiceMock.Setup(p => p.GetPlayerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ServiceResult<PlayerDto> { Success = true, Data = new PlayerDto { id = "419780", first_name = "Aaron", last_name = "Rodgers" } });
        }

        [Test]
        public async Task GetPlayer_ReturnsOk_WhenPlayerExists()
        {
            // Act
            var response = await _client.GetAsync("/players/419780");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            var player = JsonSerializer.Deserialize<PlayerDto>(responseString);

            if (player == null)
            {
                throw new InvalidOperationException("Failed to deserialize the player data.");
            }

            Assert.That(player.first_name, Is.EqualTo("Aaron"));
            Assert.That(player.last_name, Is.EqualTo("Rodgers"));

            // Verify that the service was called
            _factory.PlayerServiceMock.Verify(p => p.GetPlayerByIdAsync(419780), Times.Once);
        }

        [Test]
        public async Task GetPlayer_ReturnsNotFound_WhenPlayerDoesNotExist()
        {
            // Arrange
            int playerId = 0;
            _factory.PlayerServiceMock.Setup(p => p.GetPlayerByIdAsync(playerId))
                .ReturnsAsync(new ServiceResult<PlayerDto> { Success = false, ErrorMessage = "Player not found" });

            // Act
            var response = await _client.GetAsync($"/players/{playerId}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseContent.Contains("Player not found"));

            // Verify that the service was called
            _factory.PlayerServiceMock.Verify(p => p.GetPlayerByIdAsync(0), Times.Once);
        }

        [Test]
        public async Task GetPlayer_ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            int playerId = 0;
            _factory.PlayerServiceMock.Setup(p => p.GetPlayerByIdAsync(playerId))
                .ReturnsAsync(new ServiceResult<PlayerDto> { Success = false, ErrorMessage = "An unexpected error occurred" });

            // Act
            var response = await _client.GetAsync($"/players/{playerId}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseContent.Contains("An unexpected error occurred"));
        }

        [Test]
        public async Task SearchPlayers_ReturnsNotFound_WhenCriteriaMetButNoPlayersFound()
        {
            // Arrange
            _factory.PlayerServiceMock.Setup(p => p.SearchPlayersAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResult<List<PlayerDto>> { Success = true, Data = new List<PlayerDto>() });

            // Act
            var response = await _client.GetAsync("/players/search?position=Shark");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseContent.Contains("No players found matching the criteria."));
        }

        [Test]
        public async Task SearchPlayers_ReturnsBadRequest_WhenInvalidArguments()
        {
            // Arrange
            _factory.PlayerServiceMock.Setup(p => p.SearchPlayersAsync(It.IsAny<string>(), It.IsAny<string>(), -1, null, null, null))
                .ReturnsAsync(new ServiceResult<List<PlayerDto>> { Success = false, ErrorMessage = "Invalid input argument: Age cannot be negative" });

            // Act
            var response = await _client.GetAsync("/players/search?age=-1");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseContent.Contains("Invalid input argument: Age cannot be negative"));
        }

        [Test]
        public async Task SearchPlayers_ReturnsOk_WithMultipleFilters()
        {
            // Arrange
            var expectedPlayers = new List<PlayerDto>
            {
                new PlayerDto { id = "419780", first_name = "Aaron", last_name = "Rodgers", name_brief = "A. Rodgers" }
            };
            _factory.PlayerServiceMock.Setup(p => p.SearchPlayersAsync("football", "R", 40, 40, 40, "QB"))
                .ReturnsAsync(new ServiceResult<List<PlayerDto>> { Success = true, Data = expectedPlayers });

            // Act
            var response = await _client.GetAsync("/players/search?sport=football&lastNameStartsWith=R&age=40&minAge=40&maxAge=40&position=QB");

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            var players = await response.Content.ReadFromJsonAsync<List<PlayerDto>>();
            Assert.That(players, Is.Not.Empty);
            Assert.That(players[0].first_name, Is.EqualTo("Aaron"));
            Assert.That(players[0].last_name, Is.EqualTo("Rodgers"));
            Assert.That(players[0].name_brief, Is.EqualTo("A. Rodgers"));
        }
    }
}