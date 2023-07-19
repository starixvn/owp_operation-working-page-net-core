using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;

namespace OWP.AssigneeSuggestionList.Handlers;

public class QueryTraderSuggestionRequest : IRequest<SuggestionInfo>
{
    public int OwpMatchId { get; set; }

    public int BetTypeId { get; set; }

    public DateTime FilterDate { get; set; }

    public DateTime? OtherTime { get; set; }
}

public class QueryTraderSuggestionHandler : IRequestHandler<QueryTraderSuggestionRequest, SuggestionInfo>
{
    private readonly Repository repository;
    private readonly WorkingShiftService workingShiftService;
    private readonly SabaMatchServices sabaMatchesServices;
    private readonly RealMatchServices realMatchServices;
    private readonly MatchInfoServices matchInfoServices;

    public QueryTraderSuggestionHandler(
        Repository repository,
        WorkingShiftService workingShiftService,
        SabaMatchServices sabaMatchesServices,
        RealMatchServices realMatchServices,
        MatchInfoServices matchInfoServices)
    {
        this.repository = repository;
        this.workingShiftService = workingShiftService;
        this.sabaMatchesServices = sabaMatchesServices;
        this.realMatchServices = realMatchServices;
        this.matchInfoServices = matchInfoServices;
    }

    public async Task<SuggestionInfo> Handle(QueryTraderSuggestionRequest request, CancellationToken cancellationToken)
    {
        var matchInfo = await matchInfoServices.GetMatchInfo(request);
        var workingShifts = await workingShiftService.Calculate(request.FilterDate);
        var assignmentRaw = await repository.QueryAssignments(request.FilterDate);

        MapAssignmentToWorkingSchedule(request, assignmentRaw, workingShifts);
        return CategoryAssignees(matchInfo, workingShifts);
    }

    private void MapAssignmentToWorkingSchedule(
        QueryTraderSuggestionRequest request,
        IList<AssignmentModel> assignmentRaw,
        IList<WorkingSchedule> workingShifts)
    {
        var assignments = EndTimeHelper.Update(assignmentRaw).ToList();
        var sabaAssignments = assignments.Where(x => x.IsSaba).ToList();
        var realAssignment = assignments.Except(sabaAssignments).ToList();

        realMatchServices.MapAssignmentsToWorkingSchedule(workingShifts, realAssignment, request.FilterDate);
        sabaMatchesServices.MapAssignmentsToWorkingSchedules(workingShifts, sabaAssignments, request.FilterDate);
    }

    private SuggestionInfo CategoryAssignees(MatchInfo matchInfo, List<WorkingSchedule> workingShifts)
    {
        if (matchInfo.IsSaba)
        {
            return new SuggestionInfo
            {
                MatchInfo = matchInfo,
                Busy = workingShifts
            };
        }

        realMatchServices.CategoryAssignee(workingShifts, matchInfo);
        return new SuggestionInfo
        {
            MatchInfo = matchInfo,
            FullyAvailable = GetWorkingSchedulesByType(CategoryType.FullyAvailable, workingShifts, matchInfo),
            PartiallyAvailable = GetWorkingSchedulesByType(CategoryType.PartiallyAvailable, workingShifts, matchInfo),
            Busy = GetWorkingSchedulesByType(CategoryType.Busy, workingShifts, matchInfo)
        };
    }

    private static List<WorkingSchedule> GetWorkingSchedulesByType(CategoryType type,
        IEnumerable<WorkingSchedule> workingShifts, MatchInfo matchInfo)
    {
        var workingShiftsByType = workingShifts.Where(x => x.CategoryType == type).ToList();
        return OrderAssigneeHelper.SortAssignee(matchInfo, workingShiftsByType);
    }

}