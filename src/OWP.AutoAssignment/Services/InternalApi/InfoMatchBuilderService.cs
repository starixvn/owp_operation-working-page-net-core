using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.InternalApi
{
    using Models.InternalApi;

    public interface InfoMatchBuilderService
    {
        Task<IList<InfoSiteMatchModel>> BuildInfoSiteMatch(DateTime eventDate);
    }
    public class InfoMatchBuilderServiceImp : InfoMatchBuilderService
    {
        private readonly Repository repository;
        public InfoMatchBuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }
        public async Task<IList<InfoSiteMatchModel>> BuildInfoSiteMatch(DateTime eventDate)
        {
            var infoAutoAssignmentResult = await repository.GetAutoAssignmentByDate(eventDate);

            var betTypeByIds = BuildBetTypeByIds(infoAutoAssignmentResult.ToList()).ToList();

            var infoSiteMatches = await repository.GetInfoSiteMatchByIds(betTypeByIds);

            return infoSiteMatches.ToList();
        }

        public IEnumerable<BetTypeById> BuildBetTypeByIds(IList<AutoAssignmentResultModel> autoAssignmentResult)
        {
            foreach (var element in autoAssignmentResult)
            {
                yield return new BetTypeById
                {
                    InfoSiteMatchId = element.InfoSiteMatchId,
                    BetType = element.Bettype
                };
            }
        }
    }
}
