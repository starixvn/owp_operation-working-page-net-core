using OWP.AssigneeSuggestionList.Models;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SpecialGameGroupService
{
    void Group(
         Dictionary<int, List<AssignmentModel>> groupGamePair,
        WorkingSchedule schedule,
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc, MatchType matchType = MatchType.RealMatchOther);
}
public class SpecialGameGroupServiceImp : SpecialGameGroupService
{
    private readonly WorkingScheduleService workingSchedule;

    public SpecialGameGroupServiceImp(WorkingScheduleService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        Dictionary<int, List<AssignmentModel>> groupGamePair,
        WorkingSchedule schedule,
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc,
        MatchType matchType = MatchType.RealMatchOther)
    {
        foreach (var key in groupGamePair.Keys)
        {
            var assignmentPresent = groupGamePair[key].MinBy(x => x.BettypeId);
            if (assignmentPresent is not null)
            {
                assignmentPresent.MatchType = matchType;
            }
            var scheduleUpdateModel = buildUpdateScheduleFunc.Invoke(groupGamePair[key]);
            if (scheduleUpdateModel is not null)
            {
                workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel, schedule, assignmentPresent);
            }
        }
    }
}