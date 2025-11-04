using AutoMapper;
using OWP.Extensions;
using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.QueryAndSave.Handlers;
using OWP.PrematchAssignment.QueryAndSave.Helper;
using OWP.PrematchAssignment.QueryAndSave.Models;
using System.Globalization;

namespace OWP.PrematchAssignment.QueryAndSave.Services;

public interface ShiftAssignmentService
{
    Task<QueryPreMatchLeagueViewModel> QueryDayShift(QueryDayShiftPreMatchRequest request);

    Task<QueryPreMatchLeagueViewModel> QueryNightShift(QueryNightShiftPreMatchRequest request);

    Task MapTicketLists(List<LeagueAssignment> preMatchLeagues, List<InfoSiteMatch> infoSiteMatches, int actorId);
}

public class ShiftAssignmentServiceImp(Repository repository, IMapper mapper) : ShiftAssignmentService
{
    public async Task<QueryPreMatchLeagueViewModel> QueryDayShift(QueryDayShiftPreMatchRequest request)
    {
        var criteria = ShiftHelper.CreateShiftCriteria(request.EventDate, true);
        var assignmentShift = await BuildPreMatchAssignments(request, criteria);
        var sortedLeagues = SortPreMatchLeagues(assignmentShift.Leagues.Values);
        AddNumber(sortedLeagues);

        return CreateQueryPreMatchLeagueViewModel(sortedLeagues, assignmentShift.Matches);
    }

    private async Task<AssignmentShift> BuildPreMatchAssignments(QueryDayShiftPreMatchRequest request, QueryShiftPreMatchCriteria criteria)
    {
        var (preMatchLeagues, infoSiteMatches) = await repository.QueryDayShiftPreMatchAssignments(criteria);

        var earlyOfferedLeagues = await repository.GetEarlyOfferedLeagues(request.EventDate);

        preMatchLeagues.ForEach(x => x.IsEarlyOffered = earlyOfferedLeagues.Contains(x.LeagueName));

        var (filteredLeagues, filteredInfoSiteMatches) = FilterByRequest(preMatchLeagues, infoSiteMatches, request);

        await MapMatchStatusWhichLinkedOwp(filteredInfoSiteMatches, request.EventDate);
        await AddOddStatusNo(filteredInfoSiteMatches);

        return new AssignmentShift
        {
            Leagues = filteredLeagues.ToDictionary(x => x.LeagueId),
            Matches = filteredInfoSiteMatches
                .GroupBy(x => x.LeagueId)
                .ToDictionary(a => a.Key, b => b.ToList())
        };
    }

    public async Task<QueryPreMatchLeagueViewModel> QueryNightShift(QueryNightShiftPreMatchRequest request)
    {
        var nightShiftInfo = await QueryAssignmentsForNightShift(request);

        return BuildNightShiftWithRequestTime(nightShiftInfo.NightShifts, nightShiftInfo.DayShifts);
    }

    private async Task<NightShiftInfo> QueryAssignmentsForNightShift(QueryNightShiftPreMatchRequest request)
    {
        var dayCriteria = ShiftHelper.CreateShiftCriteria(request.EventDate, true);
        var nightRequestTimeCriteria = ShiftHelper.CreateShiftCriteria(request.EventDate, false, request.StartTime);

        var dbStartTime = repository.QueryExistedStartTime(request.EventDate);
        var dayAssignments = BuildPreMatchAssignments(request, dayCriteria);
        var nightAssignmentsWithRequestTime = BuildPreMatchAssignments(request, nightRequestTimeCriteria);

        await Task.WhenAll(dbStartTime, dayAssignments, nightAssignmentsWithRequestTime);

        var nightShiftInfo = new NightShiftInfo
        {
            DayShifts = dayAssignments.Result,
            NightShifts = nightAssignmentsWithRequestTime.Result,
            DBStartTime = dbStartTime.Result
        };

        return nightShiftInfo;
    }

    public async Task MapTicketLists(List<LeagueAssignment> preMatchLeagues, List<InfoSiteMatch> infoSiteMatches, int actorId)
    {
        var leagueIds = preMatchLeagues.Select(x => x.LeagueId);
        var infoSiteMatchIds = infoSiteMatches.Select(x => x.InfoSiteMatchId);

        var leagueTicketsTask = repository.QueryAllLeaguesTicket(leagueIds, actorId);
        var matchTicketsTask = repository.QueryAllMatchesTicket(infoSiteMatchIds, actorId);

        await Task.WhenAll(leagueTicketsTask, matchTicketsTask);

        foreach (var league in preMatchLeagues)
        {
            var leagueTicket = leagueTicketsTask.Result.GetValueOrDefault(league.LeagueId, []);
            league.TicketList = leagueTicket.OrderByDescending(x => x.LatestCreatedDate).ToList();
        }

        foreach (var match in infoSiteMatches)
        {
            var matchTicket = matchTicketsTask.Result.GetValueOrDefault(match.InfoSiteMatchId, []);
            match.TicketList = matchTicket.OrderByDescending(x => x.LatestCreatedDate).ToList();
        }
    }

    private QueryPreMatchLeagueViewModel BuildNightShiftWithRequestTime(AssignmentShift nightShift, AssignmentShift dayShift)
    {
        UpdateAssignmentHintNightShift(nightShift.Leagues, dayShift.Leagues);

        var (leaguesContainsHint, leaguesRest) = SplitPrematchLeagues(nightShift.Leagues.Values.ToList());
        var preMatchLeagues = SortByHint(leaguesContainsHint);
        preMatchLeagues.AddRange(SortPreMatchLeagues(leaguesRest));
        AddNumber(preMatchLeagues);

        return CreateQueryPreMatchLeagueViewModel(preMatchLeagues, nightShift.Matches);
    }

    public static void UpdateAssignmentHintNightShift(Dictionary<int, PreMatchLeague> nightShiftLeagues, Dictionary<int, PreMatchLeague> dayShiftLeagues)
    {
        foreach (var nightShiftLeague in nightShiftLeagues)
        {
            var dayShiftLeague = dayShiftLeagues.GetValueOrDefault(nightShiftLeague.Key, null);
            if (dayShiftLeague is not null)
            {
                UpdateAssignmentHint(nightShiftLeague.Value, dayShiftLeague);
            }
        }
    }

    private static void UpdateAssignmentHint(PreMatchLeague dest, PreMatchLeague source)
    {
        dest.HDPHint = source.HDP;
        dest.OtherHDPHint = source.OtherHDP;
        dest.OUHint = source.OU;
        dest.OtherOUHint = source.OtherOU;
        dest.HDPTakeOverHint = source.HDPTakeOver;
        dest.OtherHDPTakeOverHint = source.OtherHDPTakeOver;
        dest.OUTakeOverHint = source.OUTakeOver;
        dest.OtherOUTakeOverHint = source.OtherOUTakeOver;
    }

    private static (List<PreMatchLeague>, List<InfoSiteMatch>) FilterByRequest(
        IEnumerable<PreMatchLeague> preMatchLeagues,
        IEnumerable<InfoSiteMatch> infoSiteMatches,
        QueryDayShiftPreMatchRequest request)
    {
        var employee = request.Employee;
        var matchOffers = request.MatchOffers;

        var filteredLeagues = preMatchLeagues
            .Where(x => x.LeagueName.IsContains(request.LeagueName))
            .Where(x => string.IsNullOrEmpty(employee) ||
                CheckMainAssignmentContainsEmployee(x, employee) ||
                CheckOtherAssignmentContainsEmployee(x, employee))
            .ToList();

        filteredLeagues = request.EarlyOffered is null
            ? filteredLeagues
            : filteredLeagues.Where(x => request.EarlyOffered.Contains(ConvertBoolToInt(x.IsEarlyOffered))).ToList();

        var filteredMatches = infoSiteMatches
            .Where(x => filteredLeagues.Exists(y => y.PreMatchLeagueId == x.PreMatchLeagueId)).ToList();

        var (leagues, matches) = FilterByMatchOffer(filteredLeagues, filteredMatches, matchOffers);

        return (leagues, matches);
    }

    private static (List<PreMatchLeague>, List<InfoSiteMatch>) FilterByMatchOffer(
        List<PreMatchLeague> filteredLeagues,
        List<InfoSiteMatch> filteredMatches,
        List<string> matchOffers)
    {
        var matches = matchOffers is null
            ? filteredMatches
            : filteredMatches.Where(m => matchOffers.Exists(x => x.Equals(m.MatchOffer))).ToList();

        foreach (var league in filteredLeagues)
        {
            league.MatchCount = matches.Count(x => x.LeagueId == league.LeagueId);
        }
        var leagues = filteredLeagues.Where(x => x.MatchCount != 0).ToList();

        return (leagues, matches);
    }

    private static int ConvertBoolToInt(bool value) => value ? 1 : 0;

    public async Task MapMatchStatusWhichLinkedOwp(List<InfoSiteMatch> matches, DateTime dateTime)
    {
        var infositeIds = matches.Select(x => x.InfoSiteMatchId).ToList();
        var linkedOWPMatches = await repository.QueryLinkedOwpMatch(infositeIds, dateTime);
        foreach (var match in matches)
        {
            var linkedMatch = linkedOWPMatches.Find(x => x.InfositeMatchId == match.InfoSiteMatchId) ?? new LinkedMatch();
            match.MatchStatus = GetMatchStatus(linkedMatch, match);
        }
    }

    private QueryPreMatchLeagueViewModel CreateQueryPreMatchLeagueViewModel(List<PreMatchLeague> leagues, Dictionary<int, List<InfoSiteMatch>> matches)
        => new()
        {
            InfoSiteMatches = matches.SelectMany(x => x.Value).ToList(),
            PreMatchLeagues = mapper.Map<IEnumerable<LeagueAssignment>>(leagues)
        };

