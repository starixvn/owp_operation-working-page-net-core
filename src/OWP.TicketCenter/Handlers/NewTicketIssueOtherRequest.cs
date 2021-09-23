using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Linq;

namespace OWP.TicketCenter.Handlers
{

    public class NewTicketIssueOtherRequest : NewTicketModelBase, IRequest<int>
    {
        public int TicketType { get; set; }

        public string Subject { get; set; }
    }

    public class NewTicketIssueOtherHandler : IRequestHandler<NewTicketIssueOtherRequest, int>
    {
        private readonly Repository repository;
        public NewTicketIssueOtherHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(NewTicketIssueOtherRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueOther(request);
            await repository.AddNewMessage(ticketId, request.UserId, request.Message);
            return ticketId;
        }
    }
}
