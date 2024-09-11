using MediatR;
using OWP.Api.Maxbet.Builder;
using OWP.Api.Maxbet.Constants;
using OWP.Api.Maxbet.Handlers;
using OWP.Api.Maxbet.Models;

namespace OWP.Api.Maxbet.Selector;

public interface SpecialProductSelector
{
    public Task<TraderAssignmentSender> SelectTrader(int specialProductId, int bettypeId);

    public Task<MaxbetSpread> SelectSpread(int specialProductId, int bettypeId);

    public Task<MaxbetSpread> SelectMaxbet(MinorMatchInfo specialProduct, int bettypeId);

    public Task<int?> SelectSecondAcceptedValues(int owpMatchId, int bettypeId, decimal? maxbetMainMatch);

    public Task<NumberOfLines> SelectNumberOfLines(int owpMatchId, int bettypeId);
}

public class SpecialProductSelectorImp : SpecialProductSelector
{
    private readonly TraderAssignmentBuilder traderAssignmentBuilder;
    private readonly SpecialProductTraderBuilder specialProductTraderBuilder;
    private readonly SpreadBuilder spreadBuilder;
    private readonly SpecialProductSpreadBuilder specialProductSpreadBuilder;
    private readonly SpecialProductMaxbetBuilder specialProductMaxbetBuilder;
    private readonly IMediator mediator;

    public SpecialProductSelectorImp(
        IMediator mediator,
        TraderAssignmentBuilder traderAssignmentBuilder,
        SpecialProductTraderBuilder specialProductTraderBuilder,
        SpreadBuilder spreadBuilder,
        SpecialProductSpreadBuilder specialProductSpreadBuilder,
        SpecialProductMaxbetBuilder specialProductMaxbetBuilder
    )
    {
        this.traderAssignmentBuilder = traderAssignmentBuilder;
        this.specialProductTraderBuilder = specialProductTraderBuilder;
        this.spreadBuilder = spreadBuilder;
        this.specialProductSpreadBuilder = specialProductSpreadBuilder;
        this.specialProductMaxbetBuilder = specialProductMaxbetBuilder;
        this.mediator = mediator;
    }

    public async Task<TraderAssignmentSender> SelectTrader(int specialProductId, int bettypeId)
        => SpecialBettype.BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? await specialProductTraderBuilder.BuildTraderXMins(specialProductId)
            : await traderAssignmentBuilder.BuildTraderAssignmentSender(specialProductId);

    public async Task<MaxbetSpread> SelectSpread(int specialProductId, int bettypeId)
        => SpecialBettype.BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? await specialProductSpreadBuilder.BuildSpreadXMins(specialProductId)
            : await spreadBuilder.BuildSpreadSpecialProduct(specialProductId);

    public async Task<MaxbetSpread> SelectMaxbet(MinorMatchInfo specialProduct, int bettypeId)
    {
        if (SpecialBettype.BettypeIdSpecificXMins.Any(id => id == bettypeId))
        {
            return await specialProductMaxbetBuilder.BuildMaxbetXMins(specialProduct.MinorMatchId);
        }

        if (SpecialBettype.BettypeIdSpecialProducts.Any(id => id == bettypeId))
        {
            return await specialProductMaxbetBuilder.BuildMaxbetSpecialProduct(specialProduct, bettypeId);
        }

        return await specialProductMaxbetBuilder.BuildMaxbet(specialProduct.MinorMatchId);
    }

    public async Task<int?> SelectSecondAcceptedValues(int owpMatchId, int bettypeId, decimal? maxbetMainMatch)
    {
        if (SpecialBettype.BettypeIdSpecificXMins.Any(id => id == bettypeId))
        {
            return null;
        }

        return await mediator.Send(new GetSecondAcceptValueHandler.Request { OWPMatchId = owpMatchId, Bettype = bettypeId, MaxbetHdp = maxbetMainMatch });
    }

    public async Task<NumberOfLines> SelectNumberOfLines(int owpMatchId, int bettypeId)
        => SpecialBettype.BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? null
            : await mediator.Send(new GetNumberOfLinesSpecialProductHandler.Request
            {
                OWPMatchId = owpMatchId
            });
}