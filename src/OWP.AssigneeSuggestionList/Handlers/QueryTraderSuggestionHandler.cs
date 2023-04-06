using MediatR;
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

        var workingShifts = await workingShiftService.Calculate(request.FilterDate);
        var assignmentModels = await repository.QueryAssignments(request.FilterDate);

        await realMatchServices.MapAssignmentsToWorkingSchedule(workingShifts, assignmentModels);
        sabaMatchesServices.MapAssignmentsToWorkingSchedules(workingShifts, assignmentModels, request.FilterDate);

        return new SuggestionInfo
        {
            MatchInfo = matchInfo,
            WorkingSchedules = workingShifts
        };
    }
}