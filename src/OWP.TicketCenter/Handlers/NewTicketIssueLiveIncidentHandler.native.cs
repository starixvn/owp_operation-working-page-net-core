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
    public int OwpMatchId { get; set; }

    public string OwpLeagueName { get; set; }

    public string OwpHomeName { get; set; }

    public string OwpAwayName { get; set; }
}

public class NewTicketIssueLiveIncidentHandler : IRequestHandler<NewTicketIssueLiveIncidentRequest, int>
{
    private readonly MessageClient messageClient;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;

    public NewTicketIssueLiveIncidentHandler(
        Repository repository,
        MessageService messageService,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient)
    {
        this.repository = repository;
        this.notificationService = notificationService;
        this.recipientService = recipientService;
        this.messageClient = messageClient;
        this.messageService = messageService;
    }

    public async Task<int> Handle(NewTicketIssueLiveIncidentRequest request, CancellationToken cancellationToken)
    {
        var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);

        request.TicketId = ticketId;

        await repository.AddNewTicketIssueLiveIncident(request);

        var newMessage = await messageService.InsertMessage(request, ticketId);

        var criteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        await repository.InsertIssueCreator(criteria);

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

        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketLiveIncident);

        var userIds = await repository.GetUserIdsByTicketId(ticketId);
        await notificationService.NotifyNewMessageIncome(userIds);
        await notificationService.NotifyNewMessageLga(userIds, request.OwpMatchId, request.CreatorId);

        await notificationService.NotifyNewAddRecipient(userIds);
        await notificationService.NotifyNewAddRecipientLga(userIds, request.OwpMatchId, request.CreatorId);
    }
}