using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface MJGroupService
{
    void Group(IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
         List<int> owpIdExclude);
}

public class MJGroupServiceImp : MJGroupService
{
    private readonly SingleGameGroupService workingSchedule;

    public MJGroupServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }
    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        List<int> owpIdExclude)
    {
        var mJGroup = assignments.Where(a => a.IsMajorLeague)
            .ToList();
        owpIdExclude.AddRange(mJGroup.Select(x => x.OWPMatchId).Distinct());
        workingSchedule.Group(
           mJGroup,
           schedules,
           _ => BuildScheduleUpdateModel(assignments, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.MJ,
            TextColor = ColorCodeConst.TEXT_MJ,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark);

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = !string.IsNullOrEmpty(etPkValue),
                EtPkValue = etPkValue
            },
            AssignmentCell = assignmentCell,
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(
                gameInfo.eventDate,
                gameInfo.bettypeIds,
                !string.IsNullOrEmpty(etPkValue))
        };
    }
}