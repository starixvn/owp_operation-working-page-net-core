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
            .Where(a => a.IsCornerMatch && a.MatchType == Constant.MatchType.Other)
            .ToList();
        foreach (var game in gameGroup)
        {
            game.MatchType = Constant.MatchType.RealCorner;
        }

        workingSchedule.Group(
           gameGroup,
           schedules,
            assignmentsCorner => BuildScheduleUpdateModel(assignments, gameMapGameId, assignmentsCorner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(
        IList<AssignmentModel> assignments,
        IDictionary<int, List<AssignmentModel>> gameMapGameId,
        IList<AssignmentModel> assignmentsCorner,
        DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsCorner);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsCorner);
        }

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsCorner);

        var parentGame = assignments.FirstOrDefault(a => a.OWPMatchId == assignmentsCorner[0].ParentMatchId);

        var parentGameExtraTime = ETPKHelper.DetectExtraTime(parentGame);

        var etPkValue = (FirstHalfHelper.IsFirstHalf(assignmentsCorner) || string.Equals(parentGameExtraTime, "PK", StringComparison.OrdinalIgnoreCase))
        ? string.Empty
            : ETPKHelper.DetectExtraTime(new()
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
            AssignmentCell = new()
            {
                Color = ColorCodeConst.CORNER,
                TextColor = ColorCodeConst.TEXT_CORNER,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsCorner[0], textPostfix)
            },
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }
}