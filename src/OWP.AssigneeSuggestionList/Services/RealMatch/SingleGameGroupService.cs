using OWP.AssigneeSuggestionList.Models;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SingleGameGroupService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc, MatchType matchType = MatchType.RealMatchOther);
}
public class SingleGameGroupServiceImp : SingleGameGroupService
{
    private readonly WorkingScheduleService workingSchedule;

    public SingleGameGroupServiceImp(WorkingScheduleService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc,
        MatchType matchType = MatchType.RealMatchOther
    )
    {
        var groupGamePair = assignments
            .GroupBy(a => a.OWPMatchId)
            .ToDictionary(b => b.Key, b => b.ToList());

        foreach (var key in groupGamePair.Keys)
        {
            var assignmentPresent = groupGamePair[key].MinBy(x => x.BettypeId);
            if (assignmentPresent != null)
            {
                assignmentPresent.MatchType = matchType;
            }
            var scheduleUpdateModel = buildUpdateScheduleFunc.Invoke(groupGamePair[key]);
            workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel, schedule, assignmentPresent);
        }
    }
}