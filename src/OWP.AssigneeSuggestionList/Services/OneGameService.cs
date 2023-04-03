using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services;

public interface OneGameService
{
    Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);
}

public class OneGameServiceImp : OneGameService
{
    private const decimal MIN_MAXBET = 12000;

    private readonly SingleGameGroupService workingSchedule;

    public OneGameServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules)
    {
        var oneGameGroup = assignments
            ?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbet => maxbet > MIN_MAXBET))
            .ToList();

        await workingSchedule.Group(
            oneGameGroup,
            schedules,
            "[oneGameGroup]",
            BuildScheduleUpdateModel);
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.ONE_GAME,
            TextColor = ColorCodeConst.TEXT_ONE_GAME,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark);

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