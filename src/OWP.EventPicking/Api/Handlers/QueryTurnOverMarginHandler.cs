using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class QueryTurnOverMarginRequest : IRequest<List<MatchTurnOver>>
{
    List<MatchTurnOver> MatchTurnOvers { get; set; }

    Period Period { get; set; }

}

public class QueryTurnOverMarginHandler(QueryEventPickingService queryEventPickingService)
    : IRequestHandler<QueryTurnOverMarginRequest, List<MatchTurnOver>>
{


    public async Task<List<MatchTurnOver>> Handle(QueryTurnOverMarginRequest request, CancellationToken cancellationToken)
    => new List<MatchTurnOver>();
}