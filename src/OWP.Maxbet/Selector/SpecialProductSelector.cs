using MediatR;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Maxbet.Selector
{
    public interface SpecialProductSelector
    {
        public Task<TraderAssignmentSender> SelectTrader(int specialProductId, int bettypeId);

        public Task<MaxbetSpread> SelectSpread(int specialProductId, int bettypeId);

        public Task<MaxbetSpread> SelectMaxbet(MinorMatchInfo specialProduct, int bettypeId);

        public Task<int?> SelectSecondAcceptedValues(int owpMatchId, int bettypeId);

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

        private const int BETTYPE_ID_SPECIFIC10MINS = -1;
        private const int BETTYPE_ID_SPECIFIC15MINS = -2;
        private const int BETTYPE_ID_SPECIFIC15MINS_NUMBER_OF_CORNERS = -5;
        private const int BETTYPE_ID_SPECIFIC15MINS_TOTAL_BOOKINGS = -8;

        private readonly int[] BettypeIdSpecificXMins = new[]{
            BETTYPE_ID_SPECIFIC10MINS,
            BETTYPE_ID_SPECIFIC15MINS,
            BETTYPE_ID_SPECIFIC15MINS_NUMBER_OF_CORNERS,
            BETTYPE_ID_SPECIFIC15MINS_TOTAL_BOOKINGS
         };

        private const int BETTYPE_ID_NO_OF_CORNERS = -3;
        private const int BETTYPE_ID_NEXT_CORNERS = -4;
        private const int BETTYPE_ID_TOTAL_BOOKINGS = -6;
        private const int BETTYPE_ID_NEXT_BOOKINGS = -7;

        private readonly int[] BettypeIdSpecialProducts = new[]{
            BETTYPE_ID_NO_OF_CORNERS,
            BETTYPE_ID_NEXT_CORNERS,
            BETTYPE_ID_TOTAL_BOOKINGS,
            BETTYPE_ID_NEXT_BOOKINGS
        };

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
        => BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? await specialProductTraderBuilder.BuildTraderXMins(specialProductId)
            : await traderAssignmentBuilder.BuildTraderAssignmentSender(specialProductId);

        public async Task<MaxbetSpread> SelectSpread(int specialProductId, int bettypeId)
        => BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? await specialProductSpreadBuilder.BuildSpreadXMins(specialProductId)
            : await spreadBuilder.BuildSpreadSpecialProduct(specialProductId);

        public async Task<MaxbetSpread> SelectMaxbet(MinorMatchInfo specialProduct, int bettypeId)
        {
            if (BettypeIdSpecificXMins.Any(id => id == bettypeId))
            {
                return await specialProductMaxbetBuilder.BuildMaxbetXMins(specialProduct.MinorMatchId);
            }
            else if (BettypeIdSpecialProducts.Any(id => id == bettypeId))
            {
                return await specialProductMaxbetBuilder.BuildMaxbetSpecialProduct(specialProduct, bettypeId);
            }

            return await specialProductMaxbetBuilder.BuildMaxbet(specialProduct.MinorMatchId);
        }

        public async Task<int?> SelectSecondAcceptedValues(int owpMatchId, int bettypeId)
            => BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? null
            : await mediator.Send(new GetSecondAcceptValueHandler.Request { OWPMatchId = owpMatchId });

        public async Task<NumberOfLines> SelectNumberOfLines(int owpMatchId, int bettypeId)
            => BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? null
            : await mediator.Send(new GetNumberOfLinesSpecialProductHandler.Request
            {
                OWPMatchId = owpMatchId
            });
    }
}