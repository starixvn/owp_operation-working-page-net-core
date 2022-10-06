using MediatR;
using AutoMapper;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.Notification.Models;

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
    private readonly Repository repository;
    private readonly RecipientService recipientService;
    private readonly IMapper mapper;
    private readonly MessageClient messageClient;

    public NewTicketIssueLiveIncidentHandler(Repository repository, RecipientService recipientService, IMapper mapper, MessageClient messageClient)
    {
        this.repository = repository;
        this.recipientService = recipientService;
        this.mapper = mapper;
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

        return ticketId;
    }
}