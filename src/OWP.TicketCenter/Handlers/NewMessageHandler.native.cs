using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewMessageRequest:IRequest<int>
    {
        public int TicketId { get; set; }

        public IList<RecipientModel> NewRecipients { get; set; }

        public IList<RecipientModel> DeletedRecipients { get; set; }

        public string Message { get; set; }

        public int CreatorId { get; set; }
    }

    public class NewMessageHandler : IRequestHandler<NewMessageRequest, int>
    {
        private readonly Repository repository;

        public NewMessageHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(NewMessageRequest request, CancellationToken cancellationToken)
        {
            var messageId = await repository.AddNewMessage(request.TicketId, request.CreatorId, request.Message);
            return messageId;
        }
    }
}
