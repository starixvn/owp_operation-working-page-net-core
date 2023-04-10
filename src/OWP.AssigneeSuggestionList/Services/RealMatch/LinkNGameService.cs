using Fanex.Logging;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkNGameService
{
    Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules, DateTime filterDate);

}

public class LinkNGameServiceImp : LinkNGameService
{
    private readonly ILogger logger;

    private readonly WorkingScheduleService workingSchedule;

    public LinkNGameServiceImp(ILogger logger, WorkingScheduleService workingSchedule)
    {
        this.logger = logger;
        this.workingSchedule = workingSchedule;
    }

    public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules, DateTime filterDate)
    {
        await LinkTwoGame(assignments, schedules, filterDate);
        await LinkThreeGame(assignments, schedules, filterDate);
    }

    private async Task LinkTwoGame(IEnumerable<AssignmentModel> assignment, WorkingSchedule schedules, DateTime filterDate)
    {
        var linkNGamePair = GetValidGame(assignment, 2);
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.TWO_GAME,
            TextColor = ColorCodeConst.TEXT_TWO_GAME
        };
        await UpdateSchedule(linkNGamePair, schedules, assignmentCell, filterDate);
    }

    private async Task LinkThreeGame(IEnumerable<AssignmentModel> assignment, WorkingSchedule schedules, DateTime filterDate)
    {
        var linkNGamePair = GetValidGame(assignment, 3);
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.THREE_GAME,
            TextColor = ColorCodeConst.TEXT_THREE_GAME
        };
        await UpdateSchedule(linkNGamePair, schedules, assignmentCell, filterDate);
    }

    private async Task UpdateSchedule(
        IDictionary<DateTime, List<AssignmentModel>> linkNGamePair,
        WorkingSchedule schedules,
        AssignmentCell assignmentCell,
        DateTime filterDate)
    {

        foreach (var eventDate in linkNGamePair.Keys)
        {
            var remark = linkNGamePair[eventDate].Select(x => x.Remark).ToList();
            assignmentCell.Text = ScheduleHelper.BuildScheduleTextGroupGame(linkNGamePair[eventDate]);

            var etpkValue = ETPKHelper.BuildEtPkTextDisplay(remark);
            var timeRange = TimeRangeHelper.CalculateTimeRangeFT(eventDate, !string.IsNullOrEmpty(etpkValue));


            var scheduleUpdateModel = new ScheduleUpdateModel
            {
                FilterDate = filterDate,
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = !string.IsNullOrEmpty(etpkValue),
                    EtPkValue = etpkValue
                },
                AssignmentCell = assignmentCell,
                TimeRange = timeRange
            };

            workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel,
                schedules);

            await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[eventDate]?.Select(x => x.OWPMatchId) ?? Array.Empty<int>()));
            await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[eventDate]?.Select(x => x.BettypeId) ?? Array.Empty<int>()));
        }
    }



    private static IDictionary<DateTime, List<AssignmentModel>> GetValidGame(IEnumerable<AssignmentModel> models, int numberGame)
        => models.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbet => !maxbet.HasValue || maxbet.Value <= 12000))
            .GroupBy(a => a.EventDateTime)
            .Where(g => g.Select(x => x.OWPMatchId).Distinct().Count() == numberGame)
            .ToDictionary(x => x.Key, x => x.ToList());
}