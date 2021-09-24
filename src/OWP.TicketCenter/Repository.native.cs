using AutoMapper;
using Fanex.Data.Repository;
using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.Utils;

namespace OWP.TicketCenter
{
    using Criteria;

    public interface Repository
    {
        Task<IList<EventOptionInfo>> GetEventOptionInfo(string keyword);

        Task<IList<TicketType>> GetTicketTypes();

        Task<int> BuildTicketId(int ticketType);

        Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request);

        Task AddNewTicketIssueOther(NewTicketIssueOtherRequest request);

        Task AddNewMessage(int ticketId, int userId, string message);

        Task AddNewRecipients(int ticketId, string recipients, int recipientType);
        Task AddNewTicketIssueLeague(NewTicketIssueLeagueRequest request);
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

        public async Task<IList<EventOptionInfo>> GetEventOptionInfo(string keyword)
        {
            var criteria = new GetEventCriteria()
            {
                Keyword = keyword
            };
            criteria.ThrowExceptionIfInvalid();
            return (await dynamicRepository.FetchAsync<EventOptionInfo>(new GetEventCriteria
            {
                Keyword = keyword
            })).ToList();
        }

        public async Task<IList<TicketType>> GetTicketTypes()
                   => (await dynamicRepository.FetchAsync<TicketType>(new GetTicketTypeCriteria()))
                       .ToList();

        public async Task<int> BuildTicketId(int ticketType)
            => (await dynamicRepository.FetchAsync<int>(new BuildNewTicketIdCriteria
            {
                TicketType = ticketType
            })).FirstOrDefault();


        public async Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request)
        {
            var criteria = mapper.Map<AddNewTicketIssueEventCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task AddNewTicketIssueOther(NewTicketIssueOtherRequest request)
        {
            var criteria = mapper.Map<AddNewTicketIssueOtherCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task AddNewMessage(int ticketId, int userId, string message)
        {
            var criteria = new AddNewTicketIssueMessageCriteria()
            {
                TicketId = ticketId,
                UserId = userId,
                Message = message
            };
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task AddNewRecipients(int ticketId, string recipients, int recipientType)
        {
            var criteria = new AddRecipientCriteria
            {
                TicketId = ticketId,
                RecipientIds = recipients,
                RecipientType = recipientType
            };
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task AddNewTicketIssueLeague(NewTicketIssueLeagueRequest request)
        {
            var criteria = mapper.Map<AddNewTicketIssueLeagueCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }
    }
}