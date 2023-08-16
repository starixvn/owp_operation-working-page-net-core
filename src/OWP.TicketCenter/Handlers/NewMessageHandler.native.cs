using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class NewMessageRequest : PermissionCheckedModel, IRequest<MessageDetailViewModel>
{
    public string Message { get; set; }

    public int? FileId { get; set; }

    public int? QuoteMessageId { get; set; }

    public int TicketEventType { get; set; }

    public string TicketTitle { get; set; }
}

public class NewMessageHandler : IRequestHandler<NewMessageRequest, MessageDetailViewModel>
{
    private readonly MessageClient messageClient;
    private readonly PreMatchNotificationService preMatchNotification;
    private readonly NotificationService notification;
    private readonly Repository repository;

    public NewMessageHandler(
        Repository repository,
        NotificationService notification,
        MessageClient messageClient,
        PreMatchNotificationService preMatchNotification)
    {
        this.repository = repository;
        this.notification = notification;
        this.messageClient = messageClient;
        this.preMatchNotification = preMatchNotification;
    }

    public async Task<MessageDetailViewModel> Handle(NewMessageRequest request, CancellationToken cancellationToken)
    {
        var message = new NewMessageRequestModel
        {
            TicketId = request.TicketId,
            Message = request.Message,
            CreatorId = request.ActorId,
            QuoteMessageId = request.QuoteMessageId,
            FileId = request.FileId
        };
        var criteria = new UpdateManyRecipientReaderCriteria
        {
            TicketId = request.TicketId,
            IsRead = false,
            ActorId = request.ActorId
        };

        await repository.UpdateManyRecipientReader(criteria);
        var result = await repository.AddNewMessage(message);

        await NotifyToUsers(request);

        return result;
    }

    private async Task NotifyToUsers(NewMessageRequest request)
    {
        messageClient.SendMessage(request, RoutingKey.TicketCenterNewMessage);

        var userIds = await repository.GetUserIdsByTicketId(request.TicketId);
        await notification.NotifyNewMessageIncome(userIds);

        if (request.TicketEventType == (int)EventType.LiveIncident)
        {
            var ticketInfoLga = await repository.GetTicketInfoLga(request.TicketId);
            await notification.NotifyNewMessageLga(userIds, ticketInfoLga.OwpMatchId, request.ActorId);
        }
        if (IsEventCheckingType(request.TicketEventType))
        {
            await preMatchNotification.NotifyToReloadTicketInPrematchTable(request.TicketId, request.TicketEventType, userIds);
        }
    }

    private static bool IsEventCheckingType(int ticketType) => ticketType is (int)EventType.League or (int)EventType.Match;
}