using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper;
public static class OrderAssigneeHelper
{
    public static List<WorkingSchedule> SortAssignee(MatchInfo matchInfo, List<WorkingSchedule> workingSchedules)
        => matchInfo.IsMainMatch
            ? SortMainMatchAssignee(matchInfo, workingSchedules)
            : SortChildMatchAssignee(matchInfo, workingSchedules);

    public static List<WorkingSchedule> SortMainMatchAssignee(MatchInfo matchInfo,
        List<WorkingSchedule> workingSchedules)
        => SortAssigneeBySoloMatch(workingSchedules, matchInfo);

    public static List<WorkingSchedule> SortAssigneeBySoloMatch(
        List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
        => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
            .ThenBy(x => SortByWorkLoad(x.UserInfo))
            .ThenBy(x => x.UserInfo.ShiftStartTime)
            .ToList();

    public static List<WorkingSchedule> SortAssigneeByMaxbet(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
      => matchInfo.MaxbetHDP switch
      {
          >= 80_000 => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ThenBy(x => x.UserInfo.ShiftStartTime)
                                      .ToList(),

          < 35 => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ThenBy(x => x.UserInfo.ShiftStartTime)
                                      .ToList(),

          _ => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetTypeMainMatch(x.UserInfo.MainBetType, matchInfo))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ThenBy(x => x.UserInfo.ShiftStartTime)
                                      .ToList()
      };



    public static List<WorkingSchedule> SortChildMatchAssignee(MatchInfo matchInfo,
        List<WorkingSchedule> workingSchedules)
        => workingSchedules.OrderBy(x => SortByChildLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetTypeChildMatch(x.UserInfo.MainBetType))
            .ThenBy(x => SortByWorkLoad(x.UserInfo))
            .ThenBy(x => x.UserInfo.ShiftStartTime).ToList();



    public static int SortByMainLevel(string level)
        => Array.IndexOf(OrderConstant.MainMatchLevel, level);

    public static int SortByChildLevel(string level)
        => Array.IndexOf(OrderConstant.ChildMatchLevel, level);

    public static int SortByRole(string role)
        => Array.IndexOf(OrderConstant.RoleOrder, role);

    public static int SortByMainBetTypeMainMatch(string mainBetType, MatchInfo matchInfo)
    {
        var mainBetTypeLst = mainBetType.Split(',');
        var mainBetTypeIndex = new List<int>();

        if (OrderConstant.HdpTypeIds.Contains(matchInfo.BettypeId))
        {
            mainBetTypeIndex = mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesHdp, x)).ToList();
        }

        if (OrderConstant.OuTypeIds.Contains(matchInfo.BettypeId))
        {
            mainBetTypeIndex = mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesOu, x)).ToList();
        }

        if (matchInfo.IsFutsal || matchInfo.IsSaba)
        {
            mainBetTypeIndex = mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesFs, x)).ToList();
        }
        return mainBetTypeIndex.Max();
    }

    public static int SortByMainBetTypeChildMatch(string mainBetType)
    {
        var mainBetTypeLst = mainBetType.Split(',');
        var mainBetTypeIndex = mainBetTypeLst.Select(x => Array.IndexOf(OrderConstant.MainBetTypesChild, x)).ToList();
        return mainBetTypeIndex.Max();
    }

    public static int SortByWorkLoad(UserInfo userInfo)
    {

        return 1;
    }
       
}
