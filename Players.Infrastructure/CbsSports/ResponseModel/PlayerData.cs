namespace Players.Infrastructure.CbsSports.ResponseModel
{
    public class PlayerDataResponse
    {
        public string Uri { get; set; }
        public int StatusCode { get; set; }
        public PlayerDataList Body { get; set; }
    }

    public class PlayerDataList
    {
        public List<PlayerData> Players { get; set; }
    }

    public class PlayerData
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string EliasId { get; set; }
        public int EligibleForOffenseAndDefense { get; set; }
        public string ProTeam { get; set; }
        public string EligiblePositionsDisplay { get; set; }
        public string Throws { get; set; }
        public string Position { get; set; }
        public string Photo { get; set; }
        public string ProStatus { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Bats { get; set; }
        public int? Age { get; set; }  // Nullable since not all players have age data
        public int? Jersey { get; set; }  // Nullable since not all players have jersey numbers
    }
}