    private static string GetMatchStatus(LinkedMatch linkedMatch, InfoSiteMatch match)
    {
        if (linkedMatch.MatchStatus == MatchStatus.Live && !match.IsLive)
        {
            return MatchStatus.NotLive.ToUpper(CultureInfo.InvariantCulture);
        }

        return linkedMatch.MatchStatus.ToUpper(CultureInfo.InvariantCulture);
    }

    private static bool CheckMainAssignmentContainsEmployee(PreMatchLeague league, string employee)
    {
        var assignments = new List<string> {
            league.HDP,
            league.HDPNickname,
            league.HDPTakeOver,
            league.HDPTakeOverNickname,
            league.OU,
            league.OUNickname,
            league.OUTakeOver,
            league.OUTakeOverNickname };

        return assignments.Exists(x => x.IsContains(employee));
    }

    private static bool CheckOtherAssignmentContainsEmployee(PreMatchLeague league, string employee)
    {
        var assignments = new List<string> {
            league.OtherHDP,
            league.OtherHDPNickname,
            league.OtherHDPTakeOver,
            league.OtherHDPTakeOverNickname,
            league.OtherOU,
            league.OtherOUNickname,
            league.OtherOUTakeOver,
            league.OtherOUTakeOverNickname };

        return assignments.Exists(x => x.IsContains(employee));
    }

    private static (IList<PreMatchLeague>, IList<PreMatchLeague>) SplitPrematchLeagues(IEnumerable<PreMatchLeague> leagues)
    {
        var leaguesOnlyContainsHint = new List<PreMatchLeague>();
        var leaguesRest = new List<PreMatchLeague>();
        foreach (var league in leagues)
        {
            if (string.IsNullOrWhiteSpace(league.HDP) &&
                string.IsNullOrWhiteSpace(league.OU) &&
                string.IsNullOrWhiteSpace(league.HDPTakeOver) &&
                string.IsNullOrWhiteSpace(league.OUTakeOver))
            {
                leaguesOnlyContainsHint.Add(league);
            }
            else
            {
                leaguesRest.Add(league);
            }
        }

        return (leaguesOnlyContainsHint, leaguesRest);
    }

    private static List<PreMatchLeague> SortByHint(IEnumerable<PreMatchLeague> leagues)
    {
        return leagues
            .OrderByDescending(x => x.HDPHint)
            .ThenBy(x => x.OUHint)
            .ThenBy(x => x.HDPTakeOverHint)
            .ThenBy(x => x.OUTakeOverHint)
            .ThenBy(x => x.LeagueName)
            .ThenBy(x => x.LeagueName)
            .ToList();
    }

    private static List<PreMatchLeague> SortPreMatchLeagues(IEnumerable<PreMatchLeague> leagues)
    {
        return leagues.OrderByDescending(x =>
                   string.IsNullOrEmpty(x.HDP) &&
                   string.IsNullOrEmpty(x.OU) &&
                   string.IsNullOrEmpty(x.HDPTakeOver) &&
                   string.IsNullOrEmpty(x.OUTakeOver))
            .ThenBy(x => string.IsNullOrEmpty(x.HDP) ? string.Empty : x.HDP)
            .ThenBy(x => string.IsNullOrEmpty(x.OU) ? string.Empty : x.OU)
            .ThenBy(x => string.IsNullOrEmpty(x.HDPTakeOver) ? string.Empty : x.HDPTakeOver)
            .ThenBy(x => string.IsNullOrEmpty(x.OUTakeOver) ? string.Empty : x.OUTakeOver)
            .ThenBy(x => x.LeagueName)
            .ToList();
    }

    private static void AddNumber(List<PreMatchLeague> sortedLeagues)
    {
        for (var i = 0; i < sortedLeagues.Count; i++)
        {
            sortedLeagues[i].No = i + 1;
        }
    }

    public async Task AddOddStatusNo(IEnumerable<InfoSiteMatch> infositeMatches)
    {
        var infoSiteMatches = infositeMatches.ToList();
        var oddStatuses = await repository.QueryOddStatusFromMainDb(infoSiteMatches);
        var oddStatusesDict = oddStatuses.GroupBy(x => x.AdminSiteMatchId).ToDictionary(x => x.Key, x => x.ToList());

        foreach (var infositeMatch in infoSiteMatches)
        {
            var oddStatusByMatchId = oddStatusesDict.GetValueOrDefault(infositeMatch.AdminSiteMatchId, []);
            var openedHDP = ShiftHelper.IsRunning(oddStatusByMatchId.Where(x => x.Bettype == 1));
            var openedOU = ShiftHelper.IsRunning(oddStatusByMatchId.Where(x => x.Bettype == 3));

            infositeMatch.MatchOffer = ShiftHelper.CalculateOddStatus(openedHDP, openedOU);
        }
    }
}