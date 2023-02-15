namespace OWP.Maxbet.Builder;

using Models;
using OWP.Services;

public interface SpecialProductTraderBuilder
{
    public Task<TraderAssignmentSender> BuildTraderXMins(int specialProductId);
}

public class SpecialProductTraderBuilderImp : SpecialProductTraderBuilder
{
    private readonly MaxbetRepository maxbetRepository;

    private readonly LogServiceWrapper logServiceWrapper;

    public SpecialProductTraderBuilderImp(MaxbetRepository maxbetRepository, LogServiceWrapper logServiceWrapper)
    {
        this.maxbetRepository = maxbetRepository;
        this.logServiceWrapper = logServiceWrapper;
    }

    public async Task<TraderAssignmentSender> BuildTraderXMins(int specialProductId)
    {
        var traders = await maxbetRepository.GetTraderXMins(specialProductId);
        await logServiceWrapper.Log("=====BuildTraderXMins======", traders);
        var traderHdp = traders?.FirstOrDefault();
        return new TraderAssignmentSender
        {
            Handicap = traderHdp,
            OverUnder = traderHdp
        };
    }
}