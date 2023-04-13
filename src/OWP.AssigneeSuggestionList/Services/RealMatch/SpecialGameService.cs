using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SpecialGameService
{
    Task MapAssignmentsToWorkingSchedule(
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

    public async Task MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        IList<ParentMatchInfo> parentMatchInfo,
        DateTime fromDate,
        List<int> owpIdExclude)
    {


        await cornerService.Group(assignmentModels, schedule, fromDate, parentMatchInfo, owpIdExclude);
        await xMinsGroupService.Group(assignmentModels, schedule, fromDate, parentMatchInfo, owpIdExclude);
    }
}
