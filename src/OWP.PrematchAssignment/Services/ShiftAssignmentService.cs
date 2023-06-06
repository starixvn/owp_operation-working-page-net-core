using AutoMapper;
using OWP.Extensions;
using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Helper;
using OWP.PrematchAssignment.Models;
using System.Globalization;

namespace OWP.PrematchAssignment.Services;

public interface ShiftAssignmentService
{
    Task<QueryPreMatchLeagueViewModel> QueryDayShift(QueryDayShiftPreMatchRequest request);

    Task<QueryPreMatchLeagueViewModel> QueryNightShift(QueryNightShiftPreMatchRequest request);

    Task MapTicketLists(List<LeagueAssignment> preMatchLeagues, List<InfoSiteMatch> infoSiteMatches, int actorId);
}

public class ShiftAssignmentServiceImp : ShiftAssignmentService
{
    private readonly Repository repository;

    private readonly IMapper mapper;

    public ShiftAssignmentServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<QueryPreMatchLeagueViewModel> QueryDayShift(QueryDayShiftPreMatchRequest request)
    {
        var criteria = ShiftHelper.CreateShiftCriteria(request.EventDate, true);
        var assignmentShift = await BuildPreMatchAssignments(request, criteria);

        return CreateQueryPreMatchLeagueViewModel(assignmentShift.Leagues.Values.ToList(), assignmentShift.Matches);
    }

    private async Task<AssignmentShift> BuildPreMatchAssignments(QueryDayShiftPreMatchRequest request, QueryShiftPreMatchCriteria criteria)
    {
        var result = await repository.QueryDayShiftPreMatchAssignments(criteria);

        var (filteredLeagues, filteredInfoSiteMatches) = FilterByRequest(result.Item1, result.Item2, request);

        await MapMatchStatusWhichLinkedOwp(filteredInfoSiteMatches, request.EventDate);
        await AddOddStatusNo(filteredInfoSiteMatches);
        filteredLeagues = SortPreMatchLeaguesAndAddNumber(filteredLeagues);

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

        return GetNightShiftWithRequestTime(nightShiftInfo.NightShiftsRequestTime, nightShiftInfo.DayShifts);

        //if (!nightShiftInfo.DBStartTime.HasValue ||
        //    ShiftHelper.IsDbStartTimeLowerThanRequestStartTime(nightShiftInfo.DBStartTime.Value, request.StartTime))
        //{
        //    return GetNightShiftWithRequestTime(nightShiftInfo.NightShiftsRequestTime, nightShiftInfo.DayShifts);
        //}

        // lose data
        //return await GetNightShiftResultWithDBStartTime(nightShiftInfo);
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
            NightShiftsRequestTime = nightAssignmentsWithRequestTime.Result,
            DBStartTime = dbStartTime.Result
        };

        //if (!dbStartTime.Result.HasValue)
        //{
        //    return nightShiftInfo;
        //}

        //var nightDbTimeCriteria = ShiftHelper.CreateShiftCriteria(request.EventDate, false, dbStartTime.Result.Value);
        //nightShiftInfo.NightShiftsDBTime = await BuildPreMatchAssignments(request, nightDbTimeCriteria);

