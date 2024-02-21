using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SpecialGameService
{
    void MapAssignmentsToWorkingSchedule(
        WorkingSchedule schedule,
        IList<AssignmentModel> assignmentModels,
        DateTime fromDate,
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
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        cornerService.Group(assignmentModels, schedule, fromDate, gameMapGameId);
        GroupXMins(schedule, assignmentModels, fromDate, gameMapGameId);
        GroupWinnerAndNextRound(schedule, assignmentModels, fromDate, gameMapGameId);
        singleTeamService.Group(assignmentModels, schedule, fromDate, gameMapGameId);
        bookingService.Group(assignmentModels, schedule, fromDate, gameMapGameId);
    }

    private void GroupWinnerAndNextRound(WorkingSchedule schedule, IList<AssignmentModel> assignmentModels, DateTime fromDate, IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        winnerService.Group(assignmentModels, schedule, fromDate, gameMapGameId);
        winnerService.Group(assignmentModels, schedule, fromDate, gameMapGameId, Bettype.NextRound);
    }

    private void GroupXMins(WorkingSchedule schedule, IList<AssignmentModel> assignmentModels, DateTime fromDate, IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        xMinsGroupService.Group(assignmentModels, schedule, fromDate);
        xMinsGroupService.Group(assignmentModels, schedule, fromDate, Bettype.Specific15MinsCorners);
        xMinsGroupService.Group(assignmentModels, schedule, fromDate, Bettype.Specific15MinsTotalBookings);
    }
}
