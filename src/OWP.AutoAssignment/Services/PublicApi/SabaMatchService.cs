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
            var maxbetInfos = await GetSabaMaxbet(matchIds);

            return matchInfos.Select(i => new SabaMatchSender()
            {
                MatchId = i.InfositeId,
                Location = i.Location,
                KOTime = i.KOTime.ToString(AutoConstant.FormatDateTime),
                SabaType = GetSabaType(matchsQueried, i.InfositeId),
                MaxBet = GetMaxbetValue(maxbetInfos.ToList(), i.InfositeId)
            }).ToList();
        }

        private static int? GetSabaType(IList<MatchQueried> matchsQueried, int infositeId)
            => matchsQueried
            .FirstOrDefault(i => i.InfositeMatchId == infositeId)?
            .SabaType;

        private decimal? GetMaxbetValue(List<SabaMaxbetInfo> maxbetInfos, int infositeId)
            => maxbetInfos
            .FirstOrDefault(m => m.MatchId == infositeId)?
            .Maxbet;

        public async Task<IEnumerable<SabaMaxbetInfo>> GetSabaMaxbet(IList<int> matchIds)
            => await repository.GetSabaMaxbet(matchIds);

        public async Task<IEnumerable<SabaMatchCommonInfo>> GetSabaMatchInfo(IList<int> matchIds)
            => await repository.GetSabaMatchCommonInfoByIds(matchIds);
    }
}