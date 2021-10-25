using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;
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

        public NewMessageHandler(Repository repository)
        {
            this.repository = repository;
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
            return await repository.AddNewMessage(message);
        }
    }
}
