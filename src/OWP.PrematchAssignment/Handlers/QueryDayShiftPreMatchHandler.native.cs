using System;
using System.Threading;
using System.Threading.Tasks;
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
            return await shiftAssignmentService.QueryDayShift(request);
        }
    }
}