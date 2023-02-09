using MediatR;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;

namespace OWP.PreMatchAssignment.Handlers;

public class QueryDayShiftPreMatchRequest : IRequest<QueryPreMatchLeagueViewModel>
{
    public string Employee { get; set; }

    public DateTime EventDate { get; set; }

    public string LeagueName { get; set; }

    public int ActorId { get; set; }

    public List<string> MatchOffers { get; set; }
}

public class QueryDayShiftPreMatchHandler : IRequestHandler<QueryDayShiftPreMatchRequest, QueryPreMatchLeagueViewModel>
{
    private readonly ShiftAssignmentService shiftAssignmentService;

    private readonly MaxbetSpreadService maxbetSpreadService;

    public QueryDayShiftPreMatchHandler(ShiftAssignmentService shiftAssignmentService, MaxbetSpreadService maxbetSpreadService)
    {
        this.shiftAssignmentService = shiftAssignmentService;
        this.maxbetSpreadService = maxbetSpreadService;
    }

    public async Task<QueryPreMatchLeagueViewModel> Handle(QueryDayShiftPreMatchRequest request, CancellationToken cancellationToken)
    {
        var shiftAssignments = await shiftAssignmentService.QueryDayShift(request);
        await maxbetSpreadService.Build(shiftAssignments.InfoSiteMatches);
        await shiftAssignmentService.MapTicketLists(shiftAssignments.PreMatchLeagues, shiftAssignments.InfoSiteMatches, request.ActorId);

        return shiftAssignments;
    }
}