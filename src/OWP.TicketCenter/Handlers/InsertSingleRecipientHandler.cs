using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class InsertSingleRecipientRequest : InsertOrRemoveSingleRecipientModel, IRequest<int>;

public class InsertRecipientHandler(
    Repository repository,
    RecipientService recipientService,
    NotificationService notificationService,
    MessageClient messageClient,
    PreMatchNotificationService preMatchNotification) : IRequestHandler<InsertSingleRecipientRequest, int>
{
    public async Task<int> Handle(InsertSingleRecipientRequest request, CancellationToken cancellationToken)
    {
        var recipients = new List<InsertOrRemoveManyRecipientModel>
        {
            new ()
            {
                RecipientIds = new List<int>{request.RecipientId},
                RecipientType = request.RecipientType
            }
        };

        await recipientService.InsertManyRecipientReader(recipients, request.TicketId, request.ActorId);

        await NotifyToUsers(request);

        return await repository.AddSingleRecipient(request);
    }

    private async Task NotifyToUsers(InsertOrRemoveSingleRecipientModel request)
    {
        // publish rabbit
        await messageClient.SendMessage(request, RoutingKey.TicketCenterNewRecipient);

        var userIds = new List<string> { request.RecipientId.ToString() };
        await notificationService.NotifyNewAddRecipient(userIds, request.TicketInfo);

        if (request.TicketEventType == (int)EventType.LiveIncident)
        {
            var parentMatchId = await repository.GetParentMatchIdByTicketId(request.TicketId);
            await notificationService.NotifyNewAddRecipientLga(userIds, parentMatchId, request.ActorId);
        }

        if (request.TicketEventType is (int)EventType.League or (int)EventType.Match)
        {
            await preMatchNotification.NotifyNewAddRecipientPrematch(request.TicketId, request.TicketEventType, userIds);
        }
    }
}