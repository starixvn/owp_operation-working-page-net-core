using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;

namespace OWP.AssigneeSuggestionList.Services;

public interface RealMatchServices
{
    Task MapAssignmentsToWorkingSchedule(IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        DateTime filterDate);
}

public class RealMatchServicesImp : RealMatchServices
{
    private readonly EtPkService etPkService;
    private readonly MainGameService mainGameServices;
    private readonly LinkGameService linkGameService;
    private readonly SpecialGameService specialGameService;

    private readonly Repository repository;

    public RealMatchServicesImp(
        Repository repository,
        EtPkService etPkService,
        MainGameService mainGameServices,
        LinkGameService linkGameService,
        SpecialGameService specialGameService)
    {
        this.repository = repository;
        this.etPkService = etPkService;
        this.mainGameServices = mainGameServices;
        this.linkGameService = linkGameService;
        this.specialGameService = specialGameService;
    }

    public async Task MapAssignmentsToWorkingSchedule(
        IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        DateTime filterDate)
    {
        var fromDate = filterDate.Date.AddHours(12);
        var owpMatchId = assignmentModels
        ?.Where(a => a.ParentMatchId.HasValue)
        .Select(a => a.OWPMatchId)
        ?? new List<int>();

        var parentMatchInfo = (await repository.QueryParentMatchInfo(string.Join(",", owpMatchId)))?.ToList();

        foreach (var schedule in schedules)
        {
            var owpIdExclude = new List<int>();
            var assignments = GetAssignmentsBelongWs(assignmentModels, schedule);
            await mainGameServices.MapAssignmentsToWorkingSchedule(schedule, assignments, fromDate, owpIdExclude);
            await specialGameService.MapAssignmentsToWorkingSchedule(schedule, assignments, parentMatchInfo, fromDate, owpIdExclude);
            await linkGameService.MapAssignmentsToWorkingSchedule(schedule, assignments, fromDate, owpIdExclude);
            await etPkService.Group(assignments, schedule, fromDate, owpIdExclude);
        }
    }

    private static IList<AssignmentModel> GetAssignmentsBelongWs(IEnumerable<AssignmentModel> assignments, WorkingSchedule schedule)
        => assignments.Where(x =>
            x.UserId == schedule.UserInfo.UserId &&
            !x.IsSaba &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftStartTime) >= 0 &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime) < 0).ToList();
}
