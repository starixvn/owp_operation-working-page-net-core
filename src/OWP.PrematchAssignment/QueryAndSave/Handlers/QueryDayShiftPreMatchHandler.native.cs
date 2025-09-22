using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class QueryDayShiftPreMatchRequest : IRequest<QueryPreMatchLeagueViewModel>
{
    public string Employee { get; set; }

    public DateTime EventDate { get; set; }

    public string LeagueName { get; set; }

    public int ActorId { get; set; }

    public List<string> MatchOffers { get; set; }

    public List<int> EarlyOffered { get; set; }
}

public class QueryDayShiftPreMatchHandler(
    ShiftAssignmentService shiftAssignmentService,
    MaxbetSpreadService maxbetSpreadService)
    : IRequestHandler<QueryDayShiftPreMatchRequest, QueryPreMatchLeagueViewModel>
{
    public async Task<QueryPreMatchLeagueViewModel> Handle(QueryDayShiftPreMatchRequest request, CancellationToken cancellationToken)
    {
        var shiftAssignments = await shiftAssignmentService.QueryDayShift(request);
        await maxbetSpreadService.Build(shiftAssignments.InfoSiteMatches);
        await shiftAssignmentService.MapTicketLists(shiftAssignments.PreMatchLeagues.ToList(), shiftAssignments.InfoSiteMatches.ToList(), request.ActorId);

        return shiftAssignments;
    }

}