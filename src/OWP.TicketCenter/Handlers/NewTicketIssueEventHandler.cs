using AutoMapper;
using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;


namespace OWP.TicketCenter.Handlers;

public class NewTicketIssueEventRequest : NewTicketModelBase, IRequest<int>
{
    public int? InfoSiteMatchId { get; set; }

    public int? OwpLeagueId { get; set; }

    public int? OwpHomeId { get; set; }

    public int? OwpAwayId { get; set; }

    public string OwpHomeName { get; set; }

    public string OwpAwayName { get; set; }

    public string OwpLeagueName { get; set; }

    public DateTime eventDateTimeGmt8 { get; set; }

    public int? EventType => (int)Enum.EventType.Match;
}

public class NewTicketIssueEventHandler : IRequestHandler<NewTicketIssueEventRequest, int>
{
    private readonly MessageClient messageClient;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;
    private readonly PreMatchNotificationService preMatchNotification;
    private readonly IMapper mapper;


    public NewTicketIssueEventHandler(
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

    public async Task<int> Handle(NewTicketIssueEventRequest request, CancellationToken cancellationToken)
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

        await NotifyToUsers(request, ticketId, newMessage);

        return ticketId;
    }

    private async Task NotifyToUsers(NewTicketIssueEventRequest request, int ticketId, NewMessageRequestModel newMessage)
    {
        var notificationMessage = new MessageTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId
        };
       messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketEvent);

        var userIds = await repository.GetUserIdsByTicketId(ticketId);

        var ticketModel = await BuildModelTicket(request, ticketId);

        await notificationService.NotifyNewAddRecipient(userIds,ticketModel);

        await preMatchNotification.NotifyNewAddRecipientPrematch(request.TicketId, (int)EventType.Match, userIds);
    }

    public async Task<TicketInfo> BuildModelTicket(NewTicketIssueEventRequest request, int TicketId)
    {
        var lastComments = (await repository.GetTwoLastComment(new List<int> { TicketId })).ToList();

        return new TicketInfo
        {
            TicketId = TicketId,
            LeagueName = request.OwpLeagueName,
            OwpLeagueName = request.OwpLeagueName,
            KoTime = request.eventDateTimeGmt8,
            HomeName = request.OwpHomeName,
            AwayName = request.OwpAwayName,
            Messages = lastComments,
            TicketStatus = "Open",
            TypeName = request.TicketTypeName,
            TicketSubTypeName = request.SubTypeName,
            OwpAwayId = request.OwpAwayId,
            OwpHomeId = request.OwpHomeId,
            OwpLeagueId = request.OwpLeagueId,
            EventType = request.EventType ?? 0,
            LastCommentDate = lastComments.FirstOrDefault(new MessageInfoModel()).CreatedDate,
            TicketType = request.TicketType,
            TicketSubTypeId = request.SubType
        };
    }
}