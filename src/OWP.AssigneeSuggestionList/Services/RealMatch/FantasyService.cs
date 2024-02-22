using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface FantasyService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class FantasyServiceImp : FantasyService
{
    private readonly SpecialGroupService workingSchedule;

    public FantasyServiceImp(SpecialGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var matchGroup = assignments.Where(a => a.IsFantasyMatch && a.MatchType == Constant.MatchType.Other).ToList();
        foreach (var game in matchGroup)
        {
            game.MatchType = Constant.MatchType.RealFantasy;
        }

        workingSchedule.Group(
           matchGroup,
           schedule,
           assignment => BuildScheduleUpdateModel(gameMapGameId, assignment, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId, IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignments);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignments);
        }

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.FANTASY,
            TextColor = ColorCodeConst.TEXT_FANTASY,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments[0], textPostfix)
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignments)
        ? string.Empty
        : ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.assignmentNote);

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule(etPkValue),
            AssignmentCell = assignmentCell,
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }
}