using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkGameService
{
    void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude);
}

public class LinkGameServiceImp : LinkGameService
{
    private readonly OneGameService oneGameService;
    private readonly LinkNGameService linkNGameService;

    private readonly LinkErrorService linkErrorService;

    public LinkGameServiceImp(OneGameService oneGameService, LinkNGameService linkNGameService, LinkErrorService linkErrorService)
    {
        this.oneGameService = oneGameService;
        this.linkNGameService = linkNGameService;
        this.linkErrorService = linkErrorService;
    }

    public void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude)
    {
        oneGameService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        linkNGameService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        linkErrorService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
    }
}
