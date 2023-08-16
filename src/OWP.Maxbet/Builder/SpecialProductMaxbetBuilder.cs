using OWP.Maxbet.Models;

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

    public SpecialProductMaxbetBuilderImp(MaxbetRepository syncMaxbetRepository, MaxbetBuilder maxbetBuilder)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
        this.maxbetBuilder = maxbetBuilder;
    }

    public async Task<MaxbetSpread> BuildMaxbet(int owpMatchId)
    {
        var maxbet = (await syncMaxbetRepository.GetMaxbetByOwpMatchId(owpMatchId)).ToList();
        return !maxbet.Any()
            ? new MaxbetSpread()
            : MaxbetBuilderImp.BuildMaxbetSender(maxbet.ToList());
    }

    public async Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId)
    {
        var specialProductMaxBet = await maxbetBuilder.BuildMaxbetSpecialProduct(specialProduct, bettypeId);

        return specialProductMaxBet;
    }

    public async Task<MaxbetSpread> BuildMaxbetXMins(int owpMatchId)
    {
        var maxbet = await syncMaxbetRepository.GetMaxbetXMins(owpMatchId);
        var maxbetHdp = maxbet?.FirstOrDefault();
        return new MaxbetSpread
        {
            Handicap = maxbetHdp,
            OverUnder = maxbetHdp
        };
    }
}