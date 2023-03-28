using MediatR;
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

    public QueryTraderSuggestionHandler(Repository repository, WorkingShiftService workingShiftService)
    {
        this.repository = repository;
        this.workingShiftService = workingShiftService;
    }

    public async Task<(MatchInfo, List<WorkingSchedule>)> Handle(QueryTraderSuggestionRequest request, CancellationToken cancellationToken)
    {
        var matchInfo = await repository.QueryMatchInfo(request.OwpMatchId);

        var workingShift = await workingShiftService.Calculate(matchInfo.EventDate);

        return (matchInfo, workingShift);
    }
}