using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class NewTicketIssueSupportRequest : NewTicketModelBase, IRequest<int>
{
    public string Subject { get; set; }
}

public class NewTicketIssueSupportHandler : IRequestHandler<NewTicketIssueSupportRequest, int>
{
    private readonly Repository repository;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly MessageClient messageClient;


    public NewTicketIssueSupportHandler(
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

    public async Task<int> Handle(NewTicketIssueSupportRequest request, CancellationToken cancellationToken)
    {
        var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);
        request.TicketId = ticketId;

        await repository.AddNewTicketIssueSupport(request);

        var criteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        await repository.InsertIssueCreator(criteria);

        var newMessage = await messageService.InsertMessage(request, ticketId);

        await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

        await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

        await NotifyToUsers(request, ticketId, newMessage);

        return ticketId;
    }

    private async Task NotifyToUsers(NewTicketModelBase request, int ticketId, NewMessageRequestModel newMessage)
    {
        var notificationMessage = new MessageTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId
        };
        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketSupport);

        var userIds = await repository.GetUserIdsByTicketId(request.TicketId);
        await notificationService.NotifyNewMessageIncome(userIds);
        await notificationService.NotifyNewAddRecipient(userIds);
    }
}