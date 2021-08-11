using OWP.Maxbet.Builder;
using OWP.Maxbet.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Maxbet.Selector
{
    public interface SpecialProductSelector
    {
        public Task<TraderAssignmentSender> SelectTrader(int specialProductId, int bettypeId);

        public Task<MaxbetSpread> SelectSpread(int specialProductId, int bettypeId);

        public Task<MaxbetSpread> SelectMaxbet(int specialProductId, int bettypeId);
    }

    public class SpecialProductSelectorImp : SpecialProductSelector
    {
        private readonly TraderAssignmentBuilder traderAssignmentBuilder;
        private readonly SpecialProductTraderBuilder specialProductTraderBuilder;
        private readonly SpreadBuilder spreadBuilder;
        private readonly SpecialProductSpreadBuilder specialProductSpreadBuilder;
        private readonly SpecialProductMaxbetBuilder specialProductMaxbetBuilder;

        private const int BETTYPE_ID_SPECIFIC10MINS = -1;
        private const int BETTYPE_ID_SPECIFIC15MINS = -2;
        private const int BETTYPE_ID_SPECIFIC15MINS_NUMBER_OF_CORNERS = -5;
        private const int BETTYPE_ID_SPECIFIC15MINS_TOTAL_BOOKINGS = -8;

        private readonly int[] BettypeIdSpecificXMins = new int[4]{
            BETTYPE_ID_SPECIFIC10MINS,
            BETTYPE_ID_SPECIFIC15MINS,
            BETTYPE_ID_SPECIFIC15MINS_NUMBER_OF_CORNERS,
            BETTYPE_ID_SPECIFIC15MINS_TOTAL_BOOKINGS
         };

        public SpecialProductSelectorImp(
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
        }

        public async Task<TraderAssignmentSender> SelectTrader(int specialProductId, int bettypeId)
        => BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? await specialProductTraderBuilder.BuildTraderXMins(specialProductId)
            : await traderAssignmentBuilder.BuildTraderAssignmentSender(specialProductId);

        public async Task<MaxbetSpread> SelectSpread(int specialProductId, int bettypeId)
        => BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? await specialProductSpreadBuilder.BuildSpreadXMins(specialProductId)
            : await spreadBuilder.BuildSpread(specialProductId);

        public async Task<MaxbetSpread> SelectMaxbet(int specialProductId, int bettypeId)
        => BettypeIdSpecificXMins.Any(id => id == bettypeId)
            ? await specialProductMaxbetBuilder.BuildMaxbetXMins(specialProductId)
            : await specialProductMaxbetBuilder.BuildMaxbet(specialProductId);

    }
}
