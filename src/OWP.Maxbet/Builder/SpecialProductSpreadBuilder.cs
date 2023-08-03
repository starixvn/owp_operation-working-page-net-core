using OWP.Maxbet.Models;

namespace OWP.Maxbet.Builder;

public interface SpecialProductSpreadBuilder
{
    Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId);
}

public class SpecialProductSpreadBuilderImp : SpecialProductSpreadBuilder
{
    private readonly MaxbetRepository syncMaxbetRepository;

    public SpecialProductSpreadBuilderImp(MaxbetRepository syncMaxbetRepository)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
    }

    public async Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId)
    {
        var spread = await syncMaxbetRepository.GetSpreadXMins(owpMatchId);
        var spreadHdp = spread?.FirstOrDefault();
        return new MaxbetSpread
        {
            Handicap = spreadHdp,
            OverUnder = spreadHdp
        };
    }
}