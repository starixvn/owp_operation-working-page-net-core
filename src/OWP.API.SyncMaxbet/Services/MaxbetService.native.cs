using System.Threading.Tasks;
using MediatR;
using OWP.API.SyncMaxbet.Builder;
using OWP.API.SyncMaxbet.Handlers;
using OWP.API.SyncMaxbet.Models;

namespace OWP.API.SyncMaxbet.Services
{
    public interface MaxbetService
    {
        public Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId);

        public Task<int> GetMatchIdByAdminSiteId(int adminsiteId);
    }

    public class MaxbetServiceImp : MaxbetService
    {
        private readonly SyncMaxbetReposiory syncMaxbetReposiory;

        private readonly TraderAssignmentBuilder traderAssignmentBuilder;

        private readonly SpreadBuilder spreadBuilder;

        private readonly MaxbetBuilder maxbetBuilder;

        private readonly IMediator mediator;

        public MaxbetServiceImp(
            SyncMaxbetReposiory syncMaxbetReposiory,
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

            var cn88Maxbet = await mediator.Send(new GetMaxbetCN88Handler.Request { OWPMatchId = owpMatchId });

            var secondAcceptValue = await mediator.Send(new GetSecondAcceptValueHandler.Request { OWPMatchId = owpMatchId });

            var numberOfLines = await mediator.Send(new GetNumberOfLinesHandler.Request { OWPMatchId = owpMatchId });

            return new MaxbetInfo
            {
                MatchId = adminsiteId,
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