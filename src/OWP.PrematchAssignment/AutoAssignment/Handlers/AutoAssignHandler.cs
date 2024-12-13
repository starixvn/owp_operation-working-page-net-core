using AutoMapper;
using MediatR;
using OWP.PrematchAssignment.AutoAssignment.Helper;
using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.AutoAssignment.Handlers;

public class AutoAssignRequest : IRequest<IList<PreMatchLeagueAssignment>>
{
    public DateTime Date { get; set; }
    public int ActorId { get; set; }
    public List<string> MatchOffers { get; set; }
    public List<int> EarlyOffered { get; set; }

    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MajorPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MajorTakeOverPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MainPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> EarlyPairs { get; set; }
}

public class AutoAssignHandler(
    IMapper mapper,
    ShiftAssignmentService shiftAssignmentService) : IRequestHandler<AutoAssignRequest, IList<PreMatchLeagueAssignment>>
{
    public async Task<IList<PreMatchLeagueAssignment>> Handle(AutoAssignRequest request, CancellationToken cancellationToken)
    {
        var model = new QueryDayShiftPreMatchRequest
        {
            EventDate = request.Date,
            ActorId = request.ActorId,
            EarlyOffered = request.EarlyOffered,
            MatchOffers = request.MatchOffers
        };
        var shiftAssignments = await shiftAssignmentService.QueryDayShift(model);

        var groupLeagues = PreprocessData(shiftAssignments);

        var assignments = HandleAssignPairs(request, groupLeagues, shiftAssignments.InfoSiteMatches);

        return assignments;
    }

    private static GroupPreMatchLeagueModel PreprocessData(QueryPreMatchLeagueViewModel model)
    {
        // exclude leagues
        var filteredModel = ExcludeUnSupportedLeagueHelper.Filter(model);

        // group leagues
        var groupLeagues = GroupAndSortLeagueHelper.Build(filteredModel.PreMatchLeagues);

        return groupLeagues;
    }

    private List<PreMatchLeagueAssignment> HandleAssignPairs(
        AutoAssignRequest request,
        GroupPreMatchLeagueModel groupLeagues,
        IEnumerable<InfoSiteMatch> infoSiteMatches)
    {
        if (!groupLeagues.MajorLeagues.Any())
        {
            request.MainPairs = [.. request.MainPairs, .. request.MajorPairs, .. request.MajorTakeOverPairs];
        }

        var assignments = new List<PreMatchLeagueAssignment>();

        assignments.AddRange(AssignPairs(groupLeagues.MajorLeagues, request.MajorPairs));
        assignments.AddRange(AssignPairs(groupLeagues.MajorLeagues, request.MajorTakeOverPairs, true));

        IList<LeagueAssignment> remainLeagues = [.. groupLeagues.GreenDotLeagues, .. groupLeagues.NoGreenDotLeagues, .. groupLeagues.FantasySabaLeagues];
        assignments.AddRange(AssignRemainLeagues(request, remainLeagues, infoSiteMatches));

        assignments.AddRange(AssignChildLeagues(groupLeagues, assignments));

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

        var earlyLeagues = leagues
            .Where(l => infoSiteMatches.Any(m => m.LeagueId == l.LeagueId && m.EventDateTime <= request.Date.Date.AddHours(Hours).AddMinutes(Minutes)))
            .ToList();
        var earlyNoTakeOverLeagues = earlyLeagues
            .Where(l => !infoSiteMatches.Any(m => m.LeagueId == l.LeagueId && m.EventDateTime > request.Date.Date.AddHours(Hours).AddMinutes(Minutes)))
            .ToList();
        var mainLeagues = leagues.Except(earlyLeagues).ToList();

        var totalLeague = leagues.Count();
        var totalEarlyPair = request.EarlyPairs.Count();
        var totalMainPair = request.MainPairs.Count();
        var totalLeagueNoTakeOver = earlyNoTakeOverLeagues.Count;

        if (totalMainPair == 0)
        {
            return assignments;
        }

        var totalLeaguePerEarlyPair = (float)(totalLeague - totalLeagueNoTakeOver) / totalMainPair;
        var totalLeagueEarlyPairs = (int)Math.Round(totalLeaguePerEarlyPair * totalEarlyPair);

        if (earlyLeagues.Count < totalLeagueEarlyPairs)
        {
            var additional = totalLeagueEarlyPairs - earlyLeagues.Count;
            earlyLeagues = [.. earlyLeagues, .. mainLeagues.Take(additional).ToList()];
        }

        mainLeagues = leagues.Except(earlyLeagues).ToList();

        assignments.AddRange(AssignPairs(earlyLeagues, request.EarlyPairs));

        var earlyLeaguesNeadTakeOver = earlyLeagues
            .Where(l => infoSiteMatches.Any(m => m.LeagueId == l.LeagueId && m.EventDateTime > request.Date.Date.AddHours(Hours).AddMinutes(Minutes)))
            .ToList();
        if (earlyLeaguesNeadTakeOver.Count > 0)
        {
            assignments.AddRange(AssignPairs(earlyLeaguesNeadTakeOver, request.MainPairs, true));
            assignments.AddRange(AssignPairs(mainLeagues, request.MainPairs.Reverse()));
        }
        else
        {
            assignments.AddRange(AssignPairs(mainLeagues, request.MainPairs));
        }

        return assignments;
    }

    private List<PreMatchLeagueAssignment> AssignChildLeagues(GroupPreMatchLeagueModel groupLeagues, IList<PreMatchLeagueAssignment> assignments)
    {
        var childAssignments = new List<PreMatchLeagueAssignment>();
        IList<LeagueAssignment> mainLeagues = [
            ..groupLeagues.MajorLeagues,
            ..groupLeagues.GreenDotLeagues,
            ..groupLeagues.NoGreenDotLeagues,
            ..groupLeagues.FantasySabaLeagues];
        foreach (var child in groupLeagues.ChildLeagues)
        {
            var mainLeague = FindMainLeague(mainLeagues, child);
            if (mainLeague != null)
            {
                var mainAssignments = assignments.Where(x => x.PreMatchLeagueId == mainLeague.PreMatchLeagueId).ToList();
                childAssignments.AddRange(mapper.Map<IList<PreMatchLeagueAssignment>>(mainAssignments)
                    .Select(x =>
                    {
                        x.PreMatchLeagueId = child.PreMatchLeagueId;
                        return x;
                    }));
            }
        }

        return childAssignments;
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

    private static LeagueAssignment FindMainLeague(IList<LeagueAssignment> mainLeagues, LeagueAssignment childLeague)
    {
        var rule = AutoAssignmentConstants.ChildLeagueRules.First(x => childLeague.LeagueName.EndsWith(x));
        var leagueLength = childLeague.LeagueName.Length - rule.Length;
        var league = childLeague.LeagueName[..leagueLength].Trim();
        return mainLeagues.FirstOrDefault(x => string.Compare(x.LeagueName, league, StringComparison.OrdinalIgnoreCase) == 0);
    }
}
