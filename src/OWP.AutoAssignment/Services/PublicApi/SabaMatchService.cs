

namespace OWP.AutoAssignment.Services.PublicApi
{
    using Constant;
    using OWP.AutoAssignment.Models.PublicApi;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface SabaMatchService
    {
        Task<IEnumerable<SabaMaxbetInfo>> GetSabaMaxbet(IList<int> matchIds);

        IEnumerable<SabaMatchSender> BuildSabaMatch(IEnumerable<SabaMatchCommonInfo> matchInfos,
            List<MatchQueried> matchsQueried);
    }

    public class SabaMatchServiceImp : SabaMatchService
    {
        private readonly Repository repository;

        public SabaMatchServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<SabaMatchSender> BuildSabaMatch(IEnumerable<SabaMatchCommonInfo> matchInfos, List<MatchQueried> matchsQueried)
            => matchInfos.Select(i => new SabaMatchSender()
            {
                MatchId = i.InfositeId,
                Location = i.Location,
                KOTime = i.KOTime.ToString(AutoConstant.FormatDateTime),
                IsSaba = true,
                Group = "FT",
                SabaType = GetSabaType(matchsQueried, i.InfositeId),
            }).ToList();

        private static int GetSabaType(List<MatchQueried> matchsQueried, int infositeId)
            => matchsQueried.Where(i => i.InfositeMatchId == infositeId)
                .Select(x => x.SabaType)
                .FirstOrDefault();

        public async Task<IEnumerable<SabaMaxbetInfo>> GetSabaMaxbet(IList<int> matchIds)
            => await repository.GetSabaMaxbet(matchIds);
    }
}
