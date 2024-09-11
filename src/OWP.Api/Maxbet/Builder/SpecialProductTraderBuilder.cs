using OWP.Api.Maxbet.Models;

namespace OWP.Api.Maxbet.Builder;

public interface SpecialProductTraderBuilder
{
    public Task<TraderAssignmentSender> BuildTraderXMins(int specialProductId);
}

public class SpecialProductTraderBuilderImp : SpecialProductTraderBuilder
{
    private readonly MaxbetRepository maxbetRepository;

    public SpecialProductTraderBuilderImp(MaxbetRepository maxbetRepository)
    {
        this.maxbetRepository = maxbetRepository;
    }

    public async Task<TraderAssignmentSender> BuildTraderXMins(int specialProductId)
    {
        var traders = await maxbetRepository.GetTraderXMins(specialProductId);
        var traderHdp = traders?.FirstOrDefault();
        return new TraderAssignmentSender
        {
            Handicap = traderHdp,
            OverUnder = traderHdp
        };
    }
}