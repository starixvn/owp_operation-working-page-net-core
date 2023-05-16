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
        => matchInfo.IsSoloMatch ? SortAssigneeBySoloMatch(workingSchedules, matchInfo.BettypeId);

    public static List<WorkingSchedule> SortAssigneeBySoloMatch(
        List<WorkingSchedule> workingSchedules, int betTypeId)
        => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByMainBetType(x.UserInfo.MainBetType, betTypeId))
            .ThenBy(x => SortByWorkLoad(x.UserInfo))
            .ThenBy(x => x.UserInfo.ShiftStartTime)
            .ToList();

    public static List<WorkingSchedule> SortAssigneeByMaxbet(List<WorkingSchedule> workingSchedules, MatchInfo matchInfo)
      => matchInfo.MaxbetHDP switch
      {
          >= 80_000 => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetType(x.UserInfo.MainBetType, matchInfo.BettypeId))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ThenBy(x => x.UserInfo.ShiftStartTime)
                                      .ToList(),

          < 35 => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ThenBy(x => x.UserInfo.ShiftStartTime)
                                      .ToList(),

          _ => workingSchedules.OrderBy(x => SortByMainLevel(x.UserInfo.LevelLabel))
                                      .ThenBy(x => SortByMainBetType(x.UserInfo.MainBetType, matchInfo.BettypeId))
                                      .ThenBy(x => SortByRole(x.UserInfo.Role))
                                      .ThenBy(x => SortByWorkLoad(x.UserInfo))
                                      .ThenBy(x => x.UserInfo.ShiftStartTime)
                                      .ToList()
      };



    public static List<WorkingSchedule> SortChildMatchAssignee(MatchInfo matchInfo,
        List<WorkingSchedule> workingSchedules)
        => workingSchedules.OrderBy(x => SortByChildLevel(x.UserInfo.LevelLabel))
            .ThenBy(x => SortByWorkLoad(x.UserInfo))
            .ThenBy(x => x.UserInfo.ShiftStartTime).ToList();



    public static int SortByMainLevel(string level)
        => Array.IndexOf(OrderConstant.MainMatchLevel, level);

    public static int SortByChildLevel(string level)
        => Array.IndexOf(OrderConstant.ChildMatchLevel, level);

    public static int SortByRole(string role)
        => Array.IndexOf(OrderConstant.RoleOrder, role);

    public static int SortByMainBetType(string mainBetType, int betType)
        => 1;

    public static int SortByWorkLoad(UserInfo userInfo)
        => 1;

}
