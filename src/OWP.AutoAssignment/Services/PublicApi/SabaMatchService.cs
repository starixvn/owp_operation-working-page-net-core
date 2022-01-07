namespace OWP.AutoAssignment.Services.PublicApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Constant;
    using OWP.AutoAssignment.Models.PublicApi;

    public interface SabaMatchService
    {
        Task<IList<SabaMatchSender>> BuildSabaMatchInfo(IList<MatchQueried> matchsQueried);
    }

    public class SabaMatchServiceImp : SabaMatchService
    {
        private readonly Repository repository;

        public SabaMatchServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<SabaMatchSender>> BuildSabaMatchInfo(IList<MatchQueried> matchsQueried)
        {
            var matchIds = matchsQueried.Select(i => i.InfositeMatchId).ToList();
            var matchInfos = await GetSabaMatchInfo(matchIds);
            var maxBetInfos = (await GetSabaMaxbet(matchIds)).ToList();

            return matchInfos.Select(i => new SabaMatchSender
            {
                MatchId = i.InfositeId,
                KOTime = i.KOTime.ToString(AutoConstant.FormatDateTime),
                SabaType = GetSabaType(matchsQueried, i.InfositeId),
                LeagueSettingId = GetMaxBetValue(maxBetInfos, i.InfositeId)?.LeagueSettingId,
                MaxBet = GetMaxBetValue(maxBetInfos, i.InfositeId)?.MaxBetFTHDP,
                AwayId = i.AwayId,
                HomeId = i.HomeId
            }).ToList();
        }

        private static int? GetSabaType(IList<MatchQueried> matchsQueried, int infositeId)
            => matchsQueried
            .FirstOrDefault(i => i.InfositeMatchId == infositeId)?
            .SabaType;

        private MaxBetOfFTHDP GetMaxBetValue(List<MaxBetOfFTHDP> maxbetInfos, int infositeId)
            => maxbetInfos
                .FirstOrDefault(m => m.InfoSiteMatchId == infositeId);

        public async Task<IEnumerable<MaxBetOfFTHDP>> GetSabaMaxbet(IList<int> matchIds)
            => await repository.GetMaxMaxbetFTHDP(matchIds);

        public async Task<IEnumerable<SabaMatchCommonInfo>> GetSabaMatchInfo(IList<int> matchIds)
            => await repository.GetSabaMatchCommonInfoByIds(matchIds);
    }
}
