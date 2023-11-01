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

public class NewTicketIssueLogWorkRequest : NewTicketModelBase, IRequest<int>
{
    public int? AdminSiteId { get; set; }

    public string Subject { get; set; }

    public string Status { get; set; }

    public int? EventType => (int)Enum.EventType.LogWork;

}

public class NewTicketIssueLogWorkHandler : IRequestHandler<NewTicketIssueLogWorkRequest, int>
{
    private readonly MessageClient messageClient;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;
    private readonly IMapper mapper;

    public NewTicketIssueLogWorkHandler(
        Repository repository,
        MessageService messageService,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient,
        IMapper mapper)
    {
        this.repository = repository;
        this.notificationService = notificationService;
        this.recipientService = recipientService;
        this.messageClient = messageClient;
        this.messageService = messageService;
        this.mapper = mapper;
    }

    public async Task<int> Handle(NewTicketIssueLogWorkRequest request, CancellationToken cancellationToken)
    {
        var criteria = mapper.Map<CreateNewTicketCriteria>(request);
        var ticketId = await repository.CreateTicketIssue(criteria);
        request.TicketId = ticketId;

        var newMessage = await messageService.InsertMessage(request, ticketId);

        var creatorCriteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        await repository.InsertIssueCreator(creatorCriteria);

        await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

        await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

        var notificationMessage = new MessageTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId
        };

        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketLogWork);

        var userIds = await repository.GetUserIdsByTicketId(ticketId);
        await notificationService.NotifyNewMessageIncome(userIds);

        await notificationService.NotifyNewAddRecipient(userIds);

        return ticketId;
    }
}