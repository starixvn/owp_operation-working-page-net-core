namespace OWP.AutoAssignment.Services.PublicApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Constant;
    using OWP.AutoAssignment.Models.PublicApi;

    public interface SabaMatchService
    {
        Task<IEnumerable<SabaMaxbetInfo>> GetSabaMaxbet(IList<int> matchIds);

        Task<IEnumerable<SabaMatchSender>> BuildSabaMatch(List<MatchQueried> matchsQueried);
    }

    public class SabaMatchServiceImp : SabaMatchService
    {
        private readonly Repository repository;

        public SabaMatchServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<SabaMatchSender>> BuildSabaMatch(List<MatchQueried> matchsQueried)
        {
            var matchIds = matchsQueried.Select(i => i.InfositeMatchId).ToList();
            var matchInfos = await GetSabaMatchInfo(matchIds);
            var maxbetInfos = await GetSabaMaxbet(matchIds);

            return matchInfos.Select(i => new SabaMatchSender()
            {
                MatchId = i.InfositeId,
                Location = i.Location,
                KOTime = i.KOTime.ToString(AutoConstant.FormatDateTime),
                SabaType = GetSabaType(matchsQueried, i.InfositeId),
                MaxBetMainMatch = GetMaxbetValue(maxbetInfos.ToList(), i.InfositeId)
            }).ToList();
        }

        private static int GetSabaType(List<MatchQueried> matchsQueried, int infositeId)
            => matchsQueried.Where(i => i.InfositeMatchId == infositeId)
                .Select(x => x.SabaType)
                .FirstOrDefault();

        private decimal? GetMaxbetValue(List<SabaMaxbetInfo> maxbetInfos, int infositeId)
            => maxbetInfos.Where(m => m.MatchId == infositeId)
                .Select(x => x.Maxbet)
                .FirstOrDefault();

        public async Task<IEnumerable<SabaMaxbetInfo>> GetSabaMaxbet(IList<int> matchIds)
            => await repository.GetSabaMaxbet(matchIds);

        public async Task<IEnumerable<SabaMatchCommonInfo>> GetSabaMatchInfo(IList<int> matchIds)
            => await repository.GetSabaMatchCommonInfoByIds(matchIds);
    }
}