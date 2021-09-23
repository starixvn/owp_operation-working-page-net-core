using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueEventRequest : IRequest<bool>
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public int OwpLeagueId { get; set; }

        public int OwpHomeId { get; set; }

        public bool IsLeague { get; set; }

        public int OwpAwayId { get; set; }
    }

    public class NewTicketIssueEventHandler : IRequestHandler<NewTicketIssueEventRequest, bool>
    {
        private readonly Repository repository;
        public NewTicketIssueEventHandler(Repository repository)
        {
            this.repository = repository;
        }
        public async Task<bool> Handle(NewTicketIssueEventRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueEvent(request);
            return await Task.FromResult(true);
        }
    }
}
