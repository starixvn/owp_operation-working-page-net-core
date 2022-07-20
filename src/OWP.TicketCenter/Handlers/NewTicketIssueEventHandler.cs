using AutoMapper;
using MediatR;
using OWP.MessagePublisher;
using OWP.NotificationHandler;
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
    private readonly IMapper mapper;
    private readonly NewTicketMatchMessageDBWriter writer;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly IMessagePublisher publisher;
    private readonly MessageClient messageClient;

    public NewTicketIssueEventHandler(
        Repository repository,
        IMapper mapper,
        NewTicketMatchMessageDBWriter writer,
        NotificationService notificationService,
        RecipientService recipientService,
        IMessagePublisher publisher,
        MessageClient messageClient)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.writer = writer;
        this.notificationService = notificationService;
        this.recipientService = recipientService;
        this.publisher = publisher;
        this.messageClient = messageClient;
    }

    public async Task<int> Handle(NewTicketIssueEventRequest request, CancellationToken cancellationToken)
    {
        var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);
        request.TicketId = ticketId;
        await repository.AddNewTicketIssueEvent(request);

        var messages = new List<NewMessageRequestModel>();
        foreach (var file in request.Files)
        {
            messages.Add(new()
            {
                TicketId = ticketId,
                CreatorId = request.CreatorId,
                Message = file.Title,
                FileId = file.FileId
            });
        }
        var newMessage = mapper.Map<NewMessageRequestModel>(request);
        messages.Add(newMessage);

        foreach (var message in messages)
        {
            await repository.AddNewMessage(message);
        }

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

        // publish
        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketEvent);

        await notificationService.NotifyNewMessageIncome(ticketId);

        await notificationService.NotifyNewAddRecipient(ticketId, request.CreatorId);

        return ticketId;
    }
}