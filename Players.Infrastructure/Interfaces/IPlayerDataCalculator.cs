using Players.Infrastructure.CbsSports.ResponseModel;

namespace Players.Infrastructure.Interfaces
{
    public interface IPlayerDataCalculator
    {
        Dictionary<string, int> CalculateAverageAges(List<PlayerData> players);

        Dictionary<string, int> GetAverageAgeDifferences(List<PlayerData> players);
    }
}
