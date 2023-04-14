using MediatR;
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

    public QueryTraderSuggestionHandler(
        Repository repository,
        WorkingShiftService workingShiftService,
        SabaMatchServices sabaMatchesServices,
        RealMatchServices realMatchServices)
    {
        this.repository = repository;
        this.workingShiftService = workingShiftService;
        this.sabaMatchesServices = sabaMatchesServices;
        this.realMatchServices = realMatchServices;
    }

    public async Task<SuggestionInfo> Handle(QueryTraderSuggestionRequest request, CancellationToken cancellationToken)
    {
        var matchInfo = await repository.QueryMatchInfo(request.OwpMatchId);
        if (matchInfo == null)
        {
            return new SuggestionInfo();
        }
        matchInfo.BettypeId = request.BetTypeId;
        matchInfo.FilterDate = request.FilterDate;

        var workingShifts = await workingShiftService.Calculate(request.FilterDate);
        var assignmentRaw = await repository.QueryAssignments(request.FilterDate);
        var parentMatchInfos = await GetParentMatchInfo(assignmentRaw);

        var sabaAssignments = assignmentRaw.Where(x => x.IsSaba).ToList();
        var realAssignmentRaw = assignmentRaw.Except(sabaAssignments).ToList();
        var realAssignments = EndTimeHelper.Update(realAssignmentRaw, parentMatchInfos).ToList();

        await realMatchServices.MapAssignmentsToWorkingSchedule(workingShifts, realAssignments, parentMatchInfos, request.FilterDate);
        sabaMatchesServices.MapAssignmentsToWorkingSchedules(workingShifts, sabaAssignments, request.FilterDate);

        return new SuggestionInfo
        {
            MatchInfo = matchInfo,
            WorkingSchedules = workingShifts.OrderBy(x => x.UserInfo.Username).ToList()
        };
    }

    public async Task<IList<ParentMatchInfo>> GetParentMatchInfo(IList<AssignmentModel> assignmentModels)
    {
        var owpMatchId = assignmentModels
                             ?.Where(a => a.ParentMatchId.HasValue)
                             .Select(a => a.OWPMatchId)
                         ?? new List<int>();

        return (await repository.QueryParentMatchInfo(string.Join(",", owpMatchId)))?.ToList();
    }

}