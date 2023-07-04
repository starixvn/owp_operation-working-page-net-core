using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface XMinsGroupService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        List<int> owpIdExclude);
}

public class XMinsGroupServiceImp : XMinsGroupService
{
    private readonly SingleGameGroupService workingSchedule;

    public XMinsGroupServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        List<int> owpIdExclude)
    {
        var gameGroup = assignments.Where(a => a.Is15Mins).ToList();
        owpIdExclude.AddRange(gameGroup.Select(x => x.OWPMatchId).Distinct());

        workingSchedule.Group(
           gameGroup,
           schedule,
           assignments15Mins => BuildScheduleUpdateModel(assignments15Mins, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments15Mins, DateTime filterDate)
    {
        var gameInfo = ScheduleHelper.GetGameInfo(assignments15Mins);
        var assignment = new AssignmentModel
        {
            KOTime = assignments15Mins.First().KOTime,
            MaxbetHDP = gameInfo.maxbetHdp
        };

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.X_MINS,
            TextColor = ColorCodeConst.TEXT_X_MINS,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignment, string.Empty)
        };
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
            TimeRange = TimeRangeHelper.CalculateTimeRangeFT(gameInfo.eventDate, !string.IsNullOrEmpty(etPkValue))
        };
    }
}