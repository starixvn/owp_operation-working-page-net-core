using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueEventRequest : NewTicketModelBase, IRequest<int>
    {
        public int OwpLeagueId { get; set; }

        public int? OwpHomeId { get; set; }

        public bool IsLeague { get; set; }

        public int? OwpAwayId { get; set; }
    }

    public class NewTicketIssueEventHandler : IRequestHandler<NewTicketIssueEventRequest, int>
    {
        private readonly Repository repository;
        public NewTicketIssueEventHandler(Repository repository)
        {
            this.repository = repository;
        }
        public async Task<int> Handle(NewTicketIssueEventRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueEvent(request);
            await repository.AddNewMessage(ticketId, request.CreatorId, request.Message);
           
            return ticketId;
        }
    }
}
