using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface WinnerService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate, IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class WinnerServiceImp : WinnerService
{
    private readonly SingleGameGroupService workingSchedule;

    public WinnerServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var gameGroup = assignments
            .Where(a => a.MatchType == Constant.MatchType.Other && a.MinorBettypeId is Bettype.Winner)
            .ToList();

        foreach (var game in gameGroup)
        {
            game.MatchType = Constant.MatchType.RealWinner;
        }
        workingSchedule.Group(
           gameGroup,
           schedules,
            assignmentWinner => BuildScheduleUpdateModel(gameMapGameId, assignmentWinner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId, IList<AssignmentModel> assignmentsWinner, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsWinner);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsWinner);
        }
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.WINNER,
            TextColor = ColorCodeConst.TEXT_WINNER,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsWinner[0], textPostfix)
        };

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsWinner);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsWinner) ? string.Empty
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