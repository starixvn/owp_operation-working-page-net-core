namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Criteria;
    using Models;

    public interface Repository
    {
        Task<List<GeneralMatch>> GetLGAMatchNewSnapshot(DateTime eventDate);
    }

    public class RepositoryImp : Repository
    {
        private readonly DynamicRepository dynamicRepository;

        public RepositoryImp(DynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<List<GeneralMatch>> GetLGAMatchNewSnapshot(DateTime eventDate)
        {
            var criteria = new GetLGAMatchesByTimeRangeCriteria
            {
                EventDate = eventDate
            };
            var result = await dynamicRepository.FetchAsync<GeneralMatch>(criteria);

            return result.ToList();
        }
    }
}
