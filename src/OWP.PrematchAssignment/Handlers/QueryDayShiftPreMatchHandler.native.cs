using MediatR;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;
using System;
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
            await shiftAssignmentService.MapTicketLists(shiftAssignments);

            return shiftAssignments;

            //return new QueryPreMatchLeagueViewModel
            //{
            //    PreMatchLeagues = new List<LeagueAssignment>
            //        { shiftAssignments.PreMatchLeagues.ToList().FirstOrDefault() },
            //    InfoSiteMatches = new List<InfoSiteMatch> { shiftAssignments.InfoSiteMatches.ToList().FirstOrDefault() }
            //};
        }
    }
}