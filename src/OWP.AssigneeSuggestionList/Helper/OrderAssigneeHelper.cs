using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper;
public static class OrderAssigneeHelper
{
    public static List<WorkingSchedule> SortAssignee(MatchInfo matchInfo, List<WorkingSchedule> workingSchedules)
        => matchInfo.IsMainMatch
            ? SortMainMatchAssignee(matchInfo, workingSchedules)
            : SortChildMatchAssignee(workingSchedules);

    public static List<WorkingSchedule> SortMainMatchAssignee(MatchInfo matchInfo,
        List<WorkingSchedule> workingSchedules)
        => matchInfo.IsSoloMatch ? SortAssigneeBySoloMatch(workingSchedules, matchInfo)
            : SortAssigneeByMaxbet(workingSchedules, matchInfo);

    public static List<WorkingSchedule> SortChildMatchAssignee(List<WorkingSchedule> workingSchedules)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByChildLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeChildMatch(x.UserInfo.MainBetType))
            .ThenBy(x => SortByWorkLoad(x.UserInfo))
            .ToList();

    public static List<WorkingSchedule> SortAssigneeBySoloMatch(
        List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
        => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
            .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
            .ThenBy(x => SortByWorkLoad(x.UserInfo))
            .ToList();

    public static List<WorkingSchedule> SortAssigneeByMaxbet(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
      => matchInfo.MaxbetHDP switch
      {
          >= OrderConstant.UpperMaxBet => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ToList(),

          <= OrderConstant.LowerMaxBet => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ToList(),

          _ => workingSchedules.OrderBy(x => x.UserInfo.ShiftStartTime)
                                      .ThenBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
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

    public static int SortByWorkLoad(UserInfo userInfo)
        => userInfo.CategoryType switch
        {
            CategoryType.FullyAvailable => userInfo.WorkLoadType,
            CategoryType.Busy => (int)userInfo.MixMatchCapacity,
            _ => -1
        };
}
