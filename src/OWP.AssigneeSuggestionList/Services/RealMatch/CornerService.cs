using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface CornerService
{
    Task Group(
        IList<AssignmentModel> assignments,
         WorkingSchedule schedules,
          DateTime filterDate,
          IList<ParentMatchInfo> parentMatchInfo,
        List<int> owpIdExclude);
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
        WorkingSchedule schedules,
        DateTime filterDate,
        IList<ParentMatchInfo> parentMatchInfo,
        List<int> owpIdExclude)
    {
        var gameGroup = assignments
            ?.Where(a => a.IsCornerMatch)
            .ToList();
        owpIdExclude.AddRange(gameGroup?.Select(x => x.OWPMatchId).Distinct());

        await workingSchedule.Group(
            gameGroup,
            schedules,
            "[CORNER]",
            assignmentsCorner => BuildScheduleUpdateModel(assignmentsCorner, filterDate, parentMatchInfo));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(
        IList<AssignmentModel> assignmentsCorner,
          DateTime filterDate,
           IList<ParentMatchInfo> parentMatchInfo)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.CORNER,
            TextColor = ColorCodeConst.TEXT_CORNER,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsCorner.First())
        };
        var gameInfo = ScheduleHelper.GetMinorGameInfo(assignmentsCorner);
        var parentGame = parentMatchInfo?.FirstOrDefault(g => g.OwpMatchId == gameInfo.parentMatchId);
        var etPkValue = ETPKHelper.DetectExtraTimeCorner(parentGame?.Remark, gameInfo.remark);

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