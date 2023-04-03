using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services;

public interface CornerService
{
    Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);

}

public class CornerServiceImp : CornerService
{
    private readonly SingleGameGroupService workingSchedule;

    public CornerServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public async Task Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules)
    {
        var gameGroup = assignments
            ?.Where(a => a.IsCornerMatch)
            .ToList();

        await workingSchedule.Group(
            gameGroup,
            schedules,
            "[CORNER]",
            assignmentsCorner => BuildScheduleUpdateModel(assignmentsCorner, assignments));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignmentsCorner, IList<AssignmentModel> assignments)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.CORNER,
            TextColor = ColorCodeConst.TEXT_CORNER,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsCorner.First())
        };
        var gameInfo = ScheduleHelper.GetGameInfoMinor(assignmentsCorner);
        var parentGame = assignments.FirstOrDefault(g => g.OWPMatchId == gameInfo.parentMatchId);
        var etPkValue = ETPKHelper.DetectExtraTimeCorner(parentGame?.Remark, gameInfo.remark);

        return new ScheduleUpdateModel
        {
            EventDate = gameInfo.eventDate,
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