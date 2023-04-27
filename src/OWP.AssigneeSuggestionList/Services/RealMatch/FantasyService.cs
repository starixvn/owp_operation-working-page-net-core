using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface FantasyService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        List<int> owpIdExclude);
}

public class FantasyServiceImp : FantasyService
{
    private readonly SingleGameGroupService workingSchedule;

    public FantasyServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        List<int> owpIdExclude)
    {
        var matchGroup = assignments.Where(a => a.IsFantasyMatch).ToList();
        owpIdExclude.AddRange(matchGroup.Select(x => x.OWPMatchId).Distinct());
        workingSchedule.Group(
           matchGroup,
           schedule,
           _ => BuildScheduleUpdateModel(assignments, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.FANTASY,
            TextColor = ColorCodeConst.TEXT_FANTASY,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignments) ? string.Empty : ETPKHelper.DetectExtraTime(gameInfo.remark);

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