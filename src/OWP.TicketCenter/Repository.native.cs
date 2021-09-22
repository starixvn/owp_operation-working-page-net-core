using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter
{
    public interface Repository
    {
        Task<IList<EventInfo>> GetEventInfo(string keyword);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository dynamicRepository;

        public RepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IList<EventInfo>> GetEventInfo(string keyword)
            => (await dynamicRepository.FetchAsync<EventInfo>(new GetEventCriteria
            {
                Keyword = keyword
            })).ToList();
    }
}
