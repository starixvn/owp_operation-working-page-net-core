using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SpecialGameService
{
    void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude,
        IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class SpecialGameServiceImp : SpecialGameService
{
    private readonly CornerService cornerService;

    private readonly XMinsGroupService xMinsGroupService;

    private readonly SingleTeamService singleTeamService;

    private readonly WinnerService winnerService;

    private readonly BookingService bookingService;
    public SpecialGameServiceImp(CornerService cornerService, XMinsGroupService xMinsGroupService, SingleTeamService singleTeamService, WinnerService winnerService, BookingService bookingService)
    {
        this.cornerService = cornerService;
        this.xMinsGroupService = xMinsGroupService;
        this.singleTeamService = singleTeamService;
        this.winnerService = winnerService;
        this.bookingService = bookingService;   
    }

    public void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
        List<int> owpIdExclude,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        cornerService.Group(assignmentModels, schedule, fromDate, owpIdExclude, gameMapGameId);
        xMinsGroupService.Group(assignmentModels, schedule, fromDate, owpIdExclude);
        winnerService.Group(assignmentModels, schedule, fromDate, owpIdExclude, gameMapGameId);
        singleTeamService.Group(assignmentModels, schedule, fromDate, owpIdExclude, gameMapGameId);
        bookingService.Group(assignmentModels, schedule, fromDate, owpIdExclude, gameMapGameId);
    }
}
