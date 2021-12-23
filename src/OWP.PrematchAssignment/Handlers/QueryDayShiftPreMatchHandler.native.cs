using MediatR;
using OWP.Extensions;
using OWP.PrematchAssignment.Models;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OWP.PreMatchAssignment.Handlers
{
    public class QueryDayShiftPreMatchRequest : IRequest<QueryPreMatchLeagueViewModel>
    {
        public string Employee { get; set; }

        public DateTime EventDate { get; set; }

        public string LeagueName { get; set; }
    }

    public class QueryDayShiftPreMatchHandler : IRequestHandler<QueryDayShiftPreMatchRequest, QueryPreMatchLeagueViewModel>
    {
        private readonly ShiftAssignmentService shiftAssignmentService;

        public QueryDayShiftPreMatchHandler(ShiftAssignmentService shiftAssignmentService)
        {
            this.shiftAssignmentService = shiftAssignmentService;
        }

        public async Task<QueryPreMatchLeagueViewModel> Handle(QueryDayShiftPreMatchRequest request, CancellationToken cancellationToken)
        {
            var shiftAssignments = await shiftAssignmentService.QueryDayShift(request);
            var leagueIds = shiftAssignments.PreMatchLeagues.Select(x => x.LeagueId);
            var infoSiteMatchIds = shiftAssignments.InfoSiteMatches.Select(x => x.InfoSiteMatchId);

            var leagueTickets = await shiftAssignmentService.QueryAllLeaguesTicket(leagueIds);
            var matchTickets = await shiftAssignmentService.QueryAllMatchesTicket(infoSiteMatchIds);

            foreach (var league in shiftAssignments.PreMatchLeagues)
            {
                var leagueTicket = leagueTickets.GetValue(league.LeagueId, new List<TicketLeagueBasicInfo>());
                league.TicketList = leagueTicket;
            }

            foreach (var match in shiftAssignments.InfoSiteMatches)
            {
                var matchTicket = matchTickets.GetValue(match.InfoSiteMatchId, new List<TicketMatchBasicInfo>());
                match.TicketList = matchTicket;
            }

            return shiftAssignments;
        }
    }
}