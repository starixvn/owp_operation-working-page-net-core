using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SpecialGameService
{
    void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        IList<ParentMatchInfo> parentMatchInfo,
        DateTime fromDate,
        List<int> owpIdExclude);
}

public class SpecialGameServiceImp : SpecialGameService
{
    private readonly CornerService cornerService;

    private readonly XMinsGroupService xMinsGroupService;

    public SpecialGameServiceImp(CornerService cornerService, XMinsGroupService xMinsGroupService)
    {
        this.cornerService = cornerService;
        this.xMinsGroupService = xMinsGroupService;
    }

    public void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        IList<ParentMatchInfo> parentMatchInfo,
        DateTime fromDate,
        List<int> owpIdExclude)
    {


        cornerService.Group(assignmentModels, schedule, fromDate, parentMatchInfo, owpIdExclude);
        xMinsGroupService.Group(assignmentModels, schedule, fromDate, parentMatchInfo, owpIdExclude);
    }
}
