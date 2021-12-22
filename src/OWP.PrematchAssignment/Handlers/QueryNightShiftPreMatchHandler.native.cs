using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;

namespace OWP.PreMatchAssignment.Handlers
{
    public class QueryNightShiftPreMatchRequest : IRequest<QueryPreMatchLeagueViewModel>
    {
        public string Employee { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public string LeagueName { get; set; }
    }

    public class QueryNightShiftPreMatchHandler : IRequestHandler<QueryNightShiftPreMatchRequest, QueryPreMatchLeagueViewModel>
    {
        private readonly ShiftAssignmentService shiftAssignmentService;

        public QueryNightShiftPreMatchHandler(ShiftAssignmentService shiftAssignmentService)
        {
            this.shiftAssignmentService = shiftAssignmentService;
        }

        public async Task<QueryPreMatchLeagueViewModel> Handle(QueryNightShiftPreMatchRequest request, CancellationToken cancellationToken)
        {
            return await shiftAssignmentService.QueryNightShift(request);
        }
    }
}