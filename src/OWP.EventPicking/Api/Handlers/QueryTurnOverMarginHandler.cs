using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class QueryTurnOverMarginRequest : IRequest<List<LeagueTurnOver>>
{
    public List<string> Leagues { get; set; }

    public Period Period { get; set; }

}

public class QueryTurnOverMarginHandler(QueryEventPickingService queryEventPickingService)
    : IRequestHandler<QueryTurnOverMarginRequest, List<LeagueTurnOver>>
{


    public async Task<List<LeagueTurnOver>> Handle(QueryTurnOverMarginRequest request, CancellationToken cancellationToken)
    {
        return await queryEventPickingService.GetMatchTurnOver(request.Leagues, request.Period);
    }
}