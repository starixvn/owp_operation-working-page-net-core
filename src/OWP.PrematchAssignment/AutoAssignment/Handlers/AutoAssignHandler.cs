using MediatR;
using OWP.PrematchAssignment.AutoAssignment.Helper;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.AutoAssignment.Handlers;

public class AutoAssignRequest : IRequest
{
    public string Employee { get; set; }
    public DateTime EventDate { get; set; }
    public string LeagueName { get; set; }
    public int ActorId { get; set; }
    public List<string> MatchOffers { get; set; }
    public List<int> EarlyOffered { get; set; }

    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MajorPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MajorTakeOverPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MainPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> EarlyPairs { get; set; }
}

public class AutoAssignHandler(
    ShiftAssignmentService shiftAssignmentService,
    PreMatchAssignmentService preMatchAssignmentService) : IRequestHandler<AutoAssignRequest>
{
    public async Task Handle(AutoAssignRequest request, CancellationToken cancellationToken)
    {
        var model = new QueryDayShiftPreMatchRequest
        {
            Employee = request.Employee,
            EventDate = request.EventDate,
            ActorId = request.ActorId,
            EarlyOffered = request.EarlyOffered,
            LeagueName = request.LeagueName,
            MatchOffers = request.MatchOffers
        };
        var shiftAssignments = await shiftAssignmentService.QueryDayShift(model);

        var groupLeagues = PreprocessData(shiftAssignments);

        var assignments = HandleAssignPairs(request, groupLeagues, shiftAssignments.InfoSiteMatches);

        if (assignments.Any())
        {
            await preMatchAssignmentService.AutoAssign(assignments);
        }
    }

    private static GroupPreMatchLeagueModel PreprocessData(QueryPreMatchLeagueViewModel model)
    {
        // exclude leagues
        var filteredModel = ExcludeUnSupportedLeagueHelper.Filter(model);

        // group leagues
        var groupLeagues = GroupAndSortLeagueHelper.Build(filteredModel.PreMatchLeagues);

        return groupLeagues;
    }

    private static List<PreMatchLeagueAssignment> HandleAssignPairs(
        AutoAssignRequest request,
        GroupPreMatchLeagueModel groupLeagues,
        IEnumerable<InfoSiteMatch> infoSiteMatches)
    {
        if (!groupLeagues.MajorLeagues.Any())
        {
            request.MainPairs = request.MainPairs.Concat(request.MajorPairs).ToList();
        }

        var assignments = new List<PreMatchLeagueAssignment>();

        assignments.AddRange(AssignPairs(groupLeagues.MajorLeagues, request.MajorPairs));
        assignments.AddRange(AssignPairs(groupLeagues.MajorLeagues, request.MajorTakeOverPairs, true));

        assignments.AddRange(AssignRemainLeagues(request, groupLeagues.GreenDotLeagues, infoSiteMatches));
        assignments.AddRange(AssignRemainLeagues(request, groupLeagues.NoGreenDotLeagues, infoSiteMatches));
        assignments.AddRange(AssignRemainLeagues(request, groupLeagues.FantasySabaLeagues, infoSiteMatches));

        return assignments;
    }

    private static List<PreMatchLeagueAssignment> AssignRemainLeagues(
        AutoAssignRequest request,
        IEnumerable<LeagueAssignment> leagues,
        IEnumerable<InfoSiteMatch> infoSiteMatches)
    {
        const byte Hours = 17;
        const byte Minutes = 30;
        var assignments = new List<PreMatchLeagueAssignment>();

        var earlyPairLeagues = leagues
            .Where(l => infoSiteMatches.Any(m => m.LeagueId == l.LeagueId && m.EventDateTime <= request.EventDate.Date.AddHours(Hours).AddMinutes(Minutes)))
            .ToList();
        var earlyTakeOverLeagues = earlyPairLeagues
            .Where(l => infoSiteMatches.Any(m => m.LeagueId == l.LeagueId && m.EventDateTime > request.EventDate.Date.AddHours(Hours).AddMinutes(Minutes)))
            .ToList();
        var mainPairLeagues = leagues.Except(earlyPairLeagues).ToList();

        assignments.AddRange(AssignPairs(earlyPairLeagues, request.EarlyPairs));

        if (earlyTakeOverLeagues.Any())
        {
            assignments.AddRange(AssignPairs(earlyTakeOverLeagues, request.MainPairs, true));
            assignments.AddRange(AssignPairs(mainPairLeagues, request.MainPairs.Reverse()));
        }
        else
        {
            assignments.AddRange(AssignPairs(mainPairLeagues, request.MainPairs));
        }

        return assignments;
    }

    private static List<PreMatchLeagueAssignment> AssignPairs(
        IEnumerable<LeagueAssignment> leagues,
        IEnumerable<IEnumerable<PreMatchLeagueAssignment>> pairs,
        bool isTakeOver = false)
    {
        var assignments = new List<PreMatchLeagueAssignment>();
        var index = 0;
        var totalPairs = pairs.Count();
        if (totalPairs == 0)
        {
            return assignments;
        }

        foreach (var league in leagues)
        {
            var aindex = index % totalPairs;
            var assignees = pairs.ElementAt(aindex);

            foreach (var assignee in assignees)
            {
                assignments.Add(new PreMatchLeagueAssignment
                {
                    PreMatchLeagueId = league.PreMatchLeagueId,
                    UserId = assignee.UserId,
                    Username = assignee.Username,
                    Bettype = assignee.Bettype,
                    IsTakeOver = isTakeOver,
                    AssignmentType = assignee.AssignmentType
                });
            }

            index++;
        }

        return assignments;
    }
}
