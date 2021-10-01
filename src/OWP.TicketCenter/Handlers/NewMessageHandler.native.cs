using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewMessageRequest : IRequest<InsertMessageResultModel>
    {
        public int TicketId { get; set; }

        public string Message { get; set; }

        public int CreatorId { get; set; }

        public int? FileId { get; set; }
    }

    public class NewMessageHandler : IRequestHandler<NewMessageRequest, InsertMessageResultModel>
    {
        private readonly Repository repository;

        public NewMessageHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<InsertMessageResultModel> Handle(NewMessageRequest request, CancellationToken cancellationToken)
        {
            
            var message = new NewMessageRequestModel
            {
                TicketId = request.TicketId,
                Message = request.Message,
                CreatorId = request.CreatorId,
                FileId = request.FileId
            };
            return await repository.AddNewMessage(message);
        }
    }
}
