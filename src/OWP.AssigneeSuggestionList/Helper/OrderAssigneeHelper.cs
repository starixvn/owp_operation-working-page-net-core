using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Helper;
public static class OrderAssigneeHelper
{
    public static List<WorkingSchedule> SortAssignee(MatchInfo matchInfo, List<WorkingSchedule> workingSchedules)
        => matchInfo.IsMainMatch
            ? SortMainMatchAssignee(matchInfo, workingSchedules)
            : SortChildMatchAssignee(matchInfo, workingSchedules);

    public static List<WorkingSchedule> SortMainMatchAssignee(MatchInfo matchInfo,
        List<WorkingSchedule> workingSchedules)
        => matchInfo.IsSoloMatch ? SortAssigneeBySoloMatch(workingSchedules, matchInfo)
            : SortAssigneeByMaxbet(workingSchedules, matchInfo);

    public static List<WorkingSchedule> SortChildMatchAssignee(MatchInfo matchInfo, List<WorkingSchedule> workingSchedules)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByChildLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeChildMatch(x.UserInfo.MainBetType))
            .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
            .ToList();

    public static List<WorkingSchedule> SortAssigneeBySoloMatch(
        List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
            .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
            .ToList();

    public static List<WorkingSchedule> SortAssigneeByMaxbet(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
      => matchInfo.MaxbetHDP switch
      {
          >= OrderConstant.UpperMaxBet => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
                                      .ToList(),

          <= OrderConstant.LowerMaxBet => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
                                      .ToList(),

          _ => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo, x.CategorizedAssignments, matchInfo))
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
        if (!string.IsNullOrEmpty(mainBetType))
        {
            var mainBetTypeLst = mainBetType.Split(',');
            if (matchInfo.IsFutsal && !matchInfo.IsSaba)
            {
                return mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesFs, x)).Min();
            }

            if (OrderConstant.OuTypeIds.Contains(matchInfo.BettypeId))
            {
                return mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesOu, x)).Min();
            }

            if (OrderConstant.HdpTypeIds.Contains(matchInfo.BettypeId))
            {
                return mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesHdp, x)).Min();
            }
        }

        return int.MaxValue;
    }


    public static int SortByMainBetTypeChildMatch(string mainBetType)
    {
        if (string.IsNullOrEmpty(mainBetType))
        {
            return int.MaxValue;
        }

        var mainBetTypeLst = mainBetType.Split(',');
        var mainBetTypeIndex = mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesChild, x)).ToList();
        return mainBetTypeIndex.Min();
    }

    public static int SortByWorkLoad(UserInfo userInfo, List<AssignmentModel> categorizedAssignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var endTime = matchInfo.EndTime;
        var hasWorkingShiftInRangeTime = IsWorkingShiftInRangeTime(userInfo, kOTime, endTime);

        if (!hasWorkingShiftInRangeTime)
        {
            return 7;
        }
        var kOTimeHadBeenRoundMinute = kOTime.Date.AddHours(kOTime.Hour).AddMinutes(0).AddSeconds(0);
        var kOTimeHadBeenRoundMinuteAddOneHour = kOTimeHadBeenRoundMinute.AddHours(1);

        var assignmentInTimeRange = FindAssignmentInTimeRange(categorizedAssignments, kOTimeHadBeenRoundMinute, kOTimeHadBeenRoundMinuteAddOneHour);

        if (assignmentInTimeRange == null)
        {
            return 0;
        }

        return GetAssignmentPriority(assignmentInTimeRange);
    }

    private static bool IsWorkingShiftInRangeTime(UserInfo userInfo, DateTime kOTime, DateTime endTime)
    {
        return userInfo.ShiftStartTime <= kOTime && userInfo.ShiftEndTime >= endTime;
    }

    private static AssignmentModel FindAssignmentInTimeRange(List<AssignmentModel> categorizedAssignments,
        DateTime kOTime, DateTime kOTimeAddOneHours)
    {
        return categorizedAssignments.Find(x =>
            x.EventDateTime.Date.AddHours(x.EventDateTime.Hour).AddMinutes(0).AddSeconds(0) <= kOTime &&
            kOTimeAddOneHours <= x.EndTime.Date.AddHours(x.EndTime.Hour).AddMinutes(0).AddSeconds(0));
    }

    private static int GetAssignmentPriority(AssignmentModel assignment)
    {
        return assignment.MatchType switch
        {

            MatchType.RealMatchLink1G => 1,
            MatchType.RealMatchOther => 1,
            MatchType.RealMatchLink2G => 2,
            MatchType.RealMatchLink3G => 3,
            MatchType.Other => 4,
            MatchType.RealMatchLinkError => 5,
            _ => 6
        };
    }
}
