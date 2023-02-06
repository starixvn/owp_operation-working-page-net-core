using MediatR;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;

namespace OWP.PreMatchAssignment.Handlers
{
    public class QueryNightShiftPreMatchRequest : IRequest<QueryNightShiftPreMatchLeagueViewModel>
    {
        public string Employee { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public string LeagueName { get; set; }

        public int ActorId { get; set; }

        public List<string> MatchOffers { get; set; }
    }

    public class QueryNightShiftPreMatchHandler : IRequestHandler<QueryNightShiftPreMatchRequest, QueryNightShiftPreMatchLeagueViewModel>
    {
        private readonly ShiftAssignmentService shiftAssignmentService;

        private readonly MaxbetSpreadService maxbetSpreadService;

        public QueryNightShiftPreMatchHandler(ShiftAssignmentService shiftAssignmentService, MaxbetSpreadService maxbetSpreadService)
        {
            this.shiftAssignmentService = shiftAssignmentService;
            this.maxbetSpreadService = maxbetSpreadService;
        }

        public async Task<QueryNightShiftPreMatchLeagueViewModel> Handle(QueryNightShiftPreMatchRequest request, CancellationToken cancellationToken)
        {
            var shiftAssignments = await shiftAssignmentService.QueryNightShift(request);

            await maxbetSpreadService.Build(shiftAssignments.InfoSiteMatches);
            await shiftAssignmentService.MapTicketLists(shiftAssignments, request.ActorId);

            return shiftAssignments;
        }
    }
}