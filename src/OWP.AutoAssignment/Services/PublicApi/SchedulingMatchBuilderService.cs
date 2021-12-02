using OWP.AutoAssignment.Models.PublicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface SchedulingMatchBuilderService
    {
        Task<SchedulingMatch> GetMatchesInfo(DateTime eventDate);
    }

    public class SchedulingMatchBuilderServiceImp : SchedulingMatchBuilderService
    {
        private readonly Repository repository;

        private readonly SabaMatchService sabaMatchService;

        private readonly RealMatchService realMatchService;

        public SchedulingMatchBuilderServiceImp(
            Repository repository,
            SabaMatchService sabaMatchService,
            RealMatchService realMatchService
            )
        {
            this.repository = repository;
            this.realMatchService = realMatchService;
            this.sabaMatchService = sabaMatchService;
        }

        public async Task<SchedulingMatch> GetMatchesInfo(DateTime eventDate)
        {
            var schedulingMatch = new SchedulingMatch();
            var matchQueried = await repository.GetMatchByDate(eventDate);

            var realMatchIds = SelectMatchIds(matchQueried.ToList(), false);
            var sabaMatchIds = SelectMatchIds(matchQueried.ToList(), true);

            var sabaMatch = await sabaMatchService.BuildSabaMatchInfo(sabaMatchIds);
            var realmatch = await realMatchService.BuildRealMatchInfo(realMatchIds);

            schedulingMatch.Saba.AddRange(sabaMatch);
            schedulingMatch.Real.AddRange(realmatch);
            return schedulingMatch;
        }

        internal IEnumerable<SchedulingMatch> BuildListSchedulingMatch(object mainMatchs, object matches)
        {
            throw new NotImplementedException();
        }

        private IList<MatchQueried> SelectMatchIds(IList<MatchQueried> matchQuerieds, bool isSaba)
          => matchQuerieds
              .Where(m => m.IsSaba == isSaba)
              .ToList();
    }
}