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

public class NewTicketIssueLeagueRequest : NewTicketModelBase, IRequest<int>
{
    public int? OwpLeagueId { get; set; }

    public int? EventType => (int)Enum.EventType.League;
}

public class NewTicketIssueLeagueHandler : IRequestHandler<NewTicketIssueLeagueRequest, int>
{
    private readonly MessageClient messageClient;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;
    private readonly PreMatchNotificationService preMatchNotification;
    private readonly IMapper mapper;

    public NewTicketIssueLeagueHandler(
        Repository repository,
        MessageService messageService,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient,
        PreMatchNotificationService preMatchNotification,
        IMapper mapper)
    {
        this.repository = repository;
        this.messageService = messageService;
        this.notificationService = notificationService;
        this.recipientService = recipientService;
        this.messageClient = messageClient;
        this.preMatchNotification = preMatchNotification;
        this.mapper = mapper;
    }

    public async Task<int> Handle(NewTicketIssueLeagueRequest request, CancellationToken cancellationToken)
    {
        var criteria = mapper.Map<CreateNewTicketCriteria>(request);
        var ticketId = await repository.CreateTicketIssue(criteria);
        request.TicketId = ticketId;

        var creatorCriteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        await repository.InsertIssueCreator(creatorCriteria);

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
        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketLeague);

        var userIds = await repository.GetUserIdsByTicketId(ticketId);
        await notificationService.NotifyNewMessageIncome(userIds);
        await notificationService.NotifyNewAddRecipient(userIds);

        await preMatchNotification.NotifyToReloadTicketInPrematchTable(request.TicketId, request.TicketType, userIds);
    }
}