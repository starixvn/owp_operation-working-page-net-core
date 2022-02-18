using MediatR;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;




namespace OWP.PreMatchAssignment.Handlers
{
    public class QueryDayShiftPreMatchRequest : IRequest<QueryPreMatchLeagueViewModel>
    {
        public string Employee { get; set; }

        public DateTime EventDate { get; set; }

        public string LeagueName { get; set; }

        public int ActorId { get; set; }
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
            await shiftAssignmentService.MapTicketLists(shiftAssignments, request.ActorId);

            return shiftAssignments;
        }
    }
}