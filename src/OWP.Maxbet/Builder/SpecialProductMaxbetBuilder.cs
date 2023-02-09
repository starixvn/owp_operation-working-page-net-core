using OWP.Maxbet.Models;
using OWP.Services;

namespace OWP.Maxbet.Builder;

public interface SpecialProductMaxbetBuilder
{
    Task<MaxbetSpread> BuildMaxbet(int owpMatchId);

    Task<MaxbetSpread> BuildMaxbetXMins(int owpMatchId);

    Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId);
}

public class SpecialProductMaxbetBuilderImp : SpecialProductMaxbetBuilder
{
    private readonly MaxbetRepository syncMaxbetRepository;
    private readonly MaxbetBuilder maxbetBuilder;
    private readonly LogServiceWrapper logServiceWrapper;

    public SpecialProductMaxbetBuilderImp(MaxbetRepository syncMaxbetRepository, MaxbetBuilder maxbetBuilder, LogServiceWrapper logServiceWrapper)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
        this.maxbetBuilder = maxbetBuilder;
        this.logServiceWrapper = logServiceWrapper;
    }

    public async Task<MaxbetSpread> BuildMaxbet(int owpMatchId)
    {
        var maxbet = await syncMaxbetRepository.GetMaxbetByOwpMatchId(owpMatchId);
        await logServiceWrapper.Log("==BuildMaxbet=====", maxbet);
        return !maxbet.Any()
            ? new()
            : MaxbetBuilderImp.BuildMaxbetSender(maxbet.ToList());
    }

    public async Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId)
    {
        var specialProductMaxBet = await maxbetBuilder.BuildMaxbetSpecialProduct(specialProduct, bettypeId);
        await logServiceWrapper.Log("==BuildMaxbetSpecialProduct=====", specialProductMaxBet);

        return specialProductMaxBet;
    }

    public async Task<MaxbetSpread> BuildMaxbetXMins(int owpMatchId)
    {
        var maxbet = await syncMaxbetRepository.GetMaxbetXMins(owpMatchId);
        await logServiceWrapper.Log("==BuildMaxbet=====", maxbet);
        var maxbetHdp = maxbet?.FirstOrDefault();
        return new()
        {
            Handicap = maxbetHdp,
            OverUnder = maxbetHdp
        };
    }
}