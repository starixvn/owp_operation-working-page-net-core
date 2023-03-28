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
        var (assignmentModel, maxbetModel) = await repository.QueryAssignments(matchInfo.EventDate);

        return (matchInfo, workingShift);
    }

    private async Task GroupGame(IList<WorkingSchedule> schedules,
    IList<AssignmentModel> assignmentModels,
    IList<MaxbetModel> maxbetModels)
    {
        foreach (var schedule in schedules)
        {
            var assignments = assignmentModels.Where(x => x.UserId == schedule.UserInfo.UserId
            && x.EventDateTime >= schedule.UserInfo.ShiftStartTime
            && x.EventDateTime <= schedule.UserInfo.ShiftStartTime);

        }
    }
}