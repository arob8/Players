using Newtonsoft.Json;
using Players.Domain.Enums;
using Players.Infrastructure.CbsSports.ResponseModel;
using Players.Infrastructure.Interfaces;
using System.Net.Http.Headers;

namespace Players.Infrastructure.CbsSports.Client
{
    public class CbsSportsClient : IClient
    {

        private const string APIBaseUrl = "http://api.cbssports.com/fantasy/players/list?version=3.0&SPORT=";

        private readonly HttpClient _client;

        public CbsSportsClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<PlayerData>> GetPlayers(SportType sport)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string url = GetURL(sport);
            int maxRetries = 3;
            int retryCount = 0;
            HttpResponseMessage response;

            while (retryCount < maxRetries)
            {
                try
                {
                    response = await _client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var playerResponse = JsonConvert.DeserializeObject<PlayerDataResponse>(json);
                        return playerResponse?.Body?.Players ?? new List<PlayerData>();
                    }
                    else
                    {
                        if (retryCount == maxRetries - 1)
                            throw new HttpRequestException($"Failed to retrieve data after {maxRetries} attempts.");
                        retryCount++;
                    }
                }
                catch (HttpRequestException)
                {
                    if (retryCount == maxRetries - 1)
                        throw; // Throw exception on the last retry
                    retryCount++;
                }
            }

            return new List<PlayerData>();
        }

        private string GetURL(SportType sport)
        {
            string sportType = sport.ToString().ToLowerInvariant();

            return $"{APIBaseUrl}{sportType}&response_format=JSON";
        }
    }
}