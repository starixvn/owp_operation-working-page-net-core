using AutoMapper;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class NewTicketIssueOtherRequest : NewTicketModelBase, IRequest<int>
{
    public string Subject { get; set; }

    public int? EventType => (int)Enum.EventType.Other;

}

public class NewTicketIssueOtherHandler : IRequestHandler<NewTicketIssueOtherRequest, int>
{
    private readonly MessageClient messageClient;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;
    private readonly IMapper mapper;
    private readonly INapSession currentSession;

    public NewTicketIssueOtherHandler(
        Repository repository,
        MessageService messageService,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient,
        IMapper mapper, 
        INapSession currentSession)
    {
        this.repository = repository;
        this.recipientService = recipientService;
        this.messageService = messageService;
        this.messageClient = messageClient;
        this.notificationService = notificationService;
        this.mapper = mapper;
        this.currentSession = currentSession;
    }

    public async Task<int> Handle(NewTicketIssueOtherRequest request, CancellationToken cancellationToken)
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

        var notificationMessage = new MessageTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId
        };

        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketOther);

        var userIds = await repository.GetUserIdsByTicketId(request.TicketId);
        var modelTicket = await BuildModelTicket(request, ticketId);
        await notificationService.NotifyNewAddRecipient(userIds, modelTicket);

        return ticketId;
    }

    public async Task<TicketInfo> BuildModelTicket(NewTicketIssueOtherRequest request, int TicketId)
    {
        var lastComments =await repository.GetTwoLastComment(new List<int> { TicketId });
    
        return new TicketInfo
        {
            TicketId = TicketId,
            Subject =request.Subject,
            Messages = lastComments.ToList(),
            TicketStatus = "Open",
            TypeName = request.TicketTypeName,
            TicketSubTypeName = request.SubTypeName,
            IsRead = currentSession.UserId == request.CreatorId
        };
    }
}