using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Criteria;
    using Fanex.Data.Repository;
    using OWP.TicketCenter.Handlers;
    using OWP.TicketCenter.Models;
    using OWP.Utils;

    public interface Repository
    {
        Task<IList<EventOptionInfoModel>> GetEventOptionInfo(string keyword);

        Task<IList<TicketTypeModel>> GetTicketTypes();

        Task<int> BuildTicketId(int ticketType);

        Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request);

        Task AddNewTicketIssueOther(NewTicketIssueOtherRequest request);

        Task<MessageDetailViewModel> AddNewMessage(NewMessageRequestModel message);

        Task AddManyRecipients(AddManyRecipientCriteria criteria);

        Task AddNewTicketIssueLeague(NewTicketIssueLeagueRequest request);

        Task<IEnumerable<EventTicketOtherModel>> GetEventTicketOther(QueryEventTicketsRequest request);

        Task<IEnumerable<EventTicketLeagueModel>> GetEventTicketLeague(QueryEventTicketsRequest request);

        Task<IEnumerable<EventTicketMatchModel>> GetEventTicketMatch(QueryEventTicketsRequest request);

        Task<IEnumerable<EventTicketMatchModel>> GetTicketMatchDetail(int ticketId);

        Task<IEnumerable<EventTicketLeagueModel>> GetTicketLeagueDetail(int ticketId);

        Task<IEnumerable<EventTicketOtherModel>> GetTicketOtherDetail(int ticketId);

        Task<IEnumerable<RecipientResponseModel>> GetRecipientByTicketId(int ticketId);

        Task<IEnumerable<int>> GetUserIdsByTicketId(int ticketId);

        Task<IEnumerable<int>> GetUserIdsIgnoredCreatorId(int ticketId, int creatorId);

        Task<int> AddSingleRecipient(InsertSingleRecipientRequest request);

        Task RemoveManyRecipientReader(RemoveManyRecipientReaderCriteria criteria);

        Task<int> RemoveSingleRecipient(RemoveSingleRecipientRequest request);

        Task InsertIssueCreator(InsertIssueCreatorCriteria criteria);

        Task<IssueCreatorModel> GetIssueCreator(int ticketId);

        Task InsertManyRecipientReader(InsertManyRecipientReaderCriteria criteria);

        Task UpdateManyRecipientReader(UpdateManyRecipientReaderCriteria criteria);

        Task UpdateSingleRecipientReader(UpdateSingleRecipientReaderCriteria criteria);

        Task InsertNewTicketLeagueNotification(InsertNewTicketLeagueNotificationCriteria criteria);

        Task InsertNewTicketMatchNotification(InsertNewTicketMatchNotificationCriteria criteria);

        Task InsertNewTicketOtherNotification(InsertNewTicketOtherNotificationCriteria criteria);

        Task InsertNewRecipientNotification(InsertNewRecipientNotificationCriteria criteria);

        Task InsertNewMessageNotification(InsertNewMessageNotificationCriteria criteria);

        Task UpdateTicketStatus(UpdateTicketStatusCriteria criteria);

        Task<IEnumerable<StatusHistory>> GetStatusHistory(int ticketId);

        Task PinTicket(PinTicketCriteria criteria);
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

        public async Task AddManyRecipients(AddManyRecipientCriteria criteria)
        {
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<IEnumerable<int>> GetUserIdsByTicketId(int ticketId)
        {
            var criteria = new GetUserIdsByTicketIdCriteria
            {
                TicketId = ticketId
            };
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchAsync<int>(criteria);
        }

        public async Task<IEnumerable<int>> GetUserIdsIgnoredCreatorId(int ticketId, int creatorId)
        {
            var criteria = new GetUserIdsByTicketIdCriteria
            {
                TicketId = ticketId
            };
            criteria.ThrowExceptionIfInvalid();
            var userIds = await dynamicRepository.FetchAsync<int>(criteria);
            return userIds.Where(id => id != creatorId);
        }

        public async Task<int> AddSingleRecipient(InsertSingleRecipientRequest request)
        {
            var criteria = mapper.Map<AddSingleRecipientCriteria>(request);
            criteria.ThrowExceptionIfInvalid();
            return (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
        }

        public async Task RemoveManyRecipientReader(RemoveManyRecipientReaderCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

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

        public async Task InsertIssueCreator(InsertIssueCreatorCriteria criteria)
        {
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<IssueCreatorModel> GetIssueCreator(int ticketId)
        {
            var creator = new QueryIssueCreatorCriteria()
            {
                TicketId = ticketId
            };
            return (await dynamicRepository.FetchAsync<IssueCreatorModel>(creator))?.FirstOrDefault();
        }

        public async Task InsertManyRecipientReader(InsertManyRecipientReaderCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

        public async Task UpdateManyRecipientReader(UpdateManyRecipientReaderCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

        public async Task UpdateSingleRecipientReader(UpdateSingleRecipientReaderCriteria criteria)
        {
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task InsertNewTicketLeagueNotification(InsertNewTicketLeagueNotificationCriteria criteria)
        => await dynamicRepository.ExecuteAsync(criteria);

        public async Task InsertNewTicketMatchNotification(InsertNewTicketMatchNotificationCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

        public async Task InsertNewTicketOtherNotification(InsertNewTicketOtherNotificationCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

        public async Task InsertNewRecipientNotification(InsertNewRecipientNotificationCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

        public async Task InsertNewMessageNotification(InsertNewMessageNotificationCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

        public async Task UpdateTicketStatus(UpdateTicketStatusCriteria criteria)
        {
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<IEnumerable<StatusHistory>> GetStatusHistory(int ticketId)
        {
            var criteria = new GetStatusHistoryCriteria
            {
                TicketId = ticketId
            };

            return await dynamicRepository.FetchAsync<StatusHistory>(criteria);
        }

        public async Task PinTicket(PinTicketCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);
    }
}