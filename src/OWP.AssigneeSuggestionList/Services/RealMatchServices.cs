using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;

namespace OWP.AssigneeSuggestionList.Services;

public interface RealMatchServices
{
    Task MapAssignmentsToWorkingSchedule(IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        DateTime filterDate);
}

public class RealMatchServicesImp : RealMatchServices
{
    private readonly MJGroupService mJGroupService;
    private readonly OneGameService oneGameService;
    private readonly LinkNGameService linkNGameService;
    private readonly FirstHalfService firstHalfService;
    private readonly FantasyService fantasyService;
    private readonly CornerService cornerService;
    private readonly XMinsGroupService xMinsGroupService;
    private readonly LinkErrorService linkErrorService;

    public RealMatchServicesImp(
        MJGroupService mJGroupService,
        OneGameService oneGameService,
        LinkNGameService linkNGameService,
        FirstHalfService firstHalfService,
        FantasyService fantasyService,
        CornerService cornerService,
        XMinsGroupService xMinsGroupService,
        LinkErrorService linkErrorService)
    {
        this.mJGroupService = mJGroupService;
        this.oneGameService = oneGameService;
        this.linkNGameService = linkNGameService;
        this.firstHalfService = firstHalfService;
        this.fantasyService = fantasyService;
        this.cornerService = cornerService;
        this.xMinsGroupService = xMinsGroupService;
        this.linkErrorService = linkErrorService;
    }

    public async Task MapAssignmentsToWorkingSchedule(
        IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        DateTime filterDate)
    {
        var fromDate = filterDate.Date.AddHours(12);

        foreach (var schedule in schedules)
        {
            var assignments = GetAssignmentsBelongWs(assignmentModels, schedule);
            await mJGroupService.Group(assignments, schedule, fromDate);
            await oneGameService.Group(assignments, schedule, fromDate);
            await linkNGameService.Group(assignments, schedule, fromDate);
            await firstHalfService.Group(assignments, schedule, fromDate);
            await fantasyService.Group(assignments, schedule, fromDate);
            await cornerService.Group(assignments, schedule, fromDate);
            await xMinsGroupService.Group(assignments, schedule, fromDate);
            await linkErrorService.Group(assignments, schedule, fromDate);
        }
    }

    private static IList<AssignmentModel> GetAssignmentsBelongWs(IEnumerable<AssignmentModel> assignments, WorkingSchedule schedule)
        => assignments.Where(x =>
            x.UserId == schedule.UserInfo.UserId &&
            !x.IsSaba &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftStartTime) >= 0 &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime) < 0).ToList();
}
