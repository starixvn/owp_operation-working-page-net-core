using MediatR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly NewRecipientMessageDBWriter writer;

        public InsertRecipientHandler(
            Repository repository,
            RecipientService recipientService,
            NotificationService notificationService,
            NewRecipientMessageDBWriter writer)
        {
            this.repository = repository;
            this.recipientService = recipientService;
            this.notificationService = notificationService;
            this.writer = writer;
        }

        public async Task<int> Handle(InsertSingleRecipientRequest request, CancellationToken cancellationToken)
        {
            var recipients = new List<InsertOrRemoveManyRecipientModel>()
            {
                new ()
                {
                   RecipientIds = new List<int>{request.RecipientId},
                   RecipientType = request.RecipientType
                }
            };

            await recipientService.InsertManyRecipientReader(recipients, request.TicketId);

            var sessionId = await writer.Execute(request);

            await notificationService.Notify(sessionId, request.TicketId, request.TicketEventType);

            return await repository.AddSingleRecipient(request);
        }
    }
}