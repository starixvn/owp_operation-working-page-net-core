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
    private readonly SingleGameGroupService workingSchedule;

    public FantasyServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var matchGroup = assignments.Where(a => a.IsFantasyMatch).ToList();
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
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First(), textPostfix)
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignments)
        ? string.Empty
        : ETPKHelper.DetectExtraTime(new AssignmentModel
        {
            Remark = gameInfo.remark,
            AssignmentNote = gameInfo.assignmentNote
        });

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = !string.IsNullOrEmpty(etPkValue),
                EtPkValue = etPkValue
            },
            AssignmentCell = assignmentCell,
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }
}