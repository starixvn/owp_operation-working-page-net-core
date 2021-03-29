using MediatR;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Models;
using System.Threading.Tasks;

namespace OWP.Maxbet.Services
{
    public interface MaxbetService
    {
        public Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId);

        public Task<int> GetMatchIdByAdminSiteId(int adminsiteId);
    }

    public class MaxbetServiceImp : MaxbetService
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly TraderAssignmentBuilder traderAssignmentBuilder;

        private readonly SpreadBuilder spreadBuilder;

        private readonly MaxbetBuilder maxbetBuilder;

        private readonly IMediator mediator;

        public MaxbetServiceImp(
            MaxbetReposiory syncMaxbetReposiory,
            TraderAssignmentBuilder traderAssignmentBuilder,
            SpreadBuilder spreadBuilder,
            MaxbetBuilder maxbetBuilder,
            IMediator mediator)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
            this.traderAssignmentBuilder = traderAssignmentBuilder;
            this.spreadBuilder = spreadBuilder;
            this.maxbetBuilder = maxbetBuilder;
            this.mediator = mediator;
        }

        public async Task<int> GetMatchIdByAdminSiteId(int adminsiteId)
            => await syncMaxbetReposiory.GetMatchIdByAdminSiteId(adminsiteId);

        public async Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId)
        {
            var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);

            var spread = await spreadBuilder.BuildSpread(owpMatchId);

            var maxbet = await maxbetBuilder.BuildMaxbet(owpMatchId);

            var cn88Maxbet = new MaxbetSpread
            {
                FirstHalfHandicap = 0,
                FirstHalfOverUnder = 0,
                Handicap = 0,
                OverUnder = 0
            };

            var secondAcceptValue = await mediator.Send(new GetSecondAcceptValueHandler.Request { OWPMatchId = owpMatchId });

            var numberOfLines = await mediator.Send(new GetNumberOfLinesHandler.Request { OWPMatchId = owpMatchId });

            return new MaxbetInfo
            {
                AdminsiteID = adminsiteId,
                Traders = traderAssignmentsSender,
                Spread = spread,
                Maxbet = maxbet,
                MaxbetCN88 = cn88Maxbet,
                NumberOfLines = numberOfLines,
                SecondAcceptValue = secondAcceptValue
            };
        }
    }
}