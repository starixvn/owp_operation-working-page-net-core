using OWP.AutoAssignment.Models.PublicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface SchedulingMatchBuilderService
    {
        Task<IEnumerable<SchedulingMatch>> GetMatchesInfo(DateTime eventDate);
    }

    public class SchedulingMatchBuilderServiceImp: SchedulingMatchBuilderService
    {
        private readonly Repository repository;

        public SchedulingMatchBuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<SchedulingMatch>> GetMatchesInfo(DateTime eventDate)
        {
            var matchQueried = await repository.GetMatchByDate(eventDate);

            var realMatchIds = SelectMatchIds(matchQueried.ToList(), false);
            var sabaMatchIds = SelectMatchIds(matchQueried.ToList(), true);

          

            return new List<SchedulingMatch>();
        }

        internal IEnumerable<SchedulingMatch> BuildListSchedulingMatch(object mainMatchs, object matches)
        {
            throw new NotImplementedException();
        }

        private IList<int> SelectMatchIds(IList<MatchQueried> matchQuerieds, bool isSaba)
          => matchQuerieds
              .Where(m => m.IsSaba == isSaba)
              .Select(x => x.InfositeMatchId)
              .ToList();

    }
}