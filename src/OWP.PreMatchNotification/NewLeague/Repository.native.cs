namespace OWP.PreMatchNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using OWP.PreMatchNotification.NewLeague.Criteria;
    using OWP.PreMatchNotification.NewLeague.Models;

    public interface Repository
    {
        Task<IList<PreMatchLeague>> GetPreMatchLeagueNewSnapshot(DateTime fromDate, DateTime toDate);
    }

    public class RepositoryImp : Repository
    {
        private readonly DynamicRepository dynamicRepository;

        public RepositoryImp(DynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IList<PreMatchLeague>> GetPreMatchLeagueNewSnapshot(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetPreMatchLeaguesCriteria
            {
                FromDate = fromDate,
                ToDate = toDate
            };
            var result = await dynamicRepository.FetchAsync<PreMatchLeague>(criteria).ConfigureAwait(false);

            return result.ToList();
        }
    }
}