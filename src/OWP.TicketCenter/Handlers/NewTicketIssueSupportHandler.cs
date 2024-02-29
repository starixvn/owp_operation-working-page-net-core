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

    public int? EventType => (int)Enum.EventType.Support;

}

public class NewTicketIssueSupportHandler : IRequestHandler<NewTicketIssueSupportRequest, int>
{
    private readonly MessageClient messageClient;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;
    private readonly IMapper mapper;
   

    public NewTicketIssueSupportHandler(
        Repository repository,
        MessageService messageService,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient,
        IMapper mapper)
    {
        this.repository = repository;
        this.messageService = messageService;
        this.notificationService = notificationService;
        this.recipientService = recipientService;
        this.messageClient = messageClient;
        this.mapper = mapper;
    }

    public async Task<int> Handle(NewTicketIssueSupportRequest request, CancellationToken cancellationToken)
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

    private async Task NotifyToUsers(NewTicketIssueSupportRequest request, int ticketId, NewMessageRequestModel newMessage)
    {
        var notificationMessage = new MessageTransform
        {
            TicketId = ticketId,
            Comment = newMessage.Message,
            ActorId = request.CreatorId
        };

        messageClient.SendMessage(notificationMessage, RoutingKey.TicketCenterNewTicketSupport);

        var userIds = await repository.GetUserIdsByTicketId(request.TicketId);

        var ticketModel = await BuildModelTicket(request, ticketId);

        await notificationService.NotifyNewAddRecipient(userIds, ticketModel);
    }
    public async Task<TicketInfo> BuildModelTicket(NewTicketIssueSupportRequest request, int TicketId)
    {
        var lastComments = (await repository.GetTwoLastComment(new List<int> { TicketId })).ToList();
        
        return new TicketInfo
        {
            TicketId = TicketId,
            Subject = request.Subject,
            Messages = lastComments,
            TicketStatus = "Open",
            TypeName = request.TicketTypeName,
            TicketSubTypeName = request.SubTypeName,
            LastCommentDate = lastComments.FirstOrDefault(new MessageInfoModel()).CreatedDate,
            TicketType = request.TicketType,
            TicketSubTypeId = request.SubType, 
            EventType = request.EventType ?? 0
        };

    }
}