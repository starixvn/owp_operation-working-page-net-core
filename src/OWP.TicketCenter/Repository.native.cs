using AutoMapper;
using Fanex.Data.Repository;
using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.TicketCenter
{
    using Criteria;

    public interface Repository
    {
        Task<IList<EventoptionInfo>> GetEventOptionInfo(string keyword);

        Task<IList<TicketType>> GetTicketTypes();
        Task<int> BuildTicketId(int ticketType);

        Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly IMapper mapper;

        public RepositoryImp(IDynamicRepository dynamicRepository, IMapper mapper)
        {
            this.dynamicRepository = dynamicRepository;
            this.mapper = mapper;
        }

        public async Task<IList<EventoptionInfo>> GetEventOptionInfo(string keyword)
            => (await dynamicRepository.FetchAsync<EventoptionInfo>(new GetEventCriteria
            {
                Keyword = keyword
            })).ToList();
            

        public async Task<IList<TicketType>> GetTicketTypes()
            => (await dynamicRepository.FetchAsync<TicketType>(new GetTicketTypeCriteria()))
                .ToList();
                TicketType = ticketType
            })).FirstOrDefault();
            
 public async Task<IList<TicketType>> GetTicketTypes()
            => (await dynamicRepository.FetchAsync<TicketType>(new GetTicketTypeCriteria()))
                .ToList();

            
        public async Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request)
        {
            var criteria = mapper.Map<AddNewTicketIssueEventCriteria>(request);
            await dynamicRepository.ExecuteAsync(criteria);
        }
    }
}