using MediatR;
using OWP.PrematchAssignment.AutoAssignment.Helper;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.AutoAssignment.Handlers;

public class AutoAssignRequest : QueryDayShiftPreMatchRequest, IRequest<QueryPreMatchLeagueViewModel>;

public class AutoAssignHandler(IMediator mediator) : IRequestHandler<AutoAssignRequest, QueryPreMatchLeagueViewModel>
{
    public async Task<QueryPreMatchLeagueViewModel> Handle(AutoAssignRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        var model = PreprocessData(result);

        return result;
    }

    public QueryPreMatchLeagueViewModel PreprocessData(QueryPreMatchLeagueViewModel model)
    {
        //excludeService
        var filteredModel = ExcludeUnSupportedLeaguesHelper.Filter(model);

        //groupService


        return filteredModel;
    }
}
