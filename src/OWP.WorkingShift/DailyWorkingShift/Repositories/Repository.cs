using OWP.WorkingShift.DailyWorkingShift.Criteria;


namespace OWP.WorkingShift.DailyWorkingShift.Repositories;

public interface Repository
{
    Task<QueryResult> Query(QueryModel model);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSettings napSettings;
    private readonly List<int> ExcludeMinorBettypes = [-1, -2, -5, -8, -88, -89];
    private readonly List<int> SpecialMainMatchBettypes = [-1, -2, -88, -89];
    private const int LABettype = -1002;
    private const int NoOfCornerBettype = -3;
    private const int TotalBookingBettype = -6;
    private const int Specific15MinsNoOfCornersBettype = -5;
    private const int Specific15MinsTotalBookingBettype = -8;

    public RepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
    }

    public async Task<QueryResult> Query(QueryModel model)
    {
        var criteria = new QueryCriteria
        {
            SiteId = napSettings.SiteId,
            ShiftId = model.ShiftId,
            Date = model.Date,
            ExcludeClosedAccount = model.ExcludeClosedAccount,
            LocationCode = model.LocationCode,
            SectionId = model.SectionId,
            DepartmentId = model.DepartmentId,
            User = model.User
        };


        var mappingTypes = new[]
        {
            typeof(QueryUser), typeof(QueryUserShift), typeof(QueryUserMatch), typeof(QueryMatchInfo),
            typeof(EmployeeWorkingShiftNote)
        };
        var fetchedData = await dynamicRepository.FetchMultipleAsync(criteria, mappingTypes);
        var users = fetchedData[0].Cast<QueryUser>();
        var userShifts = fetchedData[1].Cast<QueryUserShift>();

        var userMatches = fetchedData[2].Cast<QueryUserMatch>();

        var userMatchInfos = fetchedData[3].Cast<QueryMatchInfo>().AsEnumerable();

        var userNotes = fetchedData[4].Cast<EmployeeWorkingShiftNote>().AsEnumerable();

        ProcessAssignmentNote(userMatches);

        userMatches = FilterByTimeRange(model, userMatches, userMatchInfos);


        var userMatchDict = GetUserMatchDictionary(userMatches);
        var userETPKMatchDict = GetUserETPKMatchDictionary(userMatches);
        var userGroups = GetUserGroup(users, userShifts, userNotes, userMatchDict, userETPKMatchDict);

        var userHaveShiftSorted = GetUserHaveShiftSorted(userGroups, model);

        var userSortedShift = SortUserShift(userGroups, userHaveShiftSorted);

        var fromDate = model.Date?.AddHours(12) ?? DateTime.Now;
        var toDate = model.Date?.AddDays(1).AddHours(11).AddMinutes(59).AddSeconds(59) ?? DateTime.Now;
        var assignmentMappingTypes = new[] { typeof(Assignment), typeof(OWPMatch) };
        var assignmentStatsFetchedData =
            await dynamicRepository.FetchMultipleAsync(new QueryAssignmentStatsCriteria { Date = model.Date },
                assignmentMappingTypes);

        var assignmentStatistics = CalculateAssignmentStats(fromDate, toDate, assignmentStatsFetchedData);

        return new QueryResult
        {
            Users = userSortedShift,
            Matches = userMatchInfos,
            AssignmentStatistics = assignmentStatistics
        };
    }

    private static IEnumerable<QueryUserGroup> SortUserShift(
        IEnumerable<QueryUserGroup> userGroups,
        IEnumerable<int> userHaveShiftSorted)
    {
        var userShiftToday = new List<QueryUserGroup>();
        foreach (var x in userHaveShiftSorted)
        {
            var findUser = userGroups.FirstOrDefault(y => y.UserId.Equals(x));
            userShiftToday.Add(findUser);
        }


        var userShiftNotToday = userGroups.Where(x => !userShiftToday.Exists(y => x.UserId.Equals(y.UserId)))
            .OrderByDescending(x => x.LevelId != 0).ThenBy(x => x.LevelId).ThenBy(x => x.UserName);

        return userShiftToday.Concat(userShiftNotToday);
    }

    private static IEnumerable<int> GetUserHaveShiftSorted(IEnumerable<QueryUserGroup> userGroups, QueryModel model)
    {
        var shift = new List<UserShiftToday>();

        var listUserHaveShift = userGroups.Where(x => x.Shifts.Any()).ToList();


        listUserHaveShift.ForEach(x =>
        {
            x.Shifts?.ToList().ForEach(item =>
            {
                var temp = new UserShiftToday
                {
                    LevelId = x.LevelId,
                    UserName = x.UserName,
                    EmployeeId = item.EmployeeId,
                    ShiftDate = item.ShiftDate,
                    ShiftEndTime = item.ShiftEndTime,
                    ShiftId = item.ShiftId,
                    ShiftStartTime = item.ShiftStartTime
                };
                shift.Add(temp);
            });
        });

        var tempDate = Convert.ToDateTime(model.Date.ToString()).AddHours(12);
        return shift.Where(x => !x.ShiftEndTime.Equals(tempDate)).OrderBy(x => x.ShiftStartTime)
            .ThenByDescending(x => x.LevelId != 0).ThenBy(x => x.LevelId).ThenBy(x => x.UserName).Select(x => x.EmployeeId).Distinct();
    }

    private static IEnumerable<QueryUserGroup> GetUserGroup(
        IEnumerable<QueryUser> users,
        IEnumerable<QueryUserShift> userShifts,
        IEnumerable<EmployeeWorkingShiftNote> userNotes,
        Dictionary<int?, List<QueryUserMatchGroup>> userMatchDict,
        Dictionary<int?, List<QueryUserMatchGroup>> userETPKMatchDict)
    {
        return users.Select(user =>
        {
            var userGroup = new QueryUserGroup
            {
                UserId = user.EmployeeId,
                UserName = user.UserName,
                SectionName = user.SectionName,
                LevelName = user.LevelName,
                GroupName = user.GroupName,
                LevelId = user.LevelId,
                Remark = user.Remark,
                ShiftId = user.ShiftId,
                Matches = []
            };
            if (userMatchDict.ContainsKey(user.EmployeeId))
            {
                userGroup.Matches.AddRange(userMatchDict[user.EmployeeId]);
            }

            if (userETPKMatchDict.ContainsKey(user.EmployeeId))
            {
                userGroup.Matches.AddRange(userETPKMatchDict[user.EmployeeId]);
            }

            userGroup.Matches = userGroup.Matches.OrderBy(x => x.EventDateTime).ThenBy(x => x.OWPMatchId).ToList();
            userGroup.Shifts = userShifts.Where(x => x.EmployeeId == user.EmployeeId);
            userGroup.Notes = userNotes.Where(x => x.UserId == user.EmployeeId);
            return userGroup;
        });
    }

    private static Dictionary<int?, List<QueryUserMatchGroup>> GetUserETPKMatchDictionary(
        IEnumerable<QueryUserMatch> userMatches)
    {
        return userMatches.Where(x => !string.IsNullOrEmpty(x.AssignmentNote)).GroupBy(x => x.UserId).ToDictionary(
            g => g.Key, g =>
            {
                return g.GroupBy(x => x.OWPMatchId).Select(gg =>
                {
                    return new QueryUserMatchGroup
                    {
                        OWPMatchId = gg.Key,
                        BetTypes = gg.Select(x => x.Bettype).ToList(),
                        EventDateTime = gg.FirstOrDefault()!.EventDateTime,
                        isETPK = true
                    };
                }).ToList();
            });
    }

    private static Dictionary<int?, List<QueryUserMatchGroup>> GetUserMatchDictionary(
        IEnumerable<QueryUserMatch> userMatches)
    {
        return userMatches.Where(x => string.IsNullOrEmpty(x.AssignmentNote)).GroupBy(x => x.UserId).ToDictionary(
            g => g.Key, g =>
            {
                return g.GroupBy(x => x.OWPMatchId).Select(gg =>
                {
                    return new QueryUserMatchGroup
                    {
                        OWPMatchId = gg.Key,
                        BetTypes = gg.Select(x => x.Bettype).ToList(),
                        EventDateTime = gg.FirstOrDefault()!.EventDateTime,
                        isETPK = false
                    };
                }).ToList();
            });
    }

    private static IEnumerable<QueryUserMatch> FilterByTimeRange(QueryModel model,
        IEnumerable<QueryUserMatch> userMatches, IEnumerable<QueryMatchInfo> userMatchInfos)
    {
        if (!model.Permission.FullTimeView.IsRead)
        {
            userMatches = QueryMatchesByTimeRange(userMatches);
            foreach (var item in userMatchInfos)
            {
                var findItemInTimeRange = userMatches.FirstOrDefault(x => x.OWPMatchId == item.OWPMatchId);
                if (findItemInTimeRange != null)
                {
                    item.IsAllowedToViewDetail = findItemInTimeRange.IsAllowedToViewDetail;
                }
            }
        }

        return userMatches;
    }

    private static void ProcessAssignmentNote(IEnumerable<QueryUserMatch> userMatches)
    {
        foreach (var userMatch in userMatches)
        {
            if (!string.IsNullOrEmpty(userMatch.AssignmentNote))
            {
                userMatch.AssignmentNote =
                    userMatch.AssignmentNote.IndexOf("ET", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    userMatch.AssignmentNote.IndexOf("PK", StringComparison.CurrentCultureIgnoreCase) >= 0
                        ? "ET/PK"
                        : null;
            }
        }
    }

    private static List<QueryUserMatch> QueryMatchesByTimeRange(IEnumerable<QueryUserMatch> matches)
    {
        var dateTime = DateTimeOffset.Now.ToGmt8();
        var timeRange = dateTime.TimeRangeOfDate();
        var selectedMatches = new List<QueryUserMatch>();
        foreach (var match in matches)
        {
            var result = timeRange.CheckMatch(match.EventDateTime);
            if (!result.IsMatchInTimeRange)
            {
                continue;
            }

            match.IsAllowedToViewDetail = result.IsAllowedToViewDetail;
            selectedMatches.Add(match);
        }

        return selectedMatches;
    }

    private AssignmentStatistic CalculateAssignmentStats(DateTime fromDate, DateTime toDate,
        IEnumerable<object>[] assignmentStatsFetchedData)
    {
        var assignments = assignmentStatsFetchedData[0].Cast<Assignment>();
        var fetchedMatches = assignmentStatsFetchedData[1].Cast<OWPMatch>();

        var filteredMatches = fetchedMatches.Where(x =>
                x.MinorBettypeId.HasValue && !ExcludeMinorBettypes.Contains(x.MinorBettypeId.Value) ||
                !x.MinorBettypeId.HasValue)
            .OrderBy(x => x.EventDateTime)
            .ToList();

        var assignmentStatistics = new AssignmentStatistic();

        for (var i = fromDate; i.CompareTo(toDate) < 0; i = i.AddHours(1))
        {
            var selectedMatch = CalculateSelectedMatch(i, filteredMatches);

            var assignedLA = assignments
                .Where(x => selectedMatch.Select(y => y.OWPMatchId).Contains(x.OWPMatchId) &&
                            x.Bettype == LABettype)
                .Select(x => x.UserId)
                .Distinct();

            var assignedTrader = new List<int>();
            var assignedMatch = 0;

            foreach (var match in selectedMatch)
            {
                var matchLA = assignments.Count(x => x.OWPMatchId == match.OWPMatchId && x.Bettype == LABettype);
                var matchAssignedTrader = CalculateMatchAssignedTraders(match, fetchedMatches, assignments);
                assignedTrader.AddRange(matchAssignedTrader);
                assignedMatch += matchAssignedTrader.Count > 0 || matchLA > 0 ? 1 : 0;
            }

            assignmentStatistics.MatchSelected.Add(selectedMatch.Count());
            assignmentStatistics.MatchAssigned.Add(assignedMatch);
            assignmentStatistics.LAAssigned.Add(assignedLA.Count());
            assignmentStatistics.TraderAssigned.Add(assignedTrader.AsQueryable().Distinct().ToList().Count);
        }

        return assignmentStatistics;
    }

    private List<int> CalculateMatchAssignedTraders(OWPMatch match, IEnumerable<OWPMatch> fetchedMatches,
        IEnumerable<Assignment> assigments)
    {
        var matches = new List<int> { match.OWPMatchId };
        var minorMatches = fetchedMatches
            .Where(x => x.IsMinorMatch && x.ParentMatchId == match.ParentMatchId);

        if (!match.IsMinorMatch)
        {
            var minorMatchIds = minorMatches.Where(x => SpecialMainMatchBettypes.Contains(x.MinorBettypeId.Value))
                .Select(x => x.OWPMatchId);
            matches.AddRange(minorMatchIds);
        }
        else
        {
            var minorMatchIds = new List<int>();

            if (Equals(match.MinorBettypeId, NoOfCornerBettype))
            {
                minorMatchIds = minorMatches.Where(x => x.MinorBettypeId.Value == Specific15MinsNoOfCornersBettype)
                    .Select(x => x.OWPMatchId).ToList();
            }

            if (Equals(match.MinorBettypeId, TotalBookingBettype))
            {
                minorMatchIds = minorMatches.Where(x => x.MinorBettypeId.Value == Specific15MinsTotalBookingBettype)
                    .Select(x => x.OWPMatchId).ToList();
            }

            matches.AddRange(minorMatchIds);
        }

        var matchAssignedTrader = assigments
            .Where(x => matches.Contains(x.OWPMatchId) && x.Bettype != LABettype)
            .Select(x => x.UserId)
            .Distinct()
            .ToList();

        return matchAssignedTrader;
    }

    private static IEnumerable<OWPMatch> CalculateSelectedMatch(DateTimeOffset date,
        IEnumerable<OWPMatch> filteredMatches)
    {
        var fromTime = date.AddMinutes(-29).DateTime;
        var toTime = date.AddMinutes(30).DateTime;

        var selectedMatch = filteredMatches
            .Where(x => x.EventDateTime.CompareTo(fromTime) >= 0 && x.EventDateTime.CompareTo(toTime) <= 0);

        return selectedMatch;
    }
}