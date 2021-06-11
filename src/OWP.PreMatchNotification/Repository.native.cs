namespace OWP.PreMatchNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using OWP.PreMatchNotification.Criteria;
    using OWP.PreMatchNotification.Models;

    public interface Repository
    {
        Task<IList<InfoSiteMatch>> GetInfoSiteMatchNewSnapshot(DateTime fromDate, DateTime toDate);

        Task<IList<PreMatchLeague>> GetPreMatchLeagueNewSnapshot(DateTime fromDate, DateTime toDate);
        
        Task<IEnumerable<InfoSiteMatch>> GetNewOWPMatchFromDB(DateTime eventDate);
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
        
        public async Task<IEnumerable<InfoSiteMatch>> GetNewOWPMatchFromDB(DateTime eventDate)
        {
            //var criteria = new GetInfoSiteMatchesByDateRangeCriteria
            //{
            //    FromDate = fromDate.Date.AddHours(12),
            //    ToDate = toDate.Date.AddDays(1).AddHours(11).AddMinutes(59)
            //};

            //return await dynamicRepository.FetchAsync<InfoSiteMatch>(criteria);
            return new List<InfoSiteMatch>();
        }
    }
}