using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Helper;
public static class OrderAssigneeHelper
{
    public static List<WorkingSchedule> SortAssignee(MatchInfo matchInfo, List<WorkingSchedule> workingSchedules)
        => matchInfo.IsMainMatch
            ? SortMainMatchAssignee(workingSchedules, matchInfo)
            : SortChildMatchAssignee(workingSchedules, matchInfo);

    public static List<WorkingSchedule> SortMainMatchAssignee(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
    {
        if (matchInfo.IsSaba)
        {
            return SortAssigneeBySabaMatch(workingSchedules, matchInfo);
        }

        if (matchInfo.IsSoloMatch)
        {
            return SortAssigneeBySoloMatch(workingSchedules, matchInfo);
        }

        if (matchInfo.IsFutsal)
        {
            return SortFutsalMatchAssignee(workingSchedules, matchInfo);
        }

        return SortAssigneeByMaxbet(workingSchedules, matchInfo);
    }

    public static List<WorkingSchedule> SortAssigneeBySabaMatch(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByMainBetTypeSabaMatch(x.UserInfo.MainBetType, matchInfo))
            .ThenBy(x => SortByRole(x.UserInfo.Role))
            .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
            .ThenBy(x => SortByMatchType(x.CategorizedAssignments, matchInfo))
            .ToList();

    public static List<WorkingSchedule> SortFutsalMatchAssignee(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByMainBetTypeFutsalMatch(x.UserInfo.MainBetType))
            .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByRole(x.UserInfo.Role))
            .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
            .ThenBy(x => SortByMatchType(x.CategorizedAssignments, matchInfo))
            .ToList();

    public static List<WorkingSchedule> SortChildMatchAssignee(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByChildLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeChildMatch(x.UserInfo.MainBetType))
            .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
            .ThenBy(x => SortByMatchType(x.CategorizedAssignments, matchInfo))
            .ToList();

    public static List<WorkingSchedule> SortAssigneeBySoloMatch(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
            .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
            .ThenBy(x => SortByMatchType(x.CategorizedAssignments, matchInfo))
            .ToList();

    public static List<WorkingSchedule> SortAssigneeByMaxbet(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
      => matchInfo.MaxbetHDP switch
      {
          >= OrderConstant.UpperMaxBet => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
                                      .ThenBy(x => SortByMatchType(x.CategorizedAssignments, matchInfo))
                                      .ToList(),

          <= OrderConstant.LowerMaxBet => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
                                      .ThenBy(x => SortByMatchType(x.CategorizedAssignments, matchInfo))
                                      .ToList(),

          _ => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
                                      .ThenBy(x => SortByMatchType(x.CategorizedAssignments, matchInfo))
                                      .ToList()
      };

    public static int SortByMainLevel(string level)
        => Array.IndexOf(OrderConstant.MainMatchLevel, level);

    public static int SortByChildLevel(string level)
        => Array.IndexOf(OrderConstant.ChildMatchLevel, level);

    public static int SortByRole(string role)
        => Array.IndexOf(OrderConstant.RoleOrder, role);

    public static int SortByMainBetTypeMainMatch(string mainBetType, MatchInfo matchInfo)
    {
        if (OrderConstant.OuTypeIds.Contains(matchInfo.BettypeId))
        {
            return GetMinIndexOfMainBetType(mainBetType, OrderConstant.MainBetTypesOu);
        }

        return OrderConstant.HdpTypeIds.Contains(matchInfo.BettypeId)
            ? GetMinIndexOfMainBetType(mainBetType, OrderConstant.MainBetTypesHdp)
            : int.MaxValue;
    }

    public static int SortByMainBetTypeChildMatch(string mainBetType)
        => GetMinIndexOfMainBetType(mainBetType, OrderConstant.MainBetTypesChild);

    public static int SortByMainBetTypeFutsalMatch(string mainBetType)
        => GetMinIndexOfMainBetType(mainBetType, OrderConstant.MainBetTypesFs);

    public static int SortByMainBetTypeSabaMatch(string mainBetType, MatchInfo matchInfo)
    {
        var is5MinsMatch = matchInfo.MatchType is MatchType.Saba5Mins;
        var is6MinsMatch = matchInfo.MatchType is MatchType.SabaFutsal6Mins;
        var mainBettype = is6MinsMatch ? OrderConstant.MainBetTypes6Mins : OrderConstant.MainBetTypesSb;

        return is5MinsMatch
            ? GetMinIndexOfMainBetType(mainBetType, OrderConstant.MainBetTypes5Mins)
            : GetMinIndexOfMainBetType(mainBetType, mainBettype);
    }

    private static int GetMinIndexOfMainBetType(string mainBetType, string[] mainBetTypeConstant)
    {
        if (string.IsNullOrEmpty(mainBetType))
        {
            return int.MaxValue;
        }

        var mainBetTypeLst = mainBetType.Split(',');
        return mainBetTypeLst.Select(x => Array.IndexOf(mainBetTypeConstant, x)).Min();
    }

    public static int SortByWorkLoad(UserInfo userInfo, List<AssignmentModel> categorizedAssignments, MatchInfo matchInfo)
    {
        var assignmentInTimeRange = FindAssignmentInTimeRange(categorizedAssignments, matchInfo);

        if (assignmentInTimeRange != null)
        {
            return GetAssignmentPriority(assignmentInTimeRange);
        }

        var kOTime = matchInfo.KOTime;
        var endTime = matchInfo.EndTime;

        var hasWorkingShiftInRangeTime = IsWorkingShiftInRangeTime(userInfo, kOTime, endTime);

        return !hasWorkingShiftInRangeTime ? int.MaxValue : 0;
    }
    public static int SortByMatchType(List<AssignmentModel> categorizedAssignments, MatchInfo matchInfo)
    {
        var assignmentInTimeRange = FindAssignmentInTimeRange(categorizedAssignments, matchInfo);
        return assignmentInTimeRange is { IsSaba: true } ? 1 : 0;
    }

    private static AssignmentModel FindAssignmentInTimeRange(List<AssignmentModel> categorizedAssignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var kOTimeHadBeenRoundMinute = ResetMinuteToZero(kOTime);
        var kOTimeHadBeenRoundMinuteAddOneHour = kOTimeHadBeenRoundMinute.AddHours(1);

        return categorizedAssignments.Find(x =>
            ResetMinuteToZero(x.EventDateTime) <= kOTime &&
            kOTimeHadBeenRoundMinuteAddOneHour <= ResetMinuteToZero(x.EndTime));
    }

    private static DateTime ResetMinuteToZero(DateTime time)
        => time.Date.AddHours(time.Hour).AddMinutes(0).AddSeconds(0);

    private static bool IsWorkingShiftInRangeTime(UserInfo userInfo, DateTime kOTime, DateTime endTime)
        => userInfo.ShiftStartTime <= kOTime && userInfo.ShiftEndTime >= endTime;

    private static int GetAssignmentPriority(AssignmentModel assignment)
    {
        return assignment.MatchType switch
        {

            MatchType.RealMatchLink1G => 1,
            MatchType.RealMatchOther => 1,
            MatchType.SabaOther => 1,
            MatchType.Saba5Mins => 1,
            MatchType.Saba15Mins => 1,
            MatchType.SabaFutsal6Mins => 1,
            MatchType.SabaM8816Mins => 1,
            MatchType.Saba16Mins => 1,
            MatchType.Saba20Mins => 1,
            MatchType.SabaLink1G => 1,
            MatchType.SabaMix5mins => 2,
            MatchType.SabaLink2G => 2,
            MatchType.SabaLink3G => 3,
            MatchType.SabaLink4G => 4,
            MatchType.RealMatchLink2G => 2,
            MatchType.RealMatchLink3G => 3,
            MatchType.Other => 4,
            MatchType.RealMatchLinkError => 5,
            _ => 6
        };
    }
}
