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
        var childMatchIds = assignmentRaw
            ?.Where(a => a.ParentMatchId.HasValue)
            .Select(a => a.OWPMatchId).ToList();
        var parentMatchInfos = (await repository
            .QueryParentMatchInfo(childMatchIds)).ToList();
        var assignments = EndTimeHelper.Update(assignmentRaw, parentMatchInfos).ToList();
        var sabaAssignments = assignments?.Where(x => x.IsSaba).ToList();
        var realAssignment = assignments?.Except(sabaAssignments).ToList();

        realMatchServices.MapAssignmentsToWorkingSchedule(workingShifts, realAssignment, parentMatchInfos,
           request.FilterDate);
        sabaMatchesServices.MapAssignmentsToWorkingSchedules(workingShifts, sabaAssignments, request.FilterDate);

        return new SuggestionInfo
        {
            MatchInfo = matchInfo,
            FullyAvailable = GetWorkingSchedulesByType(CategoryType.FullyAvailable, workingShifts),
            PartiallyAvailable = GetWorkingSchedulesByType(CategoryType.PartiallyAvailable, workingShifts),
            Busy = GetWorkingSchedulesByType(CategoryType.Busy, workingShifts)
        };
    }

    public List<WorkingSchedule> GetWorkingSchedulesByType(CategoryType type, List<WorkingSchedule> workingShifts)
        => workingShifts.Where(x => x.CategoryType == type)
                        .OrderBy(x => x.UserInfo.Username)
                        .ToList();
}