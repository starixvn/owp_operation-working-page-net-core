namespace OWP.PreMatchAssignment.Services;

using System.Globalization;
using AutoMapper;
using Constants;
using Criteria;
using Extensions;
using Fanex.Data.Repository;
using Handlers;
using Models;
using PrematchAssignment.Constants;
using PrematchAssignment.Models;

public interface ShiftAssignmentService
{
    Task<QueryPreMatchLeagueViewModel> QueryDayShift(QueryDayShiftPreMatchRequest request);

    Task<QueryPreMatchLeagueViewModel> QueryNightShift(QueryNightShiftPreMatchRequest request);

    Task MapTicketLists(IEnumerable<LeagueAssignment> preMatchLeagues, IEnumerable<InfoSiteMatch> infoSiteMatches, int actorId);

    Task<TimeSpan?> QueryExistedStartTime(DateTime eventDate);

    CriteriaBase CreateNightShiftCriteria(DateTime eventDate, TimeSpan? startTime, bool isDay);
}

public class ShiftAssignmentServiceImp : ShiftAssignmentService
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly IMapper mapper;

    public ShiftAssignmentServiceImp(IDynamicRepository dynamicRepository, IMapper mapper)
    {
        this.dynamicRepository = dynamicRepository;
        this.mapper = mapper;
    }

    public async Task<QueryPreMatchLeagueViewModel> QueryDayShift(QueryDayShiftPreMatchRequest request)
    {
        var queryDayShiftCriteria = new QueryShiftPreMatchCriteria
        {
            EventDate = request.EventDate,
            EventToDateTime = request.EventDate.AddDays(1).AddHours(11).AddMinutes(59).AddSeconds(59),
            EventFromDateTime = request.EventDate.AddHours(12),
            IsDay = true
        };

        var result = await QueryShift(queryDayShiftCriteria, request.Employee, request.LeagueName, request.MatchOffers, request.EventDate);

        var preMatchLeagues = SortAndAddNo(result.Item1);

        return new QueryPreMatchLeagueViewModel
        {
            InfoSiteMatches = result.Item2.ToList(),
            PreMatchLeagues = mapper.Map<IList<LeagueAssignment>>(preMatchLeagues)
        };
    }

    public async Task<QueryPreMatchLeagueViewModel> QueryNightShift(QueryNightShiftPreMatchRequest request)
    {
        var startTime = await QueryExistedStartTime(request.EventDate);

        var dayShift = await QueryAssignmentShift(request, request.StartTime, true);

        var nightShiftWithRequestStartTime = await QueryAssignmentShift(request, request.StartTime, false);
        var defaultShift = new DefaultDayAndNightShift
        {
            DayShift = dayShift,
            NightShift = nightShiftWithRequestStartTime
        };

        return IsRequestStartTimeGreaterThanDbStartTime(request, startTime)
            ? GetNightShiftResultWithoutDbStartTime(defaultShift.NightShift, defaultShift.DayShift, startTime)
            : await QueryNightShiftWithDbStartTime(defaultShift, request, startTime);
    }

    public async Task MapTicketLists(IEnumerable<LeagueAssignment> preMatchLeagues, IEnumerable<InfoSiteMatch> infoSiteMatches, int actorId)
    {
        var leagueIds = preMatchLeagues.Select(x => x.LeagueId);
        var infoSiteMatchIds = infoSiteMatches.Select(x => x.InfoSiteMatchId);

        var leagueTickets = await QueryAllLeaguesTicket(leagueIds, actorId);
        var matchTickets = await QueryAllMatchesTicket(infoSiteMatchIds, actorId);

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

    public async Task<Dictionary<int, List<TicketLeagueBasicInfo>>> QueryAllLeaguesTicket(IEnumerable<int> owpLeagueIds, int actorId)
    {
        var criteria = new QueryAllLeaguesTicketCriteria
        {
            OWPLeagueIds = string.Join(",", owpLeagueIds.Distinct()),
            ActorId = actorId
        };

        var result = await dynamicRepository.FetchAsync<TicketLeagueBasicInfo>(criteria);

        return result
            .GroupBy(x => x.OWPLeagueId)
            .ToDictionary(x => x.Key, x => x.ToList());
    }

    public async Task<Dictionary<int, List<TicketMatchBasicInfo>>> QueryAllMatchesTicket(IEnumerable<int> infoSiteMatchIds, int actorId)
    {
        var criteria = new QueryAllMatchesTicketCriteria
        {
            InfoSiteMatchIds = string.Join(",", infoSiteMatchIds.Distinct()),
            ActorId = actorId
        };

        var result = await dynamicRepository.FetchAsync<TicketMatchBasicInfo>(criteria);

        return result
            .GroupBy(x => x.InfoSiteMatchId)
            .ToDictionary(x => x.Key, x => x.ToList());
    }

    private static bool IsRequestStartTimeGreaterThanDbStartTime(QueryNightShiftPreMatchRequest request, TimeSpan? startTime)
        => !startTime.HasValue || IsDbStartTimeLowerThanRequestStartTime(startTime, request.StartTime);

    private async Task<QueryPreMatchLeagueViewModel> QueryNightShiftWithDbStartTime(DefaultDayAndNightShift defaultShifts, QueryNightShiftPreMatchRequest request, TimeSpan? startTime)
    {
        var nightShiftWithDbStartTime = await QueryAssignmentShift(request, startTime.Value, false);

        return GetNightShiftResultWithDBStartTime(defaultShifts.NightShift, defaultShifts.DayShift, nightShiftWithDbStartTime);
    }

    private async Task<AssignmentShift> QueryAssignmentShift(QueryNightShiftPreMatchRequest request, TimeSpan startTime, bool isDayShift)
    {
        var queryCriteria = CreateNightShiftCriteria(request.EventDate, startTime, isDayShift);
        var result = await QueryShift(queryCriteria, request.Employee, request.LeagueName, request.MatchOffers, request.EventDate);

        return new AssignmentShift { Leagues = result.Item1, Matches = result.Item2 };
    }

    private static bool IsDbStartTimeLowerThanRequestStartTime(TimeSpan? dbStartTime, TimeSpan requestStartTime)
    {
        var twelvePm = new TimeSpan(12, 00, 00);

        var dbStartTimeDateTime = dbStartTime.Value.CompareTo(twelvePm) >= 0
            ? DateTime.Now.Date.Add(dbStartTime.Value)
            : DateTime.Now.AddDays(1).Date.Add(dbStartTime.Value);

        var requestStartTimeDateTime = requestStartTime.CompareTo(twelvePm) >= 0
            ? DateTime.Now.Date.Add(requestStartTime)
            : DateTime.Now.AddDays(1).Date.Add(requestStartTime);

        return dbStartTimeDateTime.CompareTo(requestStartTimeDateTime) <= 0;
    }

    public async Task<TimeSpan?> QueryExistedStartTime(DateTime eventDate)
    {
        var criteria = new GetStartTimeCriteria
        {
            EventDate = eventDate
        };
        var result = await dynamicRepository.FetchAsync<TimeSpan>(criteria);

        return result.Any() ? new TimeSpan?(result.FirstOrDefault()) : null;
    }

    public CriteriaBase CreateNightShiftCriteria(DateTime eventDate, TimeSpan? startTime, bool isDay)
    {
        var eventToDateTime = eventDate.AddDays(1).AddHours(11).AddMinutes(59).AddSeconds(59);
        var eventToRequestStartTime = CalculateQueryDateTime(startTime.Value, eventDate);

        return new QueryShiftPreMatchCriteria
        {
            EventDate = eventDate,
            EventToDateTime = eventToDateTime,
            EventFromDateTime = eventToRequestStartTime,
            IsDay = isDay
        };
    }

    private QueryPreMatchLeagueViewModel GetNightShiftResultWithoutDbStartTime(
        AssignmentShift nightShiftWithRequestStartTime,
        AssignmentShift dayShift,
        TimeSpan? startTime)
    {
        var nightShiftWithRequestStartTimeLeagues = nightShiftWithRequestStartTime.Leagues;

        if (!startTime.HasValue)
        {
            var dayShiftLeagues = dayShift.Leagues;
            var dayShiftLeaguesPair = dayShiftLeagues.ToDictionary(x => x.LeagueId);

            nightShiftWithRequestStartTimeLeagues = nightShiftWithRequestStartTimeLeagues
                .Select(x => UpdateAssignmentHint(x, dayShiftLeaguesPair[x.LeagueId]));
        }
        UpdateAssignmentHintNightShift(nightShiftWithRequestStartTimeLeagues, dayShift.Leagues);
        var preMatchLeagues = SortAndAddNo(nightShiftWithRequestStartTimeLeagues);


        return new QueryPreMatchLeagueViewModel
        {
            InfoSiteMatches = nightShiftWithRequestStartTime.Matches.ToList(),
            PreMatchLeagues = mapper.Map<IList<LeagueAssignment>>(preMatchLeagues)
        };
    }

    private QueryPreMatchLeagueViewModel GetNightShiftResultWithDBStartTime(
        AssignmentShift nightShiftWithRequestStartTime,
        AssignmentShift dayShift,
        AssignmentShift nightShiftWithDBStartTime
    )
    {
        var nightShiftWithRequestStartTimeLeagues = nightShiftWithRequestStartTime.Leagues;
        var dayShiftLeagues = dayShift.Leagues;
        var nightShiftWithDbStartTimeLeagues = nightShiftWithDBStartTime.Leagues;

        var newLeagues = dayShiftLeagues
            .Where(x => nightShiftWithDbStartTimeLeagues
                            .All(y => y.LeagueId != x.LeagueId) &&
                        nightShiftWithRequestStartTimeLeagues.Any(z => z.LeagueId == x.LeagueId)).ToList();

        UpdateAssignmentHintNightShift(nightShiftWithRequestStartTimeLeagues, dayShiftLeagues);

        foreach (var league in newLeagues)
        {
            nightShiftWithRequestStartTimeLeagues = nightShiftWithRequestStartTimeLeagues
                .Select(x => x.LeagueId == league.LeagueId
                    ? UpdateAssignmentHint(x, league)
                    : x
                );
        }

        var preMatchLeagues = SortAndAddNo(nightShiftWithRequestStartTimeLeagues);

        return new QueryPreMatchLeagueViewModel
        {
            PreMatchLeagues = mapper.Map<IList<LeagueAssignment>>(preMatchLeagues),
            InfoSiteMatches = nightShiftWithRequestStartTime.Matches
        };
    }

    private void UpdateAssignmentHintNightShift(IEnumerable<PreMatchLeague> nightShiftLeagues, IEnumerable<PreMatchLeague> dayShiftLeagues)
    {

        foreach (var nightShiftLeague in nightShiftLeagues)
        {

            var dayShiftLeague = dayShiftLeagues?.FirstOrDefault(x => Equals(x.LeagueId, nightShiftLeague.LeagueId));

            if (dayShiftLeague is not null)
            {
                UpdateAssignmentHint(nightShiftLeague, dayShiftLeague);
            }
        }
    }

    private static PreMatchLeague UpdateAssignmentHint(PreMatchLeague dest, PreMatchLeague source)
    {
        dest.HDPHint = source.HDP;
        dest.OtherHDPHint = source.OtherHDP;
        dest.OUHint = source.OU;
        dest.OtherOUHint = source.OtherOU;
        dest.HDPTakeOverHint = source.HDPTakeOver;
        dest.OtherHDPTakeOverHint = source.OtherHDPTakeOver;
        dest.OUTakeOverHint = source.OUTakeOver;
        dest.OtherOUTakeOverHint = source.OtherOUTakeOver;
        return dest;
    }

    private static DateTime CalculateQueryDateTime(TimeSpan time, DateTime eventDate)
    {
        if (time.CompareTo(new TimeSpan(0, 0, 0)) >= 0 && time.CompareTo(new TimeSpan(11, 59, 59)) <= 0)
        {
            return eventDate.Date.AddDays(1).Add(time);
        }

        return eventDate.Date.Add(time);
    }

    private async Task<Tuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>>> QueryShift(CriteriaBase criteria,
        string employee, string leagueName, List<string> matchOffers, DateTime eventDate)
    {
        var data = await dynamicRepository.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(criteria);

        var filterByEmployee = data.Item1.Where(x => string.IsNullOrEmpty(employee)
                                                     || CheckMainAssignmentContainsEmployee(x, employee)
                                                     || CheckOtherAssignmentContainsEmployee(x, employee));
        var filterByLeagueName = filterByEmployee.Where(x => x.LeagueName.IsContains(leagueName));

        var matches = data.Item2.Where(x => filterByLeagueName.Any(y => y.PreMatchLeagueId == x.PreMatchLeagueId));

        await MapMatchStatusWhichLinkedOwp(matches, eventDate);

        await AddOddStatusNo(matches);

        var matchesFilterByMatchOffer = matchOffers is null ? matches : FilterByMatchOffer(matches, matchOffers);

        var preMatchLeagueIds = matchesFilterByMatchOffer.Select(m => m.PreMatchLeagueId).Distinct().ToList();

        var leaguesFilterByMatch = filterByLeagueName.Where(l => preMatchLeagueIds.Contains(l.PreMatchLeagueId));

        return new Tuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>>(leaguesFilterByMatch, matchesFilterByMatchOffer);
    }

    private static IEnumerable<InfoSiteMatch> FilterByMatchOffer(IEnumerable<InfoSiteMatch> matches, List<string> matchOffers)
        => matches?.Where(m => matchOffers.Any(x => x.Equals(m.MatchOffer)));

    public async Task MapMatchStatusWhichLinkedOwp(IEnumerable<InfoSiteMatch> matches, DateTime dateTime)
    {
        var infositeIds = matches.Select(x => x.InfoSiteMatchId).ToList();
        var linkedOWPMatches = await QueryLinkedOwpMatch(infositeIds, dateTime);
        foreach (var match in matches)
        {
            var linkedMatch = linkedOWPMatches.FirstOrDefault(x => x.InfositeMatchId == match.InfoSiteMatchId) ?? new LinkedMatch();
            match.MatchStatus = GetMatchStatus(linkedMatch, match);
        }
    }

    private static string GetMatchStatus(LinkedMatch linkedMatch, InfoSiteMatch match)
    {
        if (linkedMatch.MatchStatus == MatchStatus.Live && !match.IsLive)
        {
            return MatchStatus.NotLive.ToUpper(CultureInfo.InvariantCulture);
        }

        return linkedMatch.MatchStatus.ToUpper(CultureInfo.InvariantCulture);
    }

    private async Task<IEnumerable<LinkedMatch>> QueryLinkedOwpMatch(List<int> infositeIds, DateTime eventDate)
    {
        var criteria = new QueryLinkedOWPMatchCriteria
        {
            InfositeMatchIds = string.Join(",", infositeIds),
            EventDate = eventDate
        };

        return await dynamicRepository.FetchAsync<LinkedMatch>(criteria);
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

    private static IList<PreMatchLeague> SortAndAddNo(IEnumerable<PreMatchLeague> leagues)
    {
        var sortedLeagues = leagues.OrderByDescending(x => string.IsNullOrEmpty(x.HDP) &&
                                                           string.IsNullOrEmpty(x.OU) &&
                                                           string.IsNullOrEmpty(x.HDPTakeOver) &&
                                                           string.IsNullOrEmpty(x.OUTakeOver))
            .ThenBy(x => string.IsNullOrEmpty(x.HDP))
            .ThenBy(x => x.HDP)
            .ThenBy(x => string.IsNullOrEmpty(x.OU))
            .ThenBy(x => x.OU)
            .ThenBy(x => string.IsNullOrEmpty(x.HDPTakeOver))
            .ThenBy(x => x.HDPTakeOver)
            .ThenBy(x => string.IsNullOrEmpty(x.OUTakeOver))
            .ThenBy(x => x.OUTakeOver)
            .ThenBy(x => x.LeagueName).ToList();

        for (var i = 0; i < sortedLeagues.Count; i++)
        {
            sortedLeagues[i].No = i + 1;
        }

        return sortedLeagues;
    }

    private async Task<IEnumerable<OddStatus>> QueryOddStatusFromMainDb(IEnumerable<InfoSiteMatch> infositeMatches)
    {
        var adminSiteMatchIds = infositeMatches.Select(x => x.AdminSiteMatchId).Distinct();
        var criteria = new QueryOddStatusFromMainDBCriteria
        {
            AdminSiteMatchIds = string.Join(",", adminSiteMatchIds)
        };

        return await dynamicRepository.FetchAsync<OddStatus>(criteria);
    }

    public async Task AddOddStatusNo(IEnumerable<InfoSiteMatch> infositeMatches)
    {
        var oddStatuses = await QueryOddStatusFromMainDb(infositeMatches);
        var oddStatusesDict = oddStatuses.GroupBy(x => x.AdminSiteMatchId).ToDictionary(x => x.Key, x => x.ToList());

        foreach (var infositeMatch in infositeMatches)
        {
            var oddStatusByMatchId = oddStatusesDict.GetValue(infositeMatch.AdminSiteMatchId, new List<OddStatus>());
            var openedHDP = IsRunning(oddStatusByMatchId.Where(x => x.Bettype == 1));
            var openedOU = IsRunning(oddStatusByMatchId.Where(x => x.Bettype == 3));

            infositeMatch.MatchOffer = CalculateOddStatus(openedHDP, openedOU);
        }
    }

    private static bool IsRunning(IEnumerable<OddStatus> oddStatuses)
        => oddStatuses.Any(x => string.Equals(x.OddsStatus, "running", StringComparison.OrdinalIgnoreCase));

    private static string CalculateOddStatus(bool openedHDP, bool openedOU)
        => (openedHDP, openedOU) switch
        {
            (true, true) => MatchOffferStatus.Opened,
            (false, true) => MatchOffferStatus.Hdp,
            (true, false) => MatchOffferStatus.Ou,
            _ => string.Empty
        };
}