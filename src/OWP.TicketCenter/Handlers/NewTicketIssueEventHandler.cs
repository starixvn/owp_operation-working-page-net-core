using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class NewTicketIssueEventRequest : NewTicketModelBase, IRequest<int>
{
    public int InfoSiteMatchId { get; set; }

    public int OwpLeagueId { get; set; }

    public int? OwpHomeId { get; set; }

    public int? OwpAwayId { get; set; }
}

public class NewTicketIssueEventHandler : IRequestHandler<NewTicketIssueEventRequest, int>
{
    private readonly Repository repository;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly MessageClient messageClient;

    public NewTicketIssueEventHandler(
        Repository repository,
        MessageService messageService,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient)
    {
        this.repository = repository;
        this.messageService = messageService;
        this.notificationService = notificationService;
        this.recipientService = recipientService;
        this.messageClient = messageClient;
    }

    public async Task<int> Handle(NewTicketIssueEventRequest request, CancellationToken cancellationToken)
    {
        var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);
        request.TicketId = ticketId;
        await repository.AddNewTicketIssueEvent(request);

        var newMessage = await messageService.InsertMessage(request, ticketId);

        var criteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        await repository.InsertIssueCreator(criteria);

        await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

        await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

        var notificationMessage = new MessageTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId
        };

        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketEvent);

        await notificationService.NotifyNewMessageIncome(ticketId);

        await notificationService.NotifyNewAddRecipient(ticketId, request.CreatorId);

        return ticketId;
    }
}