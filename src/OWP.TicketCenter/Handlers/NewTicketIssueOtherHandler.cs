using MediatR;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueOtherRequest : NewTicketModelBase, IRequest<int>
    {
        public string Subject { get; set; }
    }

    public class NewTicketIssueOtherHandler : IRequestHandler<NewTicketIssueOtherRequest, int>
    {
        private readonly Repository repository;

        private readonly RecipientService recipientService;
        private readonly NotificationService notificationService;
        private readonly MessageClient messageClient;
        private readonly MessageService messageService;

        public NewTicketIssueOtherHandler(
            Repository repository,
            MessageService messageService,
            NotificationService notificationService,
            RecipientService recipientService,
            MessageClient messageClient = null)
        {
            this.repository = repository;
            this.recipientService = recipientService;
            this.messageService = messageService;
            this.messageClient = messageClient;
            this.notificationService = notificationService;
        }

        public async Task<int> Handle(NewTicketIssueOtherRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);
            request.TicketId = ticketId;

            await repository.AddNewTicketIssueOther(request);

            var criteria = new InsertIssueCreatorCriteria
            {
                TicketId = ticketId,
                CreatorId = request.CreatorId
            };

            await repository.InsertIssueCreator(criteria);

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

            await notificationService.NotifyNewMessageIncome(ticketId);

            await notificationService.NotifyNewAddRecipient(ticketId, request.CreatorId);

            return ticketId;
        }
    }
}