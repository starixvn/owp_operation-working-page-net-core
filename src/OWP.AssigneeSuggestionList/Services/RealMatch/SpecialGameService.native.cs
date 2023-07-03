using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SpecialGameService
{
    void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude);
}

public class SpecialGameServiceImp : SpecialGameService
{
    private readonly CornerService cornerService;

    private readonly XMinsGroupService xMinsGroupService;

    private readonly SingleTeamService singleTeamService;

    private readonly WinnerService winnerService;

    public SpecialGameServiceImp(CornerService cornerService, XMinsGroupService xMinsGroupService, SingleTeamService singleTeamService, WinnerService winnerService)
    {
        this.cornerService = cornerService;
        this.xMinsGroupService = xMinsGroupService;
        this.singleTeamService = singleTeamService;
        this.winnerService = winnerService;
    }

    public void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude)
    {
        cornerService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        xMinsGroupService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        winnerService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        singleTeamService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
    }
}
