using AutoMapper;
using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class NewTicketIssueLiveIncidentRequest : NewTicketModelBase, IRequest<int>
{
    public int? OwpMatchId { get; set; }

    public string OwpLeagueName { get; set; }

    public string OwpHomeName { get; set; }

    public string OwpAwayName { get; set; }

    public int? EventType => (int)Enum.EventType.LiveIncident;

    public DateTime EventDateTimeGmt8 { get; set; }

}

public class NewTicketIssueLiveIncidentHandler(
    Repository repository,
    MessageService messageService,
    NotificationService notificationService,
    RecipientService recipientService,
    MessageClient messageClient,
    IMapper mapper) : IRequestHandler<NewTicketIssueLiveIncidentRequest, int>
{
    public async Task<int> Handle(NewTicketIssueLiveIncidentRequest request, CancellationToken cancellationToken)
    {
        var criteria = mapper.Map<CreateNewTicketCriteria>(request);
        var ticketId = await repository.CreateTicketIssue(criteria);
        request.TicketId = ticketId;

        var creatorCriteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        var newMessage = await messageService.InsertMessage(request, ticketId);

        await repository.InsertIssueCreator(creatorCriteria);

        await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

        await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

        await NotifyToUsers(request, ticketId, newMessage);

        return ticketId;
    }

    private async Task NotifyToUsers(NewTicketIssueLiveIncidentRequest request, int ticketId,
        NewMessageRequestModel newMessage)
    {
        var notificationMessage = new MessageLiveIncidentTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId,
            OwpLeagueName = request.OwpLeagueName,
            OwpHomeName = request.OwpHomeName,
            OwpAwayName = request.OwpAwayName
        };

        await messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketLiveIncident);

        var userIds = await repository.GetUserIdsByTicketId(ticketId);
        var parentMatchId = await repository.GetParentMatchIdByTicketId(ticketId);
        await notificationService.NotifyNewMessageLga(userIds, parentMatchId, request.CreatorId);

        var eventModel = await BuildModelTicket(request, ticketId);
        await notificationService.NotifyNewAddRecipient(userIds, eventModel);
        await notificationService.NotifyNewAddRecipientLga(userIds, parentMatchId, request.CreatorId);
    }

    public async Task<TicketInfo> BuildModelTicket(NewTicketIssueLiveIncidentRequest request, int TicketId)
    {
        var lastComments = (await repository.GetTwoLastComment(new List<int> { TicketId })).ToList();

        return new TicketInfo
        {
            TicketId = TicketId,
            LeagueName = request.OwpLeagueName,
            OwpLeagueName = request.OwpLeagueName,
            Messages = lastComments,
            TicketStatus = "Open",
            TypeName = request.TicketTypeName,
            TicketSubTypeName = request.SubTypeName,
            HomeName = request.OwpHomeName,
            AwayName = request.OwpAwayName,
            EventType = request.EventType ?? 0,
            KoTime = request.EventDateTimeGmt8,
            LastCommentDate = lastComments.Count != 0 ? lastComments[0].CreatedDate : new MessageInfoModel().CreatedDate,
            TicketType = request.TicketType,
            TicketSubTypeId = request.SubType
        };
    }
}