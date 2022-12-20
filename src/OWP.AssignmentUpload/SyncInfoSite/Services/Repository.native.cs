namespace OWP.AssignmentUpload.SyncInfoSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Models;
    using InfositeIdSync.Criteria;

    public interface SyncInfoSiteRepository
    {
        Task<List<InfositeMatch>> QueryInfositeMatch(DateTime eventDate);
    }

    public class SyncInfoSiteRepositoryImp : SyncInfoSiteRepository
    {
        private readonly IDynamicRepository repository;

        public SyncInfoSiteRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<InfositeMatch>> QueryInfositeMatch(DateTime eventDate)
        {
            var criteria = new QueryInfositeMatchCriteria
            {
                EventDate = eventDate
            };

            var matches = await repository.FetchAsync<InfositeMatch>(criteria);
            return matches.ToList();
        }
    }
}
