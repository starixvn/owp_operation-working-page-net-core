using OWP.API.SyncMaxbet.model;

namespace OWP.API.SyncMaxbet.service
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
