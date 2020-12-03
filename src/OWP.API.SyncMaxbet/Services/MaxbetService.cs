using OWP.API.SyncMaxbet.Models;

namespace OWP.API.SyncMaxbet.Services
{
    public interface MaxbetService
    {
        public MaxbetInfo GetMaxbetInfo(int matchId);
    }

    public class MaxbetServiceImp : MaxbetService
    {

        public MaxbetInfo GetMaxbetInfo(int matchId)
        {
            return new MaxbetInfo();
        }
    }
}
