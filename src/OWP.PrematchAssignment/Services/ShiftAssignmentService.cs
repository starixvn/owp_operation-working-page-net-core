using Fanex.NAP.Client.Helpers.Web;
using OWP.PrematchAssignment.Models;

namespace OWP.PreMatchAssignment.Services
{
    using AutoMapper;
    using Constants;
    using Criteria;
    using Fanex.Data.Repository;
    using Handlers;
    using Models;
    using OWP.Extensions;

    using System.Globalization;

    public interface ShiftAssignmentService
    {
        Task<QueryPreMatchLeagueViewModel> QueryDayShift(QueryDayShiftPreMatchRequest request);

        Task<QueryPreMatchLeagueViewModel> QueryNightShift(QueryNightShiftPreMatchRequest request);

        Task MapTicketLists(QueryPreMatchLeagueViewModel shiftAssignments, int actorId);
    }

    public class ShiftAssignmentServiceImp : ShiftAssignmentService
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly INapSession currentSession;

        private readonly IMapper mapper;

        private class AssignmentShift
        {
            public IEnumerable<PreMatchLeague> Leagues { get; set; }

            public IEnumerable<InfoSiteMatch> Matches { get; set; }
        }

        private class DefaultDayAndNightShift
        {
            public AssignmentShift DayShift { get; set; }

            public AssignmentShift NightShift { get; set; }
        }

        public ShiftAssignmentServiceImp(IDynamicRepository dynamicRepository, INapSession currentSession, IMapper mapper)
        {
            this.dynamicRepository = dynamicRepository;
            this.currentSession = currentSession;
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

            var result = await QueryShift(queryDayShiftCriteria, request.Employee, request.LeagueName, request.EventDate);

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

            var dayShift = await queryAssignmentShift(request, request.StartTime, true);
            var nightShiftWithRequestStartTime = await queryAssignmentShift(request, request.StartTime, false);
            var defaultShift = new DefaultDayAndNightShift
            {
                DayShift = dayShift,
                NightShift = nightShiftWithRequestStartTime
            };

            if (IsRequestStartTimeGreaterThanDBStartTime(request, startTime))
            {
                return QueryNightShiftWithoutDBStartTime(defaultShift, startTime);
            }

            return await QueryNightShiftWithDBStartTime(defaultShift, request, startTime);
        }

        public async Task MapTicketLists(QueryPreMatchLeagueViewModel shiftAssignments, int actorId)
        {
            var leagueIds = shiftAssignments.PreMatchLeagues.Select(x => x.LeagueId);
            var infoSiteMatchIds = shiftAssignments.InfoSiteMatches.Select(x => x.InfoSiteMatchId);

            var leagueTickets = await QueryAllLeaguesTicket(leagueIds, actorId);
            var matchTickets = await QueryAllMatchesTicket(infoSiteMatchIds, actorId);

            foreach (var league in shiftAssignments.PreMatchLeagues)
            {
                var leagueTicket = leagueTickets.GetValue(league.LeagueId, new List<TicketLeagueBasicInfo>());
                league.TicketList = leagueTicket.OrderByDescending(x => x.LatestCreatedDate).ToList();
            }

            foreach (var match in shiftAssignments.InfoSiteMatches)
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

        private static bool IsRequestStartTimeGreaterThanDBStartTime(QueryNightShiftPreMatchRequest request, TimeSpan? startTime)
        => !startTime.HasValue || IsDBStartTimeLowerThanRequestStartTime(startTime, request.StartTime);

        private QueryPreMatchLeagueViewModel QueryNightShiftWithoutDBStartTime(DefaultDayAndNightShift defaultShifts, TimeSpan? startTime)

           => GetNightShiftResultWithoutDBStartTime(defaultShifts.NightShift, defaultShifts.DayShift, startTime);

        private async Task<QueryPreMatchLeagueViewModel> QueryNightShiftWithDBStartTime(DefaultDayAndNightShift defaultShifts, QueryNightShiftPreMatchRequest request, TimeSpan? startTime)
        {
            var nightShiftWithDBStartTime = await queryAssignmentShift(request, startTime.Value, false);

            return GetNightShiftResultWithDBStartTime(defaultShifts.NightShift, defaultShifts.DayShift, nightShiftWithDBStartTime);
        }

        private async Task<AssignmentShift> queryAssignmentShift(QueryNightShiftPreMatchRequest request, TimeSpan startTime, bool isDayShift)
        {
            var queryCriteria = CreateNightShiftCriteria(request.EventDate, startTime, isDayShift);
            var result = await QueryShift(queryCriteria, request.Employee, request.LeagueName, request.EventDate);

            return new AssignmentShift { Leagues = result.Item1, Matches = result.Item2 };
        }

        private static bool IsDBStartTimeLowerThanRequestStartTime(TimeSpan? dbStartTime, TimeSpan requestStartTime)
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

        private async Task<TimeSpan?> QueryExistedStartTime(DateTime EventDate)
        {
            var criteria = new GetStartTimeCriteria
            {
                EventDate = EventDate
            };
            var result = await dynamicRepository.FetchAsync<TimeSpan>(criteria);

            return result.Any() ? new TimeSpan?(result.FirstOrDefault()) : null;
        }

        private static CriteriaBase CreateNightShiftCriteria(DateTime eventDate, TimeSpan? startTime, bool IsDay)
        {
            var eventToDateTime = eventDate.AddDays(1).AddHours(11).AddMinutes(59).AddSeconds(59);
            var eventToRequestStartTime = CalculateQueryDateTime(startTime.Value, eventDate);

            return new QueryShiftPreMatchCriteria
            {
                EventDate = eventDate,
                EventToDateTime = eventToDateTime,
                EventFromDateTime = eventToRequestStartTime,
                IsDay = IsDay
            };
        }

        private QueryPreMatchLeagueViewModel GetNightShiftResultWithoutDBStartTime(
            AssignmentShift nightShiftWithRequestStartTime,
            AssignmentShift dayShift,
            TimeSpan? startTime)
        {
            var nightShiftWithRequestStartTimeLeagues = nightShiftWithRequestStartTime.Leagues;

            if (!startTime.HasValue)
            {
                var dayShiftLeagues = dayShift.Leagues;
                var dayShiftLeaguesDict = dayShiftLeagues.ToDictionary(x => x.LeagueId);

                nightShiftWithRequestStartTimeLeagues = nightShiftWithRequestStartTimeLeagues
                    .Select(x => { return copyAssignment(x, dayShiftLeaguesDict[x.LeagueId]); });
            }

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
            var nightShiftWithDBStartTimeLeagues = nightShiftWithDBStartTime.Leagues;
            var newLeagues = dayShiftLeagues
                .Where(x => nightShiftWithDBStartTimeLeagues
                    .All(y => y.LeagueId != x.LeagueId) &&
                    nightShiftWithRequestStartTimeLeagues.Any(z => z.LeagueId == x.LeagueId)).ToList();

            foreach (var league in newLeagues)
            {
                nightShiftWithRequestStartTimeLeagues = nightShiftWithRequestStartTimeLeagues
                    .Select(x =>
                    {
                        if (x.LeagueId == league.LeagueId)
                        {
                            return copyAssignment(x, league);
                        }

                        return x;
                    });
            }

            var preMatchLeagues = SortAndAddNo(nightShiftWithRequestStartTimeLeagues);

            return new QueryPreMatchLeagueViewModel
            {
                PreMatchLeagues = mapper.Map<IList<LeagueAssignment>>(preMatchLeagues),
                InfoSiteMatches = nightShiftWithRequestStartTime.Matches.ToList()
            };
        }

        private static PreMatchLeague copyAssignment(PreMatchLeague des, PreMatchLeague source)
        {
            des.HDP = source.HDP;
            des.OU = source.OU;
            des.HDPTakeOver = source.HDPTakeOver;
            des.OUTakeOver = source.OUTakeOver;
            des.OtherHDP = source.OtherHDP;
            des.OtherOU = source.OtherOU;
            des.OtherHDPTakeOver = source.OtherHDPTakeOver;
            des.OtherOUTakeOver = source.OtherOUTakeOver;

            return des;
        }

        private static DateTime CalculateQueryDateTime(TimeSpan time, DateTime eventDate)
        {
            if (time.CompareTo(new TimeSpan(0, 0, 0)) >= 0 && time.CompareTo(new TimeSpan(11, 59, 59)) <= 0)
            {
                return eventDate.Date.AddDays(1).Add(time);
            }

            return eventDate.Date.Add(time);
        }

        private async Task<Tuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>>> QueryShift(CriteriaBase criteria, string employee, string leagueName, DateTime eventDate)
        {
            var data = await dynamicRepository.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(criteria);

            var filterByEmployee = data.Item1.Where(x => string.IsNullOrEmpty(employee)
                                                         || CheckMainAssignmentContainsEmployee(x, employee)
                                                         || CheckOtherAssignmentContainsEmployee(x, employee));
            var filterByLeagueName = filterByEmployee.Where(x => x.LeagueName.IsContains(leagueName));

            var matches = data.Item2.Where(x => filterByLeagueName.Any(y => y.PreMatchLeagueId == x.PreMatchLeagueId));

            await MapMatchStatusWhichLinkedOWP(matches, eventDate);

            return new Tuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>>(filterByLeagueName, matches);
        }

        public async Task MapMatchStatusWhichLinkedOWP(IEnumerable<InfoSiteMatch> matches, DateTime dateTime)
        {
            var infositeIds = matches.Select(x => x.InfoSiteMatchId).ToList();
            var linkedOWPMatches = await QueryLinkedOWPMatch(infositeIds, dateTime);
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

        private async Task<IEnumerable<LinkedMatch>> QueryLinkedOWPMatch(List<int> infositeIds, DateTime eventDate)
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
    }
}