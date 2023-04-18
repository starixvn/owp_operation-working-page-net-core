using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface MainGameService
{
    Task MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude);
}

public class MainGameServiceImp : MainGameService
{
    private readonly MJGroupService mJGroupService;

    private readonly FirstHalfService firstHalfService;

    private readonly FantasyService fantasyService;

    public MainGameServiceImp(
        MJGroupService mJGroupService,
        FirstHalfService firstHalfService,
        FantasyService fantasyService)
    {
        this.mJGroupService = mJGroupService;
        this.firstHalfService = firstHalfService;
        this.fantasyService = fantasyService;
    }

    public async Task MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude)
    {
        await mJGroupService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        await firstHalfService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        await fantasyService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
    }
}
