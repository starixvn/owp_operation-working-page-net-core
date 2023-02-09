namespace OWP.Maxbet.Services;

using Builder;
using Constants;
using Enums;
using Handlers;
using Helpers;
using MediatR;
using Models;

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

        var cn88Maxbet = new MaxbetSpread
        {
            Handicap = 0,
            OverUnder = 0
        };

        var maxbetInfo = new MaxbetInfo
        {
            AdminsiteID = adminsiteId,
            Traders = traderAssignmentsSender,
            Spread = spread,
            Maxbet = maxbet,
            MaxbetCN88 = cn88Maxbet,
            NumberOfLines = new NumberOfLines(),
            SecondAcceptValue = secondAcceptValue
        };

        MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

        return maxbetInfo;
    }
}