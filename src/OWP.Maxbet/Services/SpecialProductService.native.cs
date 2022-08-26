using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;
using OWP.Maxbet.Selector;
using OWP.Services;

namespace OWP.Maxbet.Services
{
    public interface SpecialProductService
    {
        Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId);

        Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId);

        Task<MaxbetInfo> GetMaxbetInfoET(int adminsiteId, int bettypeId);
    }

    public class SpecialProductServiceImp : SpecialProductService
    {
        private readonly MaxbetRepository syncMaxbetRepository;

        private readonly SpecialProductSelector specialProductSelector;

        private readonly LogServiceWrapper logServiceWrapper;

        public SpecialProductServiceImp(
            MaxbetRepository syncMaxbetRepository,
            SpecialProductSelector specialProductSelector,
            LogServiceWrapper logServiceWrapper)
        {
            this.syncMaxbetRepository = syncMaxbetRepository;
            this.specialProductSelector = specialProductSelector;
            this.logServiceWrapper = logServiceWrapper;
        }

        public async Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId)
                => await syncMaxbetRepository.GetMatchIdByAdminSiteId(adminsiteId, bettypeId);

        public async Task<MaxbetInfo> GetMaxbetInfoET(int adminsiteId, int bettypeId)
        {
            var minorMatch = await GetMatchIdByAdminSiteId(adminsiteId, bettypeId);
            if (minorMatch is null)
            {
                return null;
            }
            var maxbetInfo = await GetSpecialProductInfoExcludeNumberOfLines(minorMatch, adminsiteId, bettypeId);
            maxbetInfo.NumberOfLines = NumberOfLineHelper.UpdateNumberOfLine();
            maxbetInfo.Ft1stLineHandicap = Ft1stLineHandicapHelper.BuildFt1stLineHandicap();
            return maxbetInfo;
        }

        public async Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId)
        {
            var maxbetInfo = await GetSpecialProductInfoExcludeNumberOfLines(owpMatch, adminsiteId, bettypeId);
            var numberOfLines = await specialProductSelector.SelectNumberOfLines(owpMatch.MinorMatchId, bettypeId);

            maxbetInfo.NumberOfLines = numberOfLines;

            await logServiceWrapper.Log("=====GetSpecialProductInfo request======", owpMatch, adminsiteId, bettypeId);
            await logServiceWrapper.Log("=====GetSpecialProductInfo======",
            maxbetInfo.Traders,
            maxbetInfo.Spread,
            maxbetInfo.Maxbet,
            maxbetInfo.SecondAcceptValue,
            maxbetInfo.NumberOfLines);

            var maxbetCN88 = new MaxbetSpread
            {
                FirstHalfHandicap = 0,
                FirstHalfOverUnder = 0,
                Handicap = 0,
                OverUnder = 0
            };

            return maxbetInfo;
        }

        private async Task<MaxbetInfo> GetSpecialProductInfoExcludeNumberOfLines(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId)
        {
            var traders = await specialProductSelector.SelectTrader(owpMatch.MinorMatchId, bettypeId);
            var spread = await specialProductSelector.SelectSpread(owpMatch.MinorMatchId, bettypeId);
            var maxbet = await specialProductSelector.SelectMaxbet(owpMatch, bettypeId);
            var secondAcceptValue = await specialProductSelector.SelectSecondAcceptedValues(owpMatch.MinorMatchId, bettypeId);

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
                SecondAcceptValue = secondAcceptValue
            };
        }
    }
}