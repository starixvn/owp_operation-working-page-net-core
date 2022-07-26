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

public class NewTicketIssueSupportRequest : NewTicketModelBase, IRequest<int>
{
    public string Subject { get; set; }
}

public class NewTicketIssueSupportHandler : IRequestHandler<NewTicketIssueSupportRequest, int>
{
    private readonly Repository repository;
    private readonly IMapper mapper;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly MessageClient messageClient;


    public NewTicketIssueSupportHandler(
        Repository repository,
        IMapper mapper,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient)
    {
        this.repository = repository;
        this.mapper = mapper;
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
        await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

        await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

        var notificationMessage = new MessageTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId
        };

        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketSupport);

        await notificationService.NotifyNewMessageIncome(ticketId);

        await notificationService.NotifyNewAddRecipient(ticketId, request.CreatorId);

        return ticketId;
    }
}