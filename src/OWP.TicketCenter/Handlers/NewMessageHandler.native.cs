using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class NewMessageRequest : IRequest<MessageDetailViewModel>
    {
        public int TicketId { get; set; }

        public string Message { get; set; }

        public int CreatorId { get; set; }

        public int? FileId { get; set; }
    }

    public class NewMessageHandler : IRequestHandler<NewMessageRequest, MessageDetailViewModel>
    {
        private readonly Repository repository;

        private readonly NotificationService notification;

        public NewMessageHandler(Repository repository, NotificationService notification)
        {
            this.repository = repository;
            this.notification = notification;
        }

        public async Task<MessageDetailViewModel> Handle(NewMessageRequest request, CancellationToken cancellationToken)
        {
            
            var message = new NewMessageRequestModel
            {
                TicketId = request.TicketId,
                Message = request.Message,
                CreatorId = request.CreatorId,
                FileId = request.FileId
            };
            await repository.UpdateManyRecipientReader(request.TicketId, false);

            await notification.NotifyNewMessageIncome(request.TicketId, request.CreatorId);

            return await repository.AddNewMessage(message);
        }
    }
}
