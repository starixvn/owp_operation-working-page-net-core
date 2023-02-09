using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class InsertSingleRecipientRequest : InsertOrRemoveSingleRecipientModel, IRequest<int>
{
}

public class InsertRecipientHandler : IRequestHandler<InsertSingleRecipientRequest, int>
{
    private readonly Repository repository;
    private readonly RecipientService recipientService;
    private readonly NotificationService notificationService;
    private readonly MessageClient messageClient;

    public InsertRecipientHandler(
        Repository repository,
        RecipientService recipientService,
        NotificationService notificationService,
        MessageClient messageClient
    )
    {
        this.repository = repository;
        this.recipientService = recipientService;
        this.notificationService = notificationService;
        this.messageClient = messageClient;
    }

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

        // publish rabbit
        messageClient.SendMessage(request, RoutingKey.TicketCenterNewRecipient);

        await notificationService.NotifyNewAddRecipient(request.TicketId, request.ActorId);

        if (request.TicketEventType == (int)EventType.LiveIncident)
        {
            var ticketInfoLga = await repository.GetTicketInfoLga(request.TicketId);
            await notificationService.NotifyNewAddRecipientLga(request.TicketId, ticketInfoLga.OwpMatchId, request.ActorId);
        }

        return await repository.AddSingleRecipient(request);
    }
}