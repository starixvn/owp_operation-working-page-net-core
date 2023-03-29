using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;

namespace OWP.AssigneeSuggestionList.Handlers;

public class QueryTraderSuggestionRequest : IRequest<(MatchInfo, List<WorkingSchedule>)>
{
    public int OwpMatchId { get; set; }

    public string BetType { get; set; }
}

public class QueryTraderSuggestionHandler : IRequestHandler<QueryTraderSuggestionRequest, (MatchInfo, List<WorkingSchedule>)>
{
    private readonly Repository repository;
    private readonly WorkingShiftService workingShiftService;
    private readonly MJGroupService mJGroupService;
    private readonly OneGameService oneGameService;
    private readonly LinkNGameService linkNGameService;
    private readonly FirstHalfService firstHalfService;
    private readonly FantasyService fantasyService;
    private readonly CornerService cornerService;
    private readonly XMinsGroupService xMinsGroupService;

    public QueryTraderSuggestionHandler(
        Repository repository,
         WorkingShiftService workingShiftService,
          MJGroupService mJGroupService,
           OneGameService oneGameService,
            LinkNGameService linkNGameService,
             FirstHalfService firstHalfService,
              FantasyService fantasyService,
               CornerService cornerService,
                XMinsGroupService xMinsGroupService)
    {
        this.repository = repository;
        this.workingShiftService = workingShiftService;
        this.mJGroupService = mJGroupService;
        this.oneGameService = oneGameService;
        this.linkNGameService = linkNGameService;
        this.firstHalfService = firstHalfService;
        this.fantasyService = fantasyService;
        this.cornerService = cornerService;
        this.xMinsGroupService = xMinsGroupService;
    }

    public async Task<(MatchInfo, List<WorkingSchedule>)> Handle(QueryTraderSuggestionRequest request, CancellationToken cancellationToken)
    {
        var matchInfo = await repository.QueryMatchInfo(request.OwpMatchId);

        var workingShift = await workingShiftService.Calculate(matchInfo.EventDate);
        var assignmentModels = await repository.QueryAssignments(matchInfo.EventDate);
        await GroupGame(workingShift, assignmentModels);
        return (matchInfo, workingShift);
    }

    private async Task GroupGame(IList<WorkingSchedule> schedules,
    IList<AssignmentModel> assignmentModels)
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
        }
    }

    private IList<AssignmentModel> GetAssignmentsBelongWs(IList<AssignmentModel> assignments, WorkingSchedule schedule)
    => assignments.Where(x => x.UserId == schedule.UserInfo.UserId
            && DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftStartTime) >= 0
            && DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime) <= 0)
            ?.ToList();
}