namespace OWP.PreMatchAssignment.Handlers;

using MediatR;
using Models;
using Services;

public class QueryNightShiftPreMatchRequest : IRequest<QueryPreMatchLeagueViewModel>
{
    public string Employee { get; set; }

    public DateTime EventDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public string LeagueName { get; set; }

    public int ActorId { get; set; }

    public List<string> MatchOffers { get; set; }
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
        await shiftAssignmentService.MapTicketLists(shiftAssignments.PreMatchLeagues, shiftAssignments.InfoSiteMatches, request.ActorId);

        return shiftAssignments;
    }
}