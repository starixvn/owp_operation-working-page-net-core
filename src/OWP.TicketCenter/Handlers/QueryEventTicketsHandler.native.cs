using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;
using System.Linq;

namespace OWP.TicketCenter.Handlers
{
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryEventTicketsRequest : IRequest<List<TicketDetailBaseViewModel>>
    {
        public int ActorId { get; set; }

        public string Keyword { get; set; }

        public int TicketTypeId { get; set; }

        public string TicketId { get; set; }

        public int MarketKey { get; set; }
    }

    public class QueryEventTicketsHandler : IRequestHandler<QueryEventTicketsRequest, List<TicketDetailBaseViewModel>>
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

        public async Task<List<TicketDetailBaseViewModel>> Handle(QueryEventTicketsRequest request, CancellationToken cancellationToken)
        {
            var ticketLeague = await leagueService.GetAllLeague(request);
            var ticketMatch = await matchService.GetAllMatch(request);
            var ticketOthers = await otherService.GetAllTicketOther(request);
            
            var eventTickets = new List<TicketDetailBaseViewModel>();

            eventTickets.AddRange(ticketLeague);
            eventTickets.AddRange(ticketMatch);
            eventTickets.AddRange(ticketOthers);

            return SortEventTicketByCommentLatestDate(eventTickets);
        }

        private List<TicketDetailBaseViewModel> SortEventTicketByCommentLatestDate(
            List<TicketDetailBaseViewModel> tickets)
            => tickets.OrderByDescending(x => x.CommentLatestDate).ToList();
    }
}