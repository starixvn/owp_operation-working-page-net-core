using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter
{
    using AutoMapper;
    using Fanex.Data.Repository;
    using OWP.TicketCenter.Handlers;
    using OWP.TicketCenter.Models;
    using OWP.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;

    public interface Repository
    {
        Task<IList<EventOptionInfoModel>> GetEventOptionInfo(string keyword);

        Task<IList<TicketTypeModel>> GetTicketTypes();

        Task<int> BuildTicketId(int ticketType);

        Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request);

        Task AddNewTicketIssueOther(NewTicketIssueOtherRequest request);

        Task<MessageDetailViewModel> AddNewMessage(NewMessageRequestModel message);

        Task AddManyRecipients(int ticketId, string recipientIds, int recipientType);

        Task AddNewTicketIssueLeague(NewTicketIssueLeagueRequest request);

        Task<IEnumerable<EventTicketOtherModel>> GetEventTicketOther(QueryEventTicketsRequest request);

        Task<IEnumerable<EventTicketLeagueModel>> GetEventTicketLeague(QueryEventTicketsRequest request);

        Task<IEnumerable<EventTicketMatchModel>> GetEventTicketMatch(QueryEventTicketsRequest request);

        Task<IEnumerable<EventTicketMatchModel>> GetTicketMatchDetail(int ticketId);

        Task<IEnumerable<EventTicketLeagueModel>> GetTicketLeagueDetail(int ticketId);

        Task<IEnumerable<EventTicketOtherModel>> GetTicketOtherDetail(int ticketId);

        Task<IEnumerable<RecipientResponseModel>> GetRecipientByTicketId(int ticketId);

        Task<int> AddSingleRecipient(InsertSingleRecipientRequest request);

        Task<int> RemoveSingleRecipient(RemoveSingleRecipientRequest request);

        Task InsertIssueCreator(int ticketId, int creatorId);

        Task<IssueCreatorModel> GetIssueCreator(int ticketId);
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

        public async Task<IList<EventOptionInfoModel>> GetEventOptionInfo(string keyword)
        {
            var criteria = new GetEventCriteria()
            {
                Keyword = keyword
            };
            criteria.ThrowExceptionIfInvalid();
            return (await dynamicRepository.FetchAsync<EventOptionInfoModel>(new GetEventCriteria
            {
                Keyword = keyword
            })).ToList();
        }

        public async Task<IList<TicketTypeModel>> GetTicketTypes()
                   => (await dynamicRepository.FetchAsync<TicketTypeModel>(new GetTicketTypeCriteria()))
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

        public async Task AddNewTicketIssueLeague(NewTicketIssueLeagueRequest request)
        {
            var criteria = mapper.Map<AddNewTicketIssueLeagueCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task AddManyRecipients(int ticketId, string recipientIds, int recipientType)
        {
            var criteria = new AddManyRecipientCriteria
            {
                TicketId = ticketId,
                RecipientIds = recipientIds,
                RecipientType = recipientType
            };
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<int> AddSingleRecipient(InsertSingleRecipientRequest request)
        {
            var criteria = mapper.Map<AddSingleRecipientCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            return (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
        }

        public async Task<int> RemoveSingleRecipient(RemoveSingleRecipientRequest request)
        {
            var criteria = mapper.Map<RemoveSingleRecipientCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            return (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
        }

        public async Task<MessageDetailViewModel> AddNewMessage(NewMessageRequestModel message)
        {
            var criteria = new AddNewTicketIssueMessageCriteria()
            {
                TicketId = message.TicketId,
                UserId = message.CreatorId,
                FileId = message.FileId,
                Message = message.Message ?? "Please add"
            };
            criteria.ThrowExceptionIfInvalid();
            var result = (await dynamicRepository.FetchAsync<MessageDetailViewModel>(criteria)).FirstOrDefault();

            return result;
        }

        public async Task<IEnumerable<EventTicketMatchModel>> GetEventTicketMatch(QueryEventTicketsRequest request)
        {
            var criteria = mapper.Map<GetEventTicketMatchCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<EventTicketMatchModel>(criteria);
        }

        public async Task<IEnumerable<EventTicketOtherModel>> GetEventTicketOther(QueryEventTicketsRequest request)
        {
            var criteria = mapper.Map<GetEventTicketOtherCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<EventTicketOtherModel>(criteria);
        }

        public async Task<IEnumerable<EventTicketLeagueModel>> GetEventTicketLeague(QueryEventTicketsRequest request)
        {
            var criteria = mapper.Map<GetEventTicketLeagueCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<EventTicketLeagueModel>(criteria);
        }

        public async Task<IEnumerable<EventTicketMatchModel>> GetTicketMatchDetail(int ticketId)
        {
            var criteria = new TicketDetailMatchCriteria
            {
                TicketId = ticketId
            };
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<EventTicketMatchModel>(criteria);
        }

        public async Task<IEnumerable<EventTicketLeagueModel>> GetTicketLeagueDetail(int ticketId)
        {
            var criteria = new TicketDetailLeagueCriteria
            {
                TicketId = ticketId
            };
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<EventTicketLeagueModel>(criteria);
        }

        public async Task<IEnumerable<EventTicketOtherModel>> GetTicketOtherDetail(int ticketId)
        {
            var criteria = new TicketDetailOtherCriteria
            {
                TicketId = ticketId
            };
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<EventTicketOtherModel>(criteria);
        }


        public async Task<IEnumerable<RecipientResponseModel>> GetRecipientByTicketId(int ticketId)
        {
            var criteria = new GetRecipientByTicketIdCriteria
            {
                TicketId = ticketId
            };
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<RecipientResponseModel>(criteria);
        }

        public async Task InsertIssueCreator(int ticketId, int creatorId)
        {
            var creator = new InsertIssueCreatorCriteria()
            {
                TicketId = ticketId,
                CreatorId = creatorId
            };
            await dynamicRepository.ExecuteAsync(creator);
        }

        public async Task<IssueCreatorModel> GetIssueCreator(int ticketId)
        {
            var creator = new QueryIssueCreatorCriteria()
            {
                TicketId = ticketId
            };
            return (await dynamicRepository.FetchAsync<IssueCreatorModel>(creator))?.FirstOrDefault();
        }
    }
}