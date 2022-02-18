using MediatR;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Models;
using OWP.Maxbet.Selector;
using OWP.Services;


namespace OWP.Maxbet.Services
{
    public interface MaxbetService
    {
        public Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId);

        public Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId);

        public Task<int> GetMatchIdByAdminSiteId(int adminsiteId);

        public Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId);
    }

    public class MaxbetServiceImp : MaxbetService
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly TraderAssignmentBuilder traderAssignmentBuilder;

        private readonly SpreadBuilder spreadBuilder;

        private readonly MaxbetBuilder maxbetBuilder;

        private readonly IMediator mediator;

        private readonly SpecialProductSelector specialProductSelector;

        private readonly LogServiceWrapper logServiceWrapper;

        public MaxbetServiceImp(
            MaxbetReposiory syncMaxbetReposiory,
            TraderAssignmentBuilder traderAssignmentBuilder,
            SpreadBuilder spreadBuilder,
            MaxbetBuilder maxbetBuilder,
            IMediator mediator,
            SpecialProductSelector specialProductSelector,
            LogServiceWrapper logServiceWrapper)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
            this.traderAssignmentBuilder = traderAssignmentBuilder;
            this.spreadBuilder = spreadBuilder;
            this.maxbetBuilder = maxbetBuilder;
            this.mediator = mediator;
            this.specialProductSelector = specialProductSelector;
            this.logServiceWrapper = logServiceWrapper;
        }

        public async Task<int> GetMatchIdByAdminSiteId(int adminsiteId)
            => await syncMaxbetReposiory.GetMatchIdByAdminSiteId(adminsiteId);

        public async Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId)
            => await syncMaxbetReposiory.GetMatchIdByAdminSiteId(adminsiteId, bettypeId);

        public async Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId)
        {
            var traders = await specialProductSelector.SelectTrader(owpMatch.MinorMatchId, bettypeId);
            var spread = await specialProductSelector.SelectSpread(owpMatch.MinorMatchId, bettypeId);
            var maxbet = await specialProductSelector.SelectMaxbet(owpMatch, bettypeId);
            var secondAcceptValue = await specialProductSelector.SelectSecondAcceptedValues(owpMatch.MinorMatchId, bettypeId);
            var numberOfLines = await specialProductSelector.SelectNumberOfLines(owpMatch.MinorMatchId, bettypeId);

            await logServiceWrapper.Log("=====GetSpecialProductInfo request======", owpMatch, adminsiteId, bettypeId);
            await logServiceWrapper.Log("=====GetSpecialProductInfo======", traders, spread, maxbet, secondAcceptValue, numberOfLines);

            var maxbetCN88 = new MaxbetSpread
            {
                FirstHalfHandicap = 0,
                FirstHalfOverUnder = 0,
                Handicap = 0,
                OverUnder = 0
            };

            return new MaxbetInfo
            {
                AdminsiteID = adminsiteId,
                Traders = traders,
                Spread = spread,
                Maxbet = maxbet,
                MaxbetCN88 = maxbetCN88,
                SecondAcceptValue = secondAcceptValue,
                NumberOfLines = numberOfLines
            };
        }

        public async Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId)
        {
            var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);

            var spread = await spreadBuilder.BuildSpreadMainMatch(owpMatchId);

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