using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface XMinsGroupService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate);
}

public class XMinsGroupServiceImp : XMinsGroupService
{
    private readonly SingleGameGroupService workingSchedule;

    public XMinsGroupServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate)
    {
        var gameGroup = assignments.Where(a => a.Is15Mins).ToList();
      
        foreach (var game in gameGroup)
        {
            game.MatchType = Constant.MatchType.Real15MOU;
        }
        workingSchedule.Group(
           gameGroup,
           schedule,
           assignments15Mins => BuildScheduleUpdateModel(assignments15Mins, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments15Mins, DateTime filterDate)
    {
        var gameInfo = ScheduleHelper.GetGameInfo(assignments15Mins);
        var assignment = new AssignmentModel
        {
            KOTime = assignments15Mins.First().KOTime,
            MaxbetHDP = gameInfo.maxbetHdp
        };

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.X_MINS,
            TextColor = ColorCodeConst.TEXT_X_MINS,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignment, string.Empty)
        };

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule(),
            AssignmentCell = assignmentCell,
            TimeRange = TimeRangeHelper.CalculateTimeRangeFT(gameInfo.eventDate, false)
        };
    }
}