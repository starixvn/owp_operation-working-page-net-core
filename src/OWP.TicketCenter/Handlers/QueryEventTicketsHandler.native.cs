using System.Linq;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryEventTicketsRequest : IRequest<EventTicketViewModel>
    {
        public int ActorId { get; set; }

        public string Keyword { get; set; }

        public int TicketTypeId { get; set; }

        public string TicketId { get; set; }

        public int MarketKey { get; set; }
    }

    public class QueryEventTicketsHandler : IRequestHandler<QueryEventTicketsRequest, EventTicketViewModel>
    {
        private readonly Repository repository;

        public QueryEventTicketsHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<EventTicketViewModel> Handle(QueryEventTicketsRequest request, CancellationToken cancellationToken)
        {
            var ticketLeague = await repository.GetEventTicketLeague(request);
            var ticketMatch = await repository.GetEventTicketMatch(request);
            var ticketOther = await repository.GetEventTicketOther(request);
            return new EventTicketViewModel
            {
                EventTicketLeagues = ticketLeague.ToList(),
                EventTicketMatch = ticketMatch.ToList(),
                EventTicketOthers = ticketOther.ToList()
            };
        }
    }
}