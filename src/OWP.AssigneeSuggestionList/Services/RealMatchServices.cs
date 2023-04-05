using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;

namespace OWP.AssigneeSuggestionList.Services;

public interface RealMatchServices
{
    Task MapAssignmentsToWorkingSchedule(IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels);
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

    public async Task MapAssignmentsToWorkingSchedule(IEnumerable<WorkingSchedule> schedules, IList<AssignmentModel> assignmentModels)
    {
        foreach (var schedule in schedules)
        {
            var assignments = GetAssignmentsBelongWs(assignmentModels, schedule);
            await mJGroupService.Group(assignments, schedule);
            await oneGameService.Group(assignments, schedule);
            await linkNGameService.Group(assignments, schedule);
            await linkNGameService.Group(assignments, schedule);
            await firstHalfService.Group(assignments, schedule);
            await fantasyService.Group(assignments, schedule);
            await cornerService.Group(assignments, schedule);
            await xMinsGroupService.Group(assignments, schedule);
            await linkErrorService.Group(assignments, schedule);
        }
    }

    private static IList<AssignmentModel> GetAssignmentsBelongWs(IEnumerable<AssignmentModel> assignments, WorkingSchedule schedule)
        => assignments.Where(x =>
            x.UserId == schedule.UserInfo.UserId &&
            !x.IsSaba &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftStartTime) >= 0 &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime) < 0).ToList();
}
