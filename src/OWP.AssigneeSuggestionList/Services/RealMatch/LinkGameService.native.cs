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

    public LinkGameServiceImp(OneGameService oneGameService, LinkNGameService linkNGameService)
    {
        this.oneGameService = oneGameService;
        this.linkNGameService = linkNGameService;
    }

    public void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var canLinkAssignments = assignmentModels.Where(x => x.MatchType == Constant.MatchType.Other && !x.IsFantasyMatch).ToList();

        linkNGameService.Group(canLinkAssignments, schedule, fromDate);
        oneGameService.Group(canLinkAssignments, schedule, fromDate, gameMapGameId);
    }
}
