using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class QueryNightShiftPreMatchRequest : QueryDayShiftPreMatchRequest//, IRequest<QueryPreMatchLeagueViewModel>
{
    public TimeSpan StartTime { get; set; }
}

public class QueryNightShiftPreMatchHandler(
    ShiftAssignmentService shiftAssignmentService,
    MaxbetSpreadService maxbetSpreadService)
    : IRequestHandler<QueryNightShiftPreMatchRequest, QueryPreMatchLeagueViewModel>
{
    public async Task<QueryPreMatchLeagueViewModel> Handle(QueryNightShiftPreMatchRequest request, CancellationToken cancellationToken)
    {
        var shiftAssignments = await shiftAssignmentService.QueryNightShift(request);

        await maxbetSpreadService.Build(shiftAssignments.InfoSiteMatches);
        await shiftAssignmentService.MapTicketLists(shiftAssignments.PreMatchLeagues.ToList(), shiftAssignments.InfoSiteMatches.ToList(), request.ActorId);

        return shiftAssignments;
    }
}