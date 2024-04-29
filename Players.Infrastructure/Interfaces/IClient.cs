using Players.Domain.Enums;
using Players.Infrastructure.CbsSports.ResponseModel;

namespace Players.Infrastructure.Interfaces
{
    public interface IClient
    {
        public Task<List<PlayerData>> GetPlayers(SportType sport);
    }
}
