using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;
using MessageTemplate = OWP.TicketCenter.Notification.Const.MessageTemplate;

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
                   RecipientIds = new List<int>(){request.RecipientId},
                   RecipientType = request.RecipientType
                }
            };

            await recipientService.InsertManyRecipientReader(recipients, request.TicketId);

            var result = await repository.AddSingleRecipient(request);
            var sessionId = await writer.Execute(request);

            await notificationService.Notify(sessionId, request.TicketId, (EventType)request.TicketEventType);

            return result;
        }
    }
}