using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface MainGameService
{
    void MapAssignmentsToWorkingSchedule(
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

    public void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude)
    {
        mJGroupService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        firstHalfService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        fantasyService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
    }
}