        return nightShiftInfo;
    }

    public async Task MapTicketLists(List<LeagueAssignment> preMatchLeagues, List<InfoSiteMatch> infoSiteMatches, int actorId)
    {
        var leagueIds = preMatchLeagues.Select(x => x.LeagueId);
        var infoSiteMatchIds = infoSiteMatches.Select(x => x.InfoSiteMatchId);

        var leagueTickets = await repository.QueryAllLeaguesTicket(leagueIds, actorId);
        var matchTickets = await repository.QueryAllMatchesTicket(infoSiteMatchIds, actorId);

        foreach (var league in preMatchLeagues)
        {
            var leagueTicket = leagueTickets.GetValue(league.LeagueId, new List<TicketLeagueBasicInfo>());
            league.TicketList = leagueTicket.OrderByDescending(x => x.LatestCreatedDate).ToList();
        }

        foreach (var match in infoSiteMatches)
        {
            var matchTicket = matchTickets.GetValue(match.InfoSiteMatchId, new List<TicketMatchBasicInfo>());
            match.TicketList = matchTicket.OrderByDescending(x => x.LatestCreatedDate).ToList();
        }
    }

    private QueryPreMatchLeagueViewModel GetNightShiftWithRequestTime(AssignmentShift nightShift, AssignmentShift dayShift)
    {
        UpdateAssignmentHintNightShift(nightShift.Leagues, dayShift.Leagues);

        var (leaguesContainsHint, leaguesRest) = SplitPrematchLeagues(nightShift.Leagues.Values.ToList());
        var preMatchLeagues = SortByHint(leaguesContainsHint);
        preMatchLeagues.AddRange(SortPreMatchLeaguesAndAddNumber(leaguesRest));

        return CreateQueryPreMatchLeagueViewModel(preMatchLeagues, nightShift.Matches);
    }

    //private QueryPreMatchLeagueViewModel GetNightShiftResultWithDBStartTime(NightShiftInfo nightShiftInfo)
    //{
    //    var leagues = nightShiftInfo.NightShiftsRequestTime.Leagues;
    //    var dayShiftLeagues = nightShiftInfo.DayShifts.Leagues;
    //    var nightShiftWithDbStartTimeLeagues = nightShiftInfo.NightShiftsDBTime.Leagues;

    //    UpdateAssignmentHintNightShift(leagues, dayShiftLeagues);

    //    var newLeagues = dayShiftLeagues
    //        .Where(x =>
    //            nightShiftWithDbStartTimeLeagues.All(y => y.Key != x.Key) &&
    //            leagues.Any(z => z.Key == x.Key)
    //        ).ToList();

    //    foreach (var league in newLeagues)
    //    {
    //        leagues = leagues
    //            .Select(x => x.LeagueId == league.LeagueId
    //                ? UpdateAssignmentHint(x, league)
    //                : x
    //            );
    //    }

    //    //leagues = newLeagues.Aggregate(leagues, (current, newLeague) => current.Select(x => x.LeagueId == newLeague.LeagueId
    //    //        ? UpdateAssignmentHint(x, newLeague)
    //    //        : x
    //    //    ).ToList());

    //    var (leaguesContainsHint, leaguesRest) = SplitPrematchLeagues(leagues);
    //    var preMatchLeagues = SortByHint(leaguesContainsHint);
    //    preMatchLeagues.AddRange(SortPreMatchLeaguesAndAddNumber(leaguesRest));

    //    return new QueryPreMatchLeagueViewModel
    //    {
    //        PreMatchLeagues = mapper.Map<IList<LeagueAssignment>>(preMatchLeagues),
    //        InfoSiteMatches = nightShiftInfo.NightShiftsRequestTime.Matches.SelectMany(x => x.Value)
    //    };
    //}

    private static void UpdateAssignmentHintNightShift(Dictionary<int, PreMatchLeague> nightShiftLeagues, Dictionary<int, PreMatchLeague> dayShiftLeagues)
    {
        foreach (var nightShiftLeague in nightShiftLeagues)
        {
            var dayShiftLeague = dayShiftLeagues.GetValue(nightShiftLeague.Key, null);
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
            .Where(x => string.IsNullOrEmpty(employee) ||
                                    CheckMainAssignmentContainsEmployee(x, employee) ||
                                    CheckOtherAssignmentContainsEmployee(x, employee))
            .Where(x => x.LeagueName.IsContains(request.LeagueName));

        var filteredMatches = infoSiteMatches
            .Where(x => filteredLeagues.Any(y => y.PreMatchLeagueId == x.PreMatchLeagueId));

        filteredMatches = matchOffers is null
            ? filteredMatches
            : filteredMatches.Where(m => matchOffers.Any(x => x.Equals(m.MatchOffer)));

        return (filteredLeagues.ToList(), filteredMatches.ToList());
    }

    public async Task MapMatchStatusWhichLinkedOwp(List<InfoSiteMatch> matches, DateTime dateTime)
    {
        var infositeIds = matches.Select(x => x.InfoSiteMatchId).ToList();
        var linkedOWPMatches = await repository.QueryLinkedOwpMatch(infositeIds, dateTime);
        foreach (var match in matches)
        {
            var linkedMatch = linkedOWPMatches.FirstOrDefault(x => x.InfositeMatchId == match.InfoSiteMatchId) ?? new LinkedMatch();
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

        return assignments.Any(x => x.IsContains(employee));
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

        return assignments.Any(x => x.IsContains(employee));
    }

    private static (IList<PreMatchLeague>, IList<PreMatchLeague>) SplitPrematchLeagues(IList<PreMatchLeague> leagues)
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

    private static List<PreMatchLeague> SortPreMatchLeaguesAndAddNumber(IEnumerable<PreMatchLeague> leagues)
    {
        var sortedLeagues = leagues.OrderByDescending(x =>
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

        for (var i = 0; i < sortedLeagues.Count; i++)
        {
            sortedLeagues[i].No = i + 1;
        }

        return sortedLeagues;
    }

    public async Task AddOddStatusNo(IEnumerable<InfoSiteMatch> infositeMatches)
    {
        var infoSiteMatches = infositeMatches.ToList();
        var oddStatuses = await repository.QueryOddStatusFromMainDb(infoSiteMatches);
        var oddStatusesDict = oddStatuses.GroupBy(x => x.AdminSiteMatchId).ToDictionary(x => x.Key, x => x.ToList());

        foreach (var infositeMatch in infoSiteMatches)
        {
            var oddStatusByMatchId = oddStatusesDict.GetValue(infositeMatch.AdminSiteMatchId, new List<OddStatus>());
            var openedHDP = ShiftHelper.IsRunning(oddStatusByMatchId.Where(x => x.Bettype == 1));
            var openedOU = ShiftHelper.IsRunning(oddStatusByMatchId.Where(x => x.Bettype == 3));

            infositeMatch.MatchOffer = ShiftHelper.CalculateOddStatus(openedHDP, openedOU);
        }
    }
}