using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.ViewModels;
using System.Threading;
using System.Threading.Tasks;

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

        public NewMessageHandler(Repository repository, NotificationService notification, NewMessageDBWriter writer)
        {
            this.repository = repository;
            this.notification = notification;
            this.writer = writer;
        }

        public async Task<MessageDetailViewModel> Handle(NewMessageRequest request, CancellationToken cancellationToken)
        {
            var message = new NewMessageRequestModel
            {
                TicketId = request.TicketId,
                Message = request.Message,
                CreatorId = request.ActorId,
                FileId = request.FileId
            };
            await repository.UpdateManyRecipientReader(request.TicketId, false, request.ActorId);

            var sessionId = await writer.Execute(request);

            var result = await repository.AddNewMessage(message);

            await notification.Notify(sessionId, request.TicketId, request.TicketEventType);

            await notification.NotifyNewMessageIncome(request.TicketId, request.ActorId);

            return result;
        }
    }
}