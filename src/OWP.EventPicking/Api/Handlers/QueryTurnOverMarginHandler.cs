using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class QueryTurnOverMarginRequest : IRequest<List<MatchTurnOver>>
{
    public List<EventPickingModel> Matches { get; set; }

    public Period Period { get; set; }

}

public class QueryTurnOverMarginHandler(QueryEventPickingService queryEventPickingService)
    : IRequestHandler<QueryTurnOverMarginRequest, List<MatchTurnOver>>
{


    public async Task<List<MatchTurnOver>> Handle(QueryTurnOverMarginRequest request, CancellationToken cancellationToken)
    {
        var matchTurnOvers = await queryEventPickingService.GetMatchTurnOver(request.Matches, request.Period);
        return matchTurnOvers.Select(x => new MatchTurnOver
        {
            InfoSiteMatchId = x.InfoSiteMatchId,
            LeagueName = x.LeagueName,
            TurnoverPeriod = x.TurnoverPeriod,
            MarginPeriod = x.MarginPeriod
        }).ToList();
    }
}