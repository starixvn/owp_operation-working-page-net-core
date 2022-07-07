using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.MessagePublisher;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class NewMessageRequest : PermissionCheckedModel, IRequest<MessageDetailViewModel>
    {
        public string Message { get; set; }

        public int? FileId { get; set; }

        public int TicketEventType { get; set; }

        public string TicketTitle { get; set; }
    }

    public class NewMessageHandler : IRequestHandler<NewMessageRequest, MessageDetailViewModel>
    {
        private readonly Repository repository;
        private readonly NotificationService notification;
        private readonly NewMessageDBWriter writer;
        private readonly IMessagePublisher publisher;
        private readonly INapSession napSession;

        public NewMessageHandler(Repository repository, NotificationService notification, NewMessageDBWriter writer, IMessagePublisher publisher, INapSession napSession)
        {
            this.repository = repository;
            this.notification = notification;
            this.writer = writer;
            this.publisher = publisher;
            this.napSession = napSession;
        }

        public async Task<MessageDetailViewModel> Handle(NewMessageRequest request, CancellationToken cancellationToken)
        {
            var user = napSession?.UserId;
            var message = new NewMessageRequestModel
            {
                TicketId = request.TicketId,
                Message = request.Message,
                CreatorId = request.ActorId,
                FileId = request.FileId
            };

            var criteria = new UpdateManyRecipientReaderCriteria
            {
                TicketId = request.TicketId,
                IsRead = false,
                ActorId = request.ActorId
            };

            await repository.UpdateManyRecipientReader(criteria);

            var sessionId = await writer.Execute(request);

            var result = await repository.AddNewMessage(message);

            //await notification.Notify(new NotificationSource
            //{
            //    SessionId = sessionId,
            //    TicketId = request.TicketId,
            //    EventType = request.TicketEventType,
            //    IsNewMessage = true
            //});
            var notificationSource = new NotificationSource(sessionId, request.TicketId, request.TicketEventType, true);
            publisher.PushMessage<NotificationSource>(notificationSource, RoutingKey.TicketCenter);

            await notification.NotifyNewMessageIncome(request.TicketId);

            return result;
        }
    }
}