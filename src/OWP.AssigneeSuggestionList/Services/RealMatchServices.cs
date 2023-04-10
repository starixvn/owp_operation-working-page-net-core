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
    private readonly EtPkService etPkService;

    private readonly Repository repository;

    public RealMatchServicesImp(
        MJGroupService mJGroupService,
        OneGameService oneGameService,
        LinkNGameService linkNGameService,
        FirstHalfService firstHalfService,
        FantasyService fantasyService,
        CornerService cornerService,
        XMinsGroupService xMinsGroupService,
        LinkErrorService linkErrorService,
        Repository repository,
        EtPkService etPkService)
    {
        this.mJGroupService = mJGroupService;
        this.oneGameService = oneGameService;
        this.linkNGameService = linkNGameService;
        this.firstHalfService = firstHalfService;
        this.fantasyService = fantasyService;
        this.cornerService = cornerService;
        this.xMinsGroupService = xMinsGroupService;
        this.linkErrorService = linkErrorService;
        this.repository = repository;
        this.etPkService = etPkService;
    }

    public async Task MapAssignmentsToWorkingSchedule(
        IEnumerable<WorkingSchedule> schedules,
        IList<AssignmentModel> assignmentModels,
        DateTime filterDate)
    {
        var fromDate = filterDate.Date.AddHours(12);
        var owpMatchId = assignmentModels
        ?.Where(a => a.ParentMatchId.HasValue)
        .Select(a => a.OWPMatchId)
        ?? new List<int>();

        var parentMatchInfo = (await repository.QueryParentMatchInfo(string.Join(",", owpMatchId)))?.ToList();

        foreach (var schedule in schedules)
        {
            var owpIdExclude = new List<int>();
            var assignments = GetAssignmentsBelongWs(assignmentModels, schedule);
            await mJGroupService.Group(assignments, schedule, fromDate, owpIdExclude);
            await oneGameService.Group(assignments, schedule, fromDate, owpIdExclude);
            await linkNGameService.Group(assignments, schedule, fromDate, owpIdExclude);
            await firstHalfService.Group(assignments, schedule, fromDate, owpIdExclude);
            await fantasyService.Group(assignments, schedule, fromDate, owpIdExclude);
            await cornerService.Group(assignments, schedule, fromDate, parentMatchInfo, owpIdExclude);
            await xMinsGroupService.Group(assignments, schedule, fromDate, parentMatchInfo, owpIdExclude);
            await linkErrorService.Group(assignments, schedule, fromDate, owpIdExclude);
            await etPkService.Group(assignments, schedule, fromDate, owpIdExclude);
        }
    }

    private static IList<AssignmentModel> GetAssignmentsBelongWs(IEnumerable<AssignmentModel> assignments, WorkingSchedule schedule)
        => assignments.Where(x =>
            x.UserId == schedule.UserInfo.UserId &&
            !x.IsSaba &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftStartTime) >= 0 &&
            DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime) < 0).ToList();
}
