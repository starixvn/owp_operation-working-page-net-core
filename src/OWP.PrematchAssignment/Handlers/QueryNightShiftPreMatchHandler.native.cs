using MediatR;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.Handlers;

public class QueryNightShiftPreMatchRequest : QueryDayShiftPreMatchRequest//, IRequest<QueryPreMatchLeagueViewModel>
{
    public TimeSpan StartTime { get; set; }
}

public class QueryNightShiftPreMatchHandler : IRequestHandler<QueryNightShiftPreMatchRequest, QueryPreMatchLeagueViewModel>
{
    private readonly ShiftAssignmentService shiftAssignmentService;

    private readonly MaxbetSpreadService maxbetSpreadService;

    public QueryNightShiftPreMatchHandler(ShiftAssignmentService shiftAssignmentService, MaxbetSpreadService maxbetSpreadService)
    {
        this.shiftAssignmentService = shiftAssignmentService;
        this.maxbetSpreadService = maxbetSpreadService;
    }

    public async Task<QueryPreMatchLeagueViewModel> Handle(QueryNightShiftPreMatchRequest request, CancellationToken cancellationToken)
    {
        var shiftAssignments = await shiftAssignmentService.QueryNightShift(request);

        await maxbetSpreadService.Build(shiftAssignments.InfoSiteMatches);
        await shiftAssignmentService.MapTicketLists(shiftAssignments.PreMatchLeagues.ToList(), shiftAssignments.InfoSiteMatches.ToList(), request.ActorId);

        return shiftAssignments;
    }
}