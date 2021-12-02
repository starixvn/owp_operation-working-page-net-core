using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class ChangeStatusRequest : PermissionCheckedModel, IRequest<MessageDetailViewModel>
    {
        public string Message { get; set; }

        public int? FileId { get; set; }

        public int TicketEventType { get; set; }

        public string TicketTitle { get; set; }
    }

    public class ChangeStatusHandler : IRequestHandler<NewMessageRequest, MessageDetailViewModel>
    {
        private readonly Repository repository;

        private readonly NotificationService notification;

        private readonly NewMessageDBWriter writer;

        public ChangeStatusHandler(Repository repository, NotificationService notification, NewMessageDBWriter writer)
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

            var criteria = new UpdateManyRecipientReaderCriteria
            {
                TicketId = request.TicketId,
                IsRead = false,
                ActorId = request.ActorId
            };

            await repository.UpdateManyRecipientReader(criteria);

            var sessionId = await writer.Execute(request);

            var result = await repository.AddNewMessage(message);

            await notification.NotifyNewMessageIncome(request.TicketId);

            return result;
        }
    }
}