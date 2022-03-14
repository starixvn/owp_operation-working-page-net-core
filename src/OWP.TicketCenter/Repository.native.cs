using OWP.Extensions;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter
{
  using AutoMapper;
  using Criteria;
  using Fanex.Data.Repository;
  using OWP.Models;
  using OWP.TicketCenter.Handlers;
  using OWP.TicketCenter.Models;
  using OWP.Utils;

  public interface Repository
  {
    Task<IList<EventOptionInfoModel>> GetEventOptionInfo(string keyword);

    Task<IList<ParentType>> GetTicketTypes();

    Task<IList<SubType>> GetAllTicketSubTypes();

    Task<int> BuildTicketId(int ticketType, int subType);

    Task AddNewTicketIssueEvent(NewTicketIssueEventRequest request);

    Task AddNewTicketIssueOther(NewTicketIssueOtherRequest request);

    Task AddNewTicketIssueSupport(NewTicketIssueSupportRequest request);

    Task<MessageDetailViewModel> AddNewMessage(NewMessageRequestModel message);

    Task AddManyRecipients(AddManyRecipientCriteria criteria);

    Task AddNewTicketIssueLeague(NewTicketIssueLeagueRequest request);

    Task<IEnumerable<EventTicketOtherModel>> GetEventTicketOther(QueryEventTicketsRequest request);

    Task<IEnumerable<EventTicketSupportModel>> GetEventTicketSupport(QueryEventTicketsRequest request);

    Task<IEnumerable<EventTicketLeagueModel>> GetEventTicketLeague(QueryEventTicketsRequest request);

    Task<IEnumerable<EventTicketMatchModel>> GetEventTicketMatch(QueryEventTicketsRequest request);

    Task<IEnumerable<EventTicketMatchModel>> GetTicketMatchDetail(int ticketId);

    Task<IEnumerable<EventTicketLeagueModel>> GetTicketLeagueDetail(int ticketId);

    Task<IEnumerable<EventTicketOtherModel>> GetTicketOtherDetail(int ticketId);

    Task<IEnumerable<EventTicketSupportModel>> GetTicketSupportDetail(int ticketId);

    Task<IEnumerable<RecipientResponseModel>> GetRecipientByTicketId(int ticketId);

    Task<IEnumerable<int>> GetUserIdsByTicketId(int ticketId);

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

    Task InsertNewTicketSupportNotification(InsertNewTicketSupportNotificationCriteria criteria);

    Task InsertNewRecipientNotification(InsertNewRecipientNotificationCriteria criteria);

    Task InsertNewMessageNotification(InsertNewMessageNotificationCriteria criteria);

    Task UpdateTicketStatus(UpdateTicketStatusCriteria criteria);

    Task UpdateTicketSubType(UpdateTicketSubTypeCriteria criteria);

    Task<IEnumerable<StatusHistory>> GetStatusHistory(int ticketId);

    Task PinTicket(PinTicketCriteria criteria);

    Task DeleteTicket(DeleteTicketCriteria criteria);

    Task<IEnumerable<MessageUnreadCounter>> GetMessageUnreadByMultipleUserId(IList<int> userIds);

    Task<IEnumerable<SubTypeModel>> GetSubTypeByTicketTypeId(int ticketTypeId);

    Task<bool> QueryExistedTicketId(int ticketId);

    Task UpdateIsSavedMessage(SaveMessageRequest request);

    Task UpdateTicketsAsRead(int actorId);
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

    public async Task<IEnumerable<int>> GetUserIdsByTicketId(int ticketId)
    {
      var criteria = new GetUserIdsByTicketIdCriteria
      {
        TicketId = ticketId
      };
      criteria.ThrowExceptionIfInvalid();
      return await dynamicRepository.FetchAsync<int>(criteria);
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

    public async Task<IEnumerable<EventTicketSupportModel>> GetEventTicketSupport(QueryEventTicketsRequest request)
    {
      var criteria = mapper.Map<GetEventTicketSupportCriteria>(request);
      criteria.ThrowExceptionIfInvalid();
      return await dynamicRepository.FetchAsync<EventTicketSupportModel>(criteria);
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

    public async Task<IEnumerable<EventTicketSupportModel>> GetTicketSupportDetail(int ticketId)
    {
      var criteria = new TicketDetailSupportCriteria
      {
        TicketId = ticketId
      };
      criteria.ThrowExceptionIfInvalid();
      return await dynamicRepository.FetchAsync<EventTicketSupportModel>(criteria);
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

    public async Task InsertNewTicketSupportNotification(InsertNewTicketSupportNotificationCriteria criteria)
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
    public async Task<IEnumerable<MessageUnreadCounter>> GetMessageUnreadByMultipleUserId(IList<int> userIds)
    {
      var messageUnread = new List<MessageUnreadCounter>();
      foreach (var userId in userIds)
      {
        var result = await dynamicRepository.FetchAsync<int>(new CountUnreadMessageCriteria
        {
          UserId = userId
        });
        var totalUnreadMessage = result.FirstOrDefault();
        messageUnread.Add(new MessageUnreadCounter
        {
          UserId = userId,
          Total = totalUnreadMessage
        });
      }
      return messageUnread;
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
          IsSaved = request.IsSaved,
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
  }
}