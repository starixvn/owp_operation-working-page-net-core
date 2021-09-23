using Fanex.Data.Repository;
using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.TicketCenter
{
    using Criteria;

    public interface Repository
    {
        Task<IList<EventInfo>> GetEventInfo(string keyword);

        Task<IList<TicketType>> GetTicketTypes();
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

        public async Task<IList<TicketType>> GetTicketTypes()
            => (await dynamicRepository.FetchAsync<TicketType>(new GetTicketTypeCriteria()))
                .ToList();
    }
}