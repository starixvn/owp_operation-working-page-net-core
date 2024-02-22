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

    private readonly SingleSpecialService singleSpecialService;

    private readonly BookingService bookingService;
    public SpecialGameServiceImp(CornerService cornerService, XMinsGroupService xMinsGroupService, SingleSpecialService singleSpecialService, BookingService bookingService)
    {
        this.cornerService = cornerService;
        this.xMinsGroupService = xMinsGroupService;
        this.singleSpecialService = singleSpecialService;
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
        GroupSingleTeam(schedule, assignmentModels, fromDate, gameMapGameId);
        bookingService.Group(assignmentModels, schedule, fromDate, gameMapGameId);
    }

    private void GroupWinnerAndNextRound(WorkingSchedule schedule, IList<AssignmentModel> assignmentModels, DateTime fromDate, IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        singleSpecialService.Group(assignmentModels, schedule, fromDate, gameMapGameId, Bettype.Winner);
        singleSpecialService.Group(assignmentModels, schedule, fromDate, gameMapGameId, Bettype.NextRound);
    }

    private void GroupXMins(WorkingSchedule schedule, IList<AssignmentModel> assignmentModels, DateTime fromDate, IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        xMinsGroupService.Group(assignmentModels, schedule, fromDate);
        xMinsGroupService.Group(assignmentModels, schedule, fromDate, Bettype.Specific15MinsCorners);
        xMinsGroupService.Group(assignmentModels, schedule, fromDate, Bettype.Specific15MinsTotalBookings);
    }

    private void GroupSingleTeam(WorkingSchedule schedule, IList<AssignmentModel> assignmentModels, DateTime fromDate, IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        singleSpecialService.Group(assignmentModels, schedule, fromDate, gameMapGameId, Bettype.HomeSingleTeam, Bettype.AwaySingleTeam);
    }
}
