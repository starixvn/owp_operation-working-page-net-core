namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Criteria;
    using Models;
    using Fanex.NAP.Client.Helpers.Web;

    public interface Repository
    {
        Task<List<GeneralMatch>> GetLGAMatchNewSnapshot(DateTime eventDate);

        Task<List<PrematchLeague>> GetInfositePrematch(DateTime eventDate);
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

        public async Task<List<PrematchLeague>> GetInfositePrematch(DateTime eventDate)
        {
            var criteria = new GetInfositePrematchCriteria
            {
                EventToDateTime = eventDate.AddDays(1).AddHours(11).AddMinutes(59).AddSeconds(59),
                EventFromDateTime = eventDate.AddHours(12)
            };

            return (await dynamicRepository.FetchAsync<PrematchLeague>(criteria)).ToList();
        }
    }
}
