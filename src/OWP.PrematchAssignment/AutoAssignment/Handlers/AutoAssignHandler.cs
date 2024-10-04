using MediatR;
using OWP.PrematchAssignment.AutoAssignment.Services;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.AutoAssignment.Handlers;

public class AutoAssignRequest : QueryDayShiftPreMatchRequest, IRequest<QueryPreMatchLeagueViewModel>;

public class AutoAssignHandler(IMediator mediator, PreprocessDataService preprocessData) : IRequestHandler<AutoAssignRequest, QueryPreMatchLeagueViewModel>
{
    public async Task<QueryPreMatchLeagueViewModel> Handle(AutoAssignRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        var model = preprocessData.Handle(result);

        return result;
    }
}
