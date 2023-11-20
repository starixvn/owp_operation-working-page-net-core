using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using OWP.Extensions;

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
        var assignmentMapUserId = AssignmentMapUserId(assignmentModels);
        foreach (var schedule in schedules)
        {
            var assignments = assignmentMapUserId.GetValue(schedule.UserInfo.UserId, new List<AssignmentModel>());
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

    private static Dictionary<int, List<AssignmentModel>> AssignmentMapUserId(IList<AssignmentModel> assignments)
    => assignments
        .GroupBy(a => a.UserId)
        .ToDictionary(x => x.Key, x => x.ToList());

    public void CategoryAssignee(List<WorkingSchedule> schedules, MatchInfo matchInfo)
    {
        categorizeRealMatchServices.Categorize(schedules, matchInfo);
    }
}
