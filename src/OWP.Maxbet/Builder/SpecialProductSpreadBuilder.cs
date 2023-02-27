using OWP.Maxbet.Models;
using OWP.Services;

namespace OWP.Maxbet.Builder;

public interface SpecialProductSpreadBuilder
{
    Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId);
}

public class SpecialProductSpreadBuilderImp : SpecialProductSpreadBuilder
{
    private readonly MaxbetRepository syncMaxbetRepository;
    private readonly LogServiceWrapper logServiceWrapper;

    public SpecialProductSpreadBuilderImp(MaxbetRepository syncMaxbetRepository, LogServiceWrapper logServiceWrapper)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
        this.logServiceWrapper = logServiceWrapper;
    }

    public async Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId)
    {
        var spread = await syncMaxbetRepository.GetSpreadXMins(owpMatchId);
        await logServiceWrapper.Log("====BuildSpreadXMins====", spread);
        var spreadHdp = spread?.FirstOrDefault();
        return new MaxbetSpread
        {
            Handicap = spreadHdp,
            OverUnder = spreadHdp
        };
    }
}