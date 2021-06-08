namespace OWP.PreMatchNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using OWP.PreMatchNotification.Criteria;
    using OWP.PreMatchNotification.Models;

    public interface Repository
    {
        Task<IEnumerable<InfoSiteMatch>> GetNewSnapshot(DateTime fromDate, DateTime toDate);
    }

    public class RepositoryImp : Repository
    {
        private readonly DynamicRepository dynamicRepository;

        public RepositoryImp(DynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<InfoSiteMatch>> GetNewSnapshot(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetInfoSiteMatchesByDateRangeCriteria
            {
                FromDate = fromDate.Date.AddHours(12),
                ToDate = toDate.Date.AddDays(1).AddHours(11).AddMinutes(59)
            };

            return await dynamicRepository.FetchAsync<InfoSiteMatch>(criteria);
        }
    }
}