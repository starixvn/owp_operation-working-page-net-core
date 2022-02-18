using OWP.Maxbet.Models;
using OWP.Services;


namespace OWP.Maxbet.Builder
{
    public interface SpecialProductSpreadBuilder
    {
        Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId);
    }

    public class SpecialProductSpreadBuilderImp : SpecialProductSpreadBuilder
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;
        private readonly LogServiceWrapper logServiceWrapper;

        public SpecialProductSpreadBuilderImp(MaxbetReposiory syncMaxbetReposiory, LogServiceWrapper logServiceWrapper)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
            this.logServiceWrapper = logServiceWrapper;
        }

        public async Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId)
        {
            var spread = await syncMaxbetReposiory.GetSpreadXMins(owpMatchId);
            await logServiceWrapper.Log("====BuildSpreadXMins====", spread);
            var spreadHdp = spread?.FirstOrDefault();
            return new()
            {
                Handicap = spreadHdp,
                OverUnder = spreadHdp
            };
        }
    }
}