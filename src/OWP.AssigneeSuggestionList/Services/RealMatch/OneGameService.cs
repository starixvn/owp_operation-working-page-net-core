using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface OneGameService
{
    Task Group(
        IList<AssignmentModel> assignments,
         WorkingSchedule schedules,
          DateTime filterDate,
          List<int> owpIdExclude);
}

public class OneGameServiceImp : OneGameService
{
    private const decimal MIN_MAXBET = 12000;

    private readonly SingleGameGroupService workingSchedule;

    public OneGameServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public async Task Group(
        IList<AssignmentModel> assignments,
         WorkingSchedule schedules,
          DateTime filterDate,
        List<int> owpIdExclude)
    {
        var oneGameGroup = assignments
            ?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbet => maxbet > MIN_MAXBET))
            .ToList();
        owpIdExclude.AddRange(oneGameGroup?.Select(x => x.OWPMatchId).Distinct());

        await workingSchedule.Group(
            oneGameGroup,
            schedules,
            "[oneGameGroup]",
            assignments => BuildScheduleUpdateModel(assignments, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.ONE_GAME,
            TextColor = ColorCodeConst.TEXT_ONE_GAME,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark);

        return new()
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