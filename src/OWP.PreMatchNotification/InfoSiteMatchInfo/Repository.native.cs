namespace OWP.PreMatchNotification.InfoSiteMatchInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Criteria;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;

    public interface Repository
    {
        Task<IList<InfoSiteMatch>> GetInfoSiteMatchNewSnapshot(DateTime fromDate, DateTime toDate);
    }

    public class RepositoryImp : Repository
    {
        private readonly DynamicRepository dynamicRepository;

        public RepositoryImp(DynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IList<InfoSiteMatch>> GetInfoSiteMatchNewSnapshot(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetInfoSiteMatchesByDateRangeCriteria
            {
                FromDate = fromDate.Date.AddHours(12),
                ToDate = toDate.Date.AddDays(1).AddHours(11).AddMinutes(59)
            };
            var result = await dynamicRepository.FetchAsync<InfoSiteMatch>(criteria);

            return result.ToList();
        }
    }
}