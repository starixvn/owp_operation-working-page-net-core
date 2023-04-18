using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;

namespace OWP.AssigneeSuggestionList.Services;

public interface RealMatchServices
{
    void MapAssignmentsToWorkingSchedule(IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        IList<ParentMatchInfo> parentMatchInfo,
        DateTime filterDate);
}

public class RealMatchServicesImp : RealMatchServices
{
    private readonly EtPkService etPkService;
    private readonly MainGameService mainGameServices;
    private readonly LinkGameService linkGameService;
    private readonly SpecialGameService specialGameService;

    public RealMatchServicesImp(
        EtPkService etPkService,
        MainGameService mainGameServices,
        LinkGameService linkGameService,
        SpecialGameService specialGameService)
    {
        this.etPkService = etPkService;
        this.mainGameServices = mainGameServices;
        this.linkGameService = linkGameService;
        this.specialGameService = specialGameService;
    }

    public void MapAssignmentsToWorkingSchedule(
        IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        IList<ParentMatchInfo> parentMatchInfo,
        DateTime filterDate)
    {
        var fromDate = filterDate.Date.AddHours(12);

        foreach (var schedule in schedules)
        {
            var owpIdExclude = new List<int>();
            var assignments = GetAssignmentsBelongWs(assignmentModels, schedule);
            mainGameServices.MapAssignmentsToWorkingSchedule(schedule, assignments, fromDate, owpIdExclude);
            specialGameService.MapAssignmentsToWorkingSchedule(schedule, assignments, parentMatchInfo, fromDate, owpIdExclude);
            linkGameService.MapAssignmentsToWorkingSchedule(schedule, assignments, fromDate, owpIdExclude);
            etPkService.Group(assignments, schedule, fromDate, owpIdExclude);
        }
    }

    private static IList<AssignmentModel> GetAssignmentsBelongWs(IEnumerable<AssignmentModel> assignments, WorkingSchedule schedule)
        => assignments.Where(x =>
            x.UserId == schedule.UserInfo.UserId &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftStartTime) >= 0 &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime) < 0).ToList();
}
