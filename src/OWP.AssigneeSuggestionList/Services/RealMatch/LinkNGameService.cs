using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkNGameService
{
    void Group(
        IList<AssignmentModel> assignments,
         WorkingSchedule schedules,
         DateTime filterDate,
         List<int> owpIdExclude);

}

public class LinkNGameServiceImp : LinkNGameService
{

    private readonly WorkingScheduleService workingSchedule;

    public LinkNGameServiceImp(WorkingScheduleService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
         DateTime filterDate,
         List<int> owpIdExclude)
    {
        LinkTwoGame(assignments, schedules, filterDate, owpIdExclude);
        LinkThreeGame(assignments, schedules, filterDate, owpIdExclude);
    }

    private void LinkTwoGame(
        IEnumerable<AssignmentModel> assignments,
        WorkingSchedule schedules,
         DateTime filterDate,
         List<int> owpIdExclude)
    {
        var linkNGamePair = LinkGameConditionHelper.GroupGame(assignments, numberGameInGroup => numberGameInGroup == 2);
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.TWO_GAME,
            TextColor = ColorCodeConst.TEXT_TWO_GAME
        };
        GetOwpMatchIdExclude(linkNGamePair, owpIdExclude);
        UpdateSchedule(linkNGamePair, schedules, assignmentCell, filterDate);
    }

    private void LinkThreeGame(
        IEnumerable<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        List<int> owpIdExclude)
    {
        var linkNGamePair = LinkGameConditionHelper.GroupGame(assignments, numberGameInGroup => numberGameInGroup == 3);
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.THREE_GAME,
            TextColor = ColorCodeConst.TEXT_THREE_GAME
        };
        GetOwpMatchIdExclude(linkNGamePair, owpIdExclude);
        UpdateSchedule(linkNGamePair, schedules, assignmentCell, filterDate);
    }

    private static void GetOwpMatchIdExclude(IDictionary<DateTime, List<AssignmentModel>> linkNGamePair
       , List<int> owpIdExclude)
    {
        foreach (var eventDate in linkNGamePair.Keys)
        {
            var owpMatchId = linkNGamePair[eventDate].Select(x => x.OWPMatchId).Distinct();
            owpIdExclude.AddRange(owpMatchId);
        }
    }
    private void UpdateSchedule(
        IDictionary<DateTime, List<AssignmentModel>> linkNGamePair,
        WorkingSchedule schedules,
        AssignmentCell assignmentCell,
        DateTime filterDate)
    {

        foreach (var eventDate in linkNGamePair.Keys)
        {
            var assignmentPresent = linkNGamePair[eventDate].DistinctBy(a => a.OWPMatchId).ToList();
            var remark = assignmentPresent.Select(x => x.Remark).ToList();
            assignmentCell.Text = ScheduleHelper.BuildScheduleTextGroupGame(assignmentPresent);

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
        }
    }
}