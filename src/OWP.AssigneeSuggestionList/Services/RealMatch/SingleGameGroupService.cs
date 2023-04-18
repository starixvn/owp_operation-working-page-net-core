using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SingleGameGroupService
{
    Task Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        string groupType,
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc);
}
public class SingleGameGroupServiceImp : SingleGameGroupService
{
    private readonly WorkingScheduleService workingSchedule;

    public SingleGameGroupServiceImp(WorkingScheduleService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public async Task Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        string groupType,
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc)
    {
        var groupGamePair = assignments
            ?.GroupBy(a => a.OWPMatchId)
            .ToDictionary(b => b.Key, b => b.ToList());
        if (groupGamePair is not null)
        {
            foreach (var key in groupGamePair.Keys)
            {
                var scheduleUpdateModel = buildUpdateScheduleFunc.Invoke(groupGamePair[key]);
                workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel, schedule);
            }
        }
    }
}