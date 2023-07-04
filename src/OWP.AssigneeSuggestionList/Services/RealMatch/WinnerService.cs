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
        DateTime filterDate,
        List<int> owpIdExclude, IDictionary<int, List<AssignmentModel>> gameMapGameId);
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
        List<int> owpIdExclude, IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var gameGroup = assignments
            .Where(a => a.BettypeId == Bettype.Winner)
            .ToList();
        owpIdExclude.AddRange(gameGroup.Select(x => x.OWPMatchId).Distinct());

        workingSchedule.Group(
           gameGroup,
           schedules,
            assignmentWinner => BuildScheduleUpdateModel(gameMapGameId,assignmentWinner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId,IList<AssignmentModel> assignmentsWinner, DateTime filterDate)
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
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsWinner.First(), textPostfix)
        };

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsWinner);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsWinner) ? string.Empty
            : ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.minorBettypeId);

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