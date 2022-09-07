namespace OWP.Maxbet.Services;

using Builder;
using Constants;
using Handlers;
using MediatR;
using Models;

public interface ServiceCommon
{
    Task<MaxbetInfo> BuildMatchInfoExcludeNumberOfLines(int owpMatchId, int adminsiteId);
}

public class ServiceCommonImp : ServiceCommon
{
    private readonly MaxbetRepository syncMaxbetRepository;
    private readonly TraderAssignmentBuilder traderAssignmentBuilder;

    private readonly SpreadBuilder spreadBuilder;

    private readonly MaxbetBuilder maxbetBuilder;

    private readonly IMediator mediator;

    public ServiceCommonImp(MaxbetRepository syncMaxbetRepository,
    TraderAssignmentBuilder traderAssignmentBuilder,
        SpreadBuilder spreadBuilder,
        MaxbetBuilder maxbetBuilder,
        IMediator mediator)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
        this.traderAssignmentBuilder = traderAssignmentBuilder;
        this.spreadBuilder = spreadBuilder;
        this.maxbetBuilder = maxbetBuilder;
        this.mediator = mediator;
    }

    public async Task<MaxbetInfo> BuildMatchInfoExcludeNumberOfLines(int owpMatchId, int adminsiteId)
    {
        var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);

        var spread = await spreadBuilder.BuildSpreadMainMatch(owpMatchId);
        var maxbet = await maxbetBuilder.BuildMaxbet(owpMatchId);


        var secondAcceptValue = await mediator.Send(new GetSecondAcceptValueHandler.Request()
        { OWPMatchId = owpMatchId, MaxbetHdp = maxbet.Handicap, Bettype = SpecialBettype.BETTYPE_ID_MAIN_MATCH });

        return new MaxbetInfo
        {
            AdminsiteID = adminsiteId,
            Traders = traderAssignmentsSender,
            Spread = spread,
            Maxbet = maxbet,
            SecondAcceptValue = secondAcceptValue
        };
    }
}