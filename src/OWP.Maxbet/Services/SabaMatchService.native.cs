using MediatR;
using OWP.Enums;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;

namespace OWP.Maxbet.Services;

public interface SabaMatchService
{
    public Task<MaxbetInfo> GetMaxbetInfoSaba(int owpMatchId, int adminsiteId);
}

public class SabaMatchServiceImp : SabaMatchService
{
    private readonly TraderAssignmentBuilder traderAssignmentBuilder;
    private readonly IMediator mediator;
    private readonly MaxbetRepository maxbetRepository;

    public SabaMatchServiceImp(TraderAssignmentBuilder traderAssignmentBuilder, IMediator mediator, MaxbetRepository maxbetRepository)
    {
        this.traderAssignmentBuilder = traderAssignmentBuilder;
        this.mediator = mediator;
        this.maxbetRepository = maxbetRepository;
    }

    public async Task<MaxbetInfo> GetMaxbetInfoSaba(int owpMatchId, int adminsiteId)
    {
        var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);

        var maxbetMainMatch = (await maxbetRepository.GetMaxbetByOwpMatchId(owpMatchId))
        .FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));

        var secondAcceptValue = await mediator.Send(new GetSecondAcceptValueHandler.Request
        { OWPMatchId = owpMatchId, Bettype = SpecialBettype.BETTYPE_ID_MAIN_MATCH, MaxbetHdp = maxbetMainMatch?.Value });

        var spread = new MaxbetSpread
        {
            Handicap = MaxBetSetting.SPREAD_SABA,
            OverUnder = MaxBetSetting.SPREAD_SABA
        };

        var maxbet = new MaxbetSpread
        {
            Handicap = MaxBetSetting.MAXBET_SABA,
            OverUnder = MaxBetSetting.MAXBET_SABA
        };

        var maxbetInfo = new MaxbetInfo
        {
            AdminsiteID = adminsiteId,
            Traders = traderAssignmentsSender,
            Spread = spread,
            Maxbet = maxbet,
            NumberOfLines = new NumberOfLines(),
            SecondAcceptValue = secondAcceptValue
        };

        MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

        return maxbetInfo;
    }
}