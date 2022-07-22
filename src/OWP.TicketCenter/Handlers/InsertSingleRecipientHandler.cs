using MediatR;
using OWP.NotificationHandler;
using OWP.NotificationHandler.Constants;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers
{
    public class InsertSingleRecipientRequest : InsertOrRemoveSingleRecipientModel, IRequest<int>
    {
    }

    public class InsertRecipientHandler : IRequestHandler<InsertSingleRecipientRequest, int>
    {
        private readonly Repository repository;
        private readonly RecipientService recipientService;
        private readonly NotificationService notificationService;
        //private readonly NewRecipientMessageDBWriter writer;
        private readonly MessageClient messageClient;

        public InsertRecipientHandler(MessageClient messageClient)
        {
            this.messageClient = messageClient;
        }

        public InsertRecipientHandler(
            Repository repository,
            RecipientService recipientService,
            NotificationService notificationService,
            MessageClient messageClient
          //NewRecipientMessageDBWriter writer,
          )
        {
            this.repository = repository;
            this.recipientService = recipientService;
            this.notificationService = notificationService;
            this.messageClient = messageClient;
            //this.writer = writer;
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

            //var sessionId = await writer.Execute(request);

            //await notificationService.Notify(new NotificationSource
            //{
            //    SessionId = sessionId,
            //    TicketId = request.TicketId,
            //    EventType = request.TicketEventType
            //});
            //var notificationSource = new NotificationSource(sessionId, request.TicketId, request.TicketEventType);
            //publisher.PushMessage<NotificationSource>(notificationSource, RoutingKey.TicketCenter);

            // publish rabbit
            messageClient.SendMessage(request, RoutingKey.TicketCenterNewRecipient);

            await notificationService.NotifyNewAddRecipient(request.TicketId, request.ActorId);

            return await repository.AddSingleRecipient(request);
        }
    }
}