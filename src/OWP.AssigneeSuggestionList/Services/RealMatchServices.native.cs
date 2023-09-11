using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;

namespace OWP.AssigneeSuggestionList.Services;

public interface RealMatchServices
{
    void MapAssignmentsToWorkingSchedule(IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        DateTime filterDate);

    void CategoryAssignee(List<WorkingSchedule> schedules, MatchInfo matchInfo);
}

public class RealMatchServicesImp : RealMatchServices
{
    private readonly EtPkService etPkService;
    private readonly MainGameService mainGameServices;
    private readonly LinkGameService linkGameService;
    private readonly SpecialGameService specialGameService;
    private readonly CategorizeRealMatchServices categorizeRealMatchServices;

    public RealMatchServicesImp(
        EtPkService etPkService,
        MainGameService mainGameServices,
        LinkGameService linkGameService,
        SpecialGameService specialGameService,
        CategorizeRealMatchServices categorizeRealMatchServices)
    {
        this.etPkService = etPkService;
        this.mainGameServices = mainGameServices;
        this.linkGameService = linkGameService;
        this.specialGameService = specialGameService;
        this.categorizeRealMatchServices = categorizeRealMatchServices;
    }

    public void MapAssignmentsToWorkingSchedule(
        IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        DateTime filterDate)
    {
        var fromDate = filterDate.Date.AddHours(12);

        var gameMapGameId = GameMapGameId(assignmentModels);
        foreach (var schedule in schedules)
        {
            var assignments = GetAssignmentsBelongWs(assignmentModels, schedule);
            mainGameServices.MapAssignmentsToWorkingSchedule(schedule, assignments, fromDate, gameMapGameId);
            specialGameService.MapAssignmentsToWorkingSchedule(schedule, assignments, fromDate, gameMapGameId);
            linkGameService.MapAssignmentsToWorkingSchedule(schedule, assignments, fromDate, gameMapGameId);
            etPkService.Group(assignments, schedule, fromDate);
        }
    }

    private static IDictionary<int, List<AssignmentModel>> GameMapGameId(IList<AssignmentModel> assignmentModels)
    => assignmentModels
        .GroupBy(g => g.OWPMatchId)
        .ToDictionary(x => x.Key, x => x.ToList());

    private static IList<AssignmentModel> GetAssignmentsBelongWs(IEnumerable<AssignmentModel> assignments, WorkingSchedule schedule)
        => assignments.Where(x =>
            x.UserId == schedule.UserInfo.UserId &&
            DateTime.Compare(x.EndTime, schedule.UserInfo.ShiftStartTime) >= 0 &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime) < 0).ToList();

    public void CategoryAssignee(List<WorkingSchedule> schedules, MatchInfo matchInfo)
    {
        categorizeRealMatchServices.Categorize(schedules, matchInfo);
    }
}
