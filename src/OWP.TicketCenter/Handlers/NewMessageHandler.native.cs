using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.NotificationHandler;
using OWP.NotificationHandler.Constants;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Models;
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
        private readonly MessageClient messageClient;
        private readonly INapSession napSession;

        public NewMessageHandler(
            Repository repository,
            NotificationService notification,
            INapSession napSession,
            MessageClient messageClient)
        {
            this.repository = repository;
            this.notification = notification;
            this.napSession = napSession;
            this.messageClient = messageClient;
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


            var result = await repository.AddNewMessage(message);

            // publish rabbit
            messageClient.SendMessage(request, RoutingKey.TicketCenterNewMessage);

            await notification.NotifyNewMessageIncome(request.TicketId);

            return result;
        }
    }
}