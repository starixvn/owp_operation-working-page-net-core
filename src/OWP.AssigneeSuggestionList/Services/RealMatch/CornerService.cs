using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface CornerService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class CornerServiceImp : CornerService
{
    private readonly SingleGameGroupService workingSchedule;

    public CornerServiceImp(SingleGameGroupService workingSchedule)
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
            .Where(a => a.IsCornerMatch)
            .ToList();
        foreach (var game in gameGroup)
        {
            game.MatchType = Constant.MatchType.RealCorner;
        }
        
        workingSchedule.Group(
           gameGroup,
           schedules,
            assignmentsCorner => BuildScheduleUpdateModel(gameMapGameId, assignmentsCorner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId, IList<AssignmentModel> assignmentsCorner, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsCorner);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsCorner);
        }
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.CORNER,
            TextColor = ColorCodeConst.TEXT_CORNER,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsCorner[0], textPostfix)
        };

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsCorner);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsCorner) ? string.Empty
            : ETPKHelper.DetectExtraTime(new AssignmentModel
            {
                Remark = gameInfo.remark,
                AssignmentNote = gameInfo.assignmentNote,
                MinorBettypeId = gameInfo.minorBettypeId
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