using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Players.Domain.Enums;
using Players.Infrastructure.CbsSports.Client;
using Players.Infrastructure.CbsSports.ResponseModel;
using System.Net;

namespace Players.Infrastructure.Tests
{
    public class CbsSportsClientTests
    {
        private Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private CbsSportsClient _cbsSportsClient;

        [SetUp]
        public void Setup()
        {
            // Mock the HTTP handler to intercept the outgoing requests
            _handlerMock = new Mock<HttpMessageHandler>();

            // Set up the HttpClient to use the mocked handler
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://api.cbssports.com/fantasy/players/list?version=3.0&SPORT=")
            };

            _cbsSportsClient = new CbsSportsClient(_httpClient);
        }

        [Test]
        public async Task GetPlayers_ReturnsPlayers_WhenApiCallIsSuccessful()
        {
            // Arrange - Create a fake API response
            var fakeResponse = new PlayerDataResponse
            {
                Body = new PlayerDataList
                {
                    Players = new List<PlayerData>
                {
                    new PlayerData { Id = "4312", FirstName = "Aaron", LastName = "Rodgers" },
                    new PlayerData { Id = "6543", FirstName = "Jordan", LastName = "Love" }
                }
                }
            };

            // Serialize the fake response to JSON
            var json = JsonConvert.SerializeObject(fakeResponse);

            // Set up the handler response
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                });

            // Act - Call the method that should be tested
            var result = await _cbsSportsClient.GetPlayers(SportType.football);

            // Assert - Verify that the method returns the expected results
            Assert.IsNotNull(result, "The result should not be null when the API call is successful.");
            Assert.That(result.Count, Is.EqualTo(2), "The number of players returned should match the number in the fake response.");
            Assert.That(result[0].FirstName, Is.EqualTo("Aaron"), "The first name of the first player should be 'Aaron'.");
        }

        [Test]
        public async Task GetPlayers_ThrowsHttpRequestException_WhenApiCallFails()
        {
            // Arrange - Simulate a server error response
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act & Assert - Ensure that the method throws an HttpRequestException when the API call fails
            Assert.ThrowsAsync<HttpRequestException>(() => _cbsSportsClient.GetPlayers(SportType.football),
                "An HttpRequestException should be thrown if the API call results in a server error.");
        }
    }
}