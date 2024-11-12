using Fanex.NAP.Client;
using MediatR;
using OWP.PrematchAssignment.AutoAssignment.Helper;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;
using OWP.RabbitMQ;

namespace OWP.PrematchAssignment.AutoAssignment.Handlers;

public class AutoAssignRequest : QueryDayShiftPreMatchRequest, IRequest<QueryPreMatchLeagueViewModel>
{
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MajorPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MajorTakeOverPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> MainPairs { get; set; }
    public IEnumerable<IEnumerable<PreMatchLeagueAssignment>> EarlyPairs { get; set; }
}

public class AutoAssignHandler(
    IMediator mediator,
    PreMatchAssignmentService preMatchAssignmentService,
    MessageClient messageClient) : IRequestHandler<AutoAssignRequest, QueryPreMatchLeagueViewModel>
{
    public async Task<QueryPreMatchLeagueViewModel> Handle(AutoAssignRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);

        var groupLeagues = PreprocessData(result);

        var assignments = HandleAssignPairs(request, groupLeagues, result.InfoSiteMatches);

        await preMatchAssignmentService.AutoAssignment(assignments);

        return result;
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
        assignments.AddRange(AssignPairs(groupLeagues.MajorLeagues, request.MajorTakeOverPairs));

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
            .Where(l => infoSiteMatches.Any(m => m.LeagueId == l.LeagueId && m.EventDateTime <= m.EventDateTime.Date.AddHours(Hours).AddMinutes(Minutes)))
            .ToList();
        var earlyTakeOverLeagues = earlyPairLeagues
            .Where(l => infoSiteMatches.Any(m => m.LeagueId == l.LeagueId && m.EventDateTime > m.EventDateTime.Date.AddHours(Hours).AddMinutes(Minutes)))
            .ToList();
        var mainPairLeagues = leagues.Except(earlyPairLeagues).ToList();

        assignments.AddRange(AssignPairs(earlyPairLeagues, request.EarlyPairs));
        assignments.AddRange(AssignPairs(earlyTakeOverLeagues, request.MainPairs.Reverse().ToList()));
        assignments.AddRange(AssignPairs(mainPairLeagues, request.MainPairs));

        return assignments;
    }

    private static List<PreMatchLeagueAssignment> AssignPairs(IEnumerable<LeagueAssignment> leagues, IEnumerable<IEnumerable<PreMatchLeagueAssignment>> pairs)
    {
        var assignments = new List<PreMatchLeagueAssignment>();
        var index = 0;
        var totaPairs = pairs.Count();
        foreach (var league in leagues)
        {
            var aindex = index % totaPairs;
            var assignees = pairs.ElementAt(aindex);

            foreach (var assignee in assignees)
            {
                assignments.Add(new PreMatchLeagueAssignment
                {
                    PreMatchLeagueId = league.PreMatchLeagueId,
                    UserId = assignee.UserId,
                    Username = assignee.Username,
                    Bettype = assignee.Bettype,
                    IsTakeOver = false,
                    AssignmentType = assignee.AssignmentType
                });
            }

            index++;
        }

        return assignments;
    }
}
