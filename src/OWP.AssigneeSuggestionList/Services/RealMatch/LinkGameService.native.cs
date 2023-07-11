using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkGameService
{
    void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate, IDictionary<int, List<AssignmentModel>> gameMapGameId);
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
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var canLinkAssignments = assignmentModels.Where(x => !x.IsFantasyMatch).ToList();

        oneGameService.Group(canLinkAssignments, schedule, fromDate, gameMapGameId);
        linkNGameService.Group(canLinkAssignments, schedule, fromDate);
        linkErrorService.Group(canLinkAssignments, schedule, fromDate);
    }
}
