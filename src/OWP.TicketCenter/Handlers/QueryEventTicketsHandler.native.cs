using System.Linq;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    using System.Collections.Generic;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

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
        private readonly EventTicketLeagueService leagueService;

        private readonly EventTicketMatchService matchService;

        private readonly EventTicketOtherService otherService;

        public QueryEventTicketsHandler(EventTicketLeagueService leagueService,
            EventTicketMatchService matchService,
            EventTicketOtherService otherService)
        {
            this.leagueService = leagueService;
            this.matchService = matchService;
            this.otherService = otherService;
        }

        public async Task<EventTicketViewModel> Handle(QueryEventTicketsRequest request, CancellationToken cancellationToken)
        {

            var ticketLeague = await leagueService.GetAllLeague(request);
            var ticketMatch = await matchService.GetAllMatch(request);
            var ticketOthers = await otherService.GetAllTicketOther(request);
            return new EventTicketViewModel
            {
                EventTicketLeagues = ticketLeague.ToList(),
                EventTicketMatch = ticketMatch.ToList(),
                EventTicketOthers = ticketOthers.ToList()
            };
        }
    }
}