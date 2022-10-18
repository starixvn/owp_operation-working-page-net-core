namespace OWP.TicketCenter.Handlers;

using AutoMapper;
using Criteria;
using MediatR;
using Models;
using Notification.Models;
using Notification.Services;
using RabbitMQ;
using RabbitMQ.Constants;
using Services;

public class NewTicketIssueLiveIncidentRequest : NewTicketModelBase, IRequest<int>
{
    public int OwpMatchId { get; set; }

    public string OwpLeagueName { get; set; }

    public string OwpHomeName { get; set; }

    public string OwpAwayName { get; set; }
}

public class NewTicketIssueLiveIncidentHandler : IRequestHandler<NewTicketIssueLiveIncidentRequest, int>
{
    private readonly IMapper mapper;
    private readonly MessageClient messageClient;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;

    public NewTicketIssueLiveIncidentHandler(
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

    public async Task<int> Handle(NewTicketIssueLiveIncidentRequest request, CancellationToken cancellationToken)
    {
        var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);

        request.TicketId = ticketId;

        await repository.AddNewTicketIssueLiveIncident(request);

        var messages = new List<NewMessageRequestModel>();

        foreach (var file in request.Files)
        {
            messages.Add(new NewMessageRequestModel
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

        await notificationService.NotifyNewMessageLga(ticketId, request.OwpMatchId, request.CreatorId);

        await notificationService.NotifyNewAddRecipientLga(ticketId, request.OwpMatchId, request.CreatorId);

        return ticketId;
    }
}