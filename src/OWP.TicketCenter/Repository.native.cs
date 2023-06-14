using AutoMapper;
using Fanex.Data.Repository;
using OWP.Extensions;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter;

public interface Repository
{
    Task<IList<EventCheckingOption>> GetEventOptionInfo(string keyword);

    Task<IList<EventLGAOption>> GetEventOptionLGAInfo(string keyword);

    Task<IList<ParentType>> GetTicketTypes();

    Task<IList<SubType>> GetAllTicketSubTypes();

    Task<int> BuildTicketId(int ticketType, int subType);

    Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request);

    Task AddNewTicketIssueOther(NewTicketIssueOtherRequest request);

    Task AddNewTicketIssueSupport(NewTicketIssueSupportRequest request);

    Task AddNewTicketIssueLiveIncident(NewTicketIssueLiveIncidentRequest request);

    Task<MessageDetailViewModel> AddNewMessage(NewMessageRequestModel message);

    Task AddManyRecipients(AddManyRecipientCriteria criteria);

    Task AddNewTicketIssueLeague(NewTicketIssueLeagueRequest request);

    Task<IEnumerable<TicketOtherInfo>> GetEventTicketOther(QueryEventTicketsRequest request);

    Task<IEnumerable<TicketSupportInfo>> GetEventTicketSupport(QueryEventTicketsRequest request);

    Task<IEnumerable<TicketMatchInfo>> GetEventTicketLiveIncident(QueryEventTicketsRequest request);

    Task<IEnumerable<TicketLeagueInfo>> GetEventTicketLeague(QueryEventTicketsRequest request);

    Task<IEnumerable<TicketMatchInfo>> GetEventTicketMatch(QueryEventTicketsRequest request);

    Task<EventTicketMatchModel> GetTicketMatchDetail(int ticketId);

    Task<EventTicketMatchModel> GetTicketLiveIncidentDetail(int ticketId);

    Task<EventTicketLeagueModel> GetTicketLeagueDetail(int ticketId);

    Task<EventTicketOtherModel> GetTicketOtherDetail(int ticketId);

    Task<EventTicketSupportModel> GetTicketSupportDetail(int ticketId);

    Task<IEnumerable<RecipientResponseModel>> GetRecipientByTicketId(int ticketId);

    Task<List<string>> GetUserIdsByTicketId(int ticketId, int actorId = 0);

    Task<int> AddSingleRecipient(InsertSingleRecipientRequest request);

    Task RemoveManyRecipientReader(RemoveManyRecipientReaderCriteria criteria);

    Task<int> RemoveSingleRecipient(RemoveSingleRecipientRequest request);

    Task InsertIssueCreator(InsertIssueCreatorCriteria criteria);

    Task<IssueCreatorModel> GetIssueCreator(int ticketId);

    Task InsertManyRecipientReader(InsertManyRecipientReaderCriteria criteria);

    Task UpdateManyRecipientReader(UpdateManyRecipientReaderCriteria criteria);

    Task UpdateSingleRecipientReader(UpdateSingleRecipientReaderCriteria criteria);

    Task UpdateTicketStatus(UpdateTicketStatusCriteria criteria);

    Task UpdateTicketSubType(UpdateTicketSubTypeCriteria criteria);

    Task<IEnumerable<StatusHistory>> GetStatusHistory(int ticketId);

    Task PinTicket(PinTicketCriteria criteria);

    Task DeleteTicket(DeleteTicketCriteria criteria);

    Task<IEnumerable<SubTypeModel>> GetSubTypeByTicketTypeId(int ticketTypeId);

    Task<bool> QueryExistedTicketId(int ticketId);

    Task UpdateIsSavedMessage(SaveMessageRequest request);

    Task UpdateTicketsAsRead(int actorId);

    Task<IEnumerable<MessageInfoModel>> GetTwoLastComment(IList<int> ticketIds);

    Task<(bool, IEnumerable<MessageDetailViewModel>)> GetMessageInLastTwoDays(int ticketId);

    Task<TicketInfoLgaModel> GetTicketInfoLga(int ticketId);
    Task<(bool, IEnumerable<MessageDetailViewModel>)> GetNextListMessages(GetNextListMessageCriteria criteria);

    Task<(bool canShowMoreMessage, List<MessageDetailViewModel> messages)> GetMessageListByGreaterMessageId(int ticketId, int messageId);
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

    public async Task<IList<EventCheckingOption>> GetEventOptionInfo(string keyword)
    {
        var criteria = new GetEventCriteria
        {
            Keyword = keyword
        };
        criteria.ThrowExceptionIfInvalid();
        return (await dynamicRepository.FetchAsync<EventCheckingOption>(new GetEventCriteria
        {
            Keyword = keyword
        })).ToList();
    }

    public async Task<IList<EventLGAOption>> GetEventOptionLGAInfo(string keyword)
    {
        var criteria = new GetEventLGACriteria
        {
            Keyword = keyword
        };
        criteria.ThrowExceptionIfInvalid();
        return (await dynamicRepository.FetchAsync<EventLGAOption>(criteria)).ToList();
    }

    public async Task<IList<ParentType>> GetTicketTypes()
        => (await dynamicRepository.FetchAsync<ParentType>(new GetTicketTypeCriteria()))
            .ToList();

    public async Task<IList<SubType>> GetAllTicketSubTypes()
        => (await dynamicRepository.FetchAsync<SubType>(new GetAllTicketSubTypeCriteria()))
            .ToList();

    public async Task<int> BuildTicketId(int ticketType, int subType)
        => (await dynamicRepository.FetchAsync<int>(new BuildNewTicketIdCriteria
        {
            TicketType = ticketType,
            SubType = subType
        })).FirstOrDefault();
    public async Task AddNewTicketIssueLiveIncident(NewTicketIssueLiveIncidentRequest request)
    {
        var criteria = mapper.Map<AddNewTicketIssueLiveIncidentCriteria>(request);
        criteria.ThrowExceptionIfInvalid();
        await dynamicRepository.ExecuteAsync(criteria);
    }
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

    public async Task AddNewTicketIssueSupport(NewTicketIssueSupportRequest request)
    {
        var criteria = mapper.Map<AddNewTicketIssueSupportCriteria>(request);
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

    public async Task<List<string>> GetUserIdsByTicketId(int ticketId, int actorId = 0)
    {
        var criteria = new GetUserIdsByTicketIdCriteria { TicketId = ticketId };
        var userIds = await dynamicRepository.FetchAsync<int>(criteria);

        var selectedUserIds = actorId == 0 ? userIds.Where(u => u != actorId) : userIds;

        return selectedUserIds.Distinct().Select(u => u.ToString()).ToList();
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
        var criteria = new AddNewTicketIssueMessageCriteria
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

    public async Task<IEnumerable<TicketMatchInfo>> GetEventTicketMatch(QueryEventTicketsRequest request)
    {
        var criteria = mapper.Map<GetEventTicketMatchCriteria>(request);
        criteria.ThrowExceptionIfInvalid();
        return await dynamicRepository.FetchAsync<TicketMatchInfo>(criteria);
    }

    public async Task<IEnumerable<TicketOtherInfo>> GetEventTicketOther(QueryEventTicketsRequest request)
    {
        var criteria = mapper.Map<GetEventTicketOtherCriteria>(request);
        criteria.ThrowExceptionIfInvalid();
        return await dynamicRepository.FetchAsync<TicketOtherInfo>(criteria);
    }

    public async Task<IEnumerable<TicketSupportInfo>> GetEventTicketSupport(QueryEventTicketsRequest request)
    {
        var criteria = mapper.Map<GetEventTicketSupportCriteria>(request);
        criteria.ThrowExceptionIfInvalid();
        return await dynamicRepository.FetchAsync<TicketSupportInfo>(criteria);
    }

    public async Task<IEnumerable<TicketMatchInfo>> GetEventTicketLiveIncident(QueryEventTicketsRequest request)
    {
        var criteria = mapper.Map<GetEventTicketLiveIncidentCriteria>(request);
        var result = await dynamicRepository.FetchAsync<TicketMatchInfo>(criteria);

        var eventTicketLiveIncident = result.ToList();
        eventTicketLiveIncident.ToList().ForEach(x => { x.EventType = 5; });

        return eventTicketLiveIncident;
    }

    public async Task<IEnumerable<TicketLeagueInfo>> GetEventTicketLeague(QueryEventTicketsRequest request)
    {
        var criteria = mapper.Map<GetEventTicketLeagueCriteria>(request);

        criteria.ThrowExceptionIfInvalid();
        return await dynamicRepository.FetchAsync<TicketLeagueInfo>(criteria);
    }

    public async Task<EventTicketMatchModel> GetTicketMatchDetail(int ticketId)
    {
        var criteria = new TicketDetailMatchCriteria
        {
            TicketId = ticketId
        };

        return await dynamicRepository.GetAsync<EventTicketMatchModel>(criteria);
    }

    public async Task<EventTicketMatchModel> GetTicketLiveIncidentDetail(int ticketId)
    {
        var criteria = new TicketDetailLiveIncidentCriteria { TicketId = ticketId };
        return await dynamicRepository.GetAsync<EventTicketMatchModel>(criteria);
    }

    public async Task<EventTicketLeagueModel> GetTicketLeagueDetail(int ticketId)
    {
        var criteria = new TicketDetailLeagueCriteria { TicketId = ticketId };
        return await dynamicRepository.GetAsync<EventTicketLeagueModel>(criteria);
    }

    public async Task<EventTicketOtherModel> GetTicketOtherDetail(int ticketId)
    {
        var criteria = new TicketDetailOtherCriteria { TicketId = ticketId };
        return await dynamicRepository.GetAsync<EventTicketOtherModel>(criteria);
    }

    public async Task<EventTicketSupportModel> GetTicketSupportDetail(int ticketId)
    {
        var criteria = new TicketDetailSupportCriteria { TicketId = ticketId };
        return await dynamicRepository.GetAsync<EventTicketSupportModel>(criteria);
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
        var creator = new QueryIssueCreatorCriteria
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

    public async Task UpdateTicketStatus(UpdateTicketStatusCriteria criteria)
    {
        criteria.ThrowExceptionIfInvalid();
        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task UpdateTicketSubType(UpdateTicketSubTypeCriteria criteria)
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

    public async Task DeleteTicket(DeleteTicketCriteria criteria)
        => await dynamicRepository.ExecuteAsync(criteria);

    public async Task<bool> QueryExistedTicketId(int ticketId)
    {
        var criteria = new GetExistedTicketIdCriteria
        {
            TicketId = ticketId
        };
        var existedTicketId = (await dynamicRepository.FetchAsync<int?>(criteria)).FirstOrDefault();

        return existedTicketId.HasValue;
    }

    public async Task<IEnumerable<SubTypeModel>> GetSubTypeByTicketTypeId(int ticketTypeId)
    {
        var criteria = new GetSubTypeCriteria
        {
            TicketTypeId = ticketTypeId
        };
        return await dynamicRepository.FetchAsync<SubTypeModel>(criteria);
    }

    public async Task UpdateIsSavedMessage(SaveMessageRequest request)
    {
        var criteria = new SaveMessageCriteria
        {
            MessageId = request.MessageId,
            IsSaved = request.IsSaved
        };
        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task UpdateTicketsAsRead(int actorId)
    {
        var criteria = new MarkAllTicketsCriteria
        {
            ActorId = actorId
        };
        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task<IEnumerable<MessageInfoModel>> GetTwoLastComment(IList<int> ticketIds)
    {
        var criteria = new GetTwoLastCommentCriteria
        {
            TicketId = string.Join(',', ticketIds)
        };

        return await dynamicRepository.FetchAsync<MessageInfoModel>(criteria);
    }

    public async Task<(bool, IEnumerable<MessageDetailViewModel>)> GetMessageInLastTwoDays(int ticketId)
    {
        var criteria = new GetMessageInLast2DaysCriteria { TicketId = ticketId };
        var messages = await dynamicRepository.FetchAsync<MessageDetailViewModel>(criteria);
        return (criteria.CanShowMoreMessage, messages);
    }


    public async Task<TicketInfoLgaModel> GetTicketInfoLga(int ticketId)
    {
        var criteria = new GetInfoTicketLgaCriteria
        {
            TicketId = ticketId
        };
        return (await dynamicRepository.FetchAsync<TicketInfoLgaModel>(criteria)).FirstOrDefault();
    }
    public async Task<(bool, IEnumerable<MessageDetailViewModel>)> GetNextListMessages(GetNextListMessageCriteria criteria)
    {
        var messages = await dynamicRepository.FetchAsync<MessageDetailViewModel>(criteria);
        return (criteria.CanShowMoreMessage, messages);
    }

    public async Task<(bool, List<MessageDetailViewModel>)> GetMessageListByGreaterMessageId(int ticketId, int messageId)
    {
        var criteria = new GetMessageByGreaterMessageIdCriteria
        {
            TicketId = ticketId,
            MessageId = messageId
        };
        var messages = await dynamicRepository.FetchAsync<MessageDetailViewModel>(criteria);
        return (criteria.CanShowMoreMessage, messages.ToList());
    }
}