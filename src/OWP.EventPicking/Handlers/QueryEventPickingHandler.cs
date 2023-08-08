using MediatR;
using OWP.EventPicking.Middleware.Models;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;

namespace OWP.EventPicking.Handlers;

public class QueryEventPickingRequest : EventPickingRequest, IRequest<List<EventPickingModel>>
{ }

public class QueryEventPickingHandler : IRequestHandler<QueryEventPickingRequest, List<EventPickingModel>>
{
    private readonly BuildMainAndChildMatches buildMainAndChildMatches;

    public QueryEventPickingHandler(BuildMainAndChildMatches buildMainAndChildMatches)
    {
        this.buildMainAndChildMatches = buildMainAndChildMatches;
    }

    public async Task<List<EventPickingModel>> Handle(QueryEventPickingRequest request, CancellationToken cancellationToken)
    {
        var matches = await buildMainAndChildMatches.GetMainAndChildMatchEvent(request.Date);

        return FilterAllMatchesByRequest(matches, request);
    }
    public List<EventPickingModel> FilterAllMatchesByRequest(List<EventPickingModel> matches, QueryEventPickingRequest request)
    => matches
        .Where(x => ValidMatch(x, request))
        .ToList();

    private bool ValidMatch(EventPickingModel match, QueryEventPickingRequest condition)
        => CheckCondition1(match, condition) && CheckCondition2(match, condition);

    private bool CheckCondition1(EventPickingModel match, QueryEventPickingRequest condition)
        => condition.SelectGame.Contains(ConvertBoolToInt(match.IsNoOffer))
           && condition.FilterMatchCategory.Contains(ConvertBoolToInt(match.IsSaba));

    private bool CheckCondition2(EventPickingModel match, QueryEventPickingRequest condition)
        => condition.FilterMatch.Contains(match.RowColor, StringComparer.OrdinalIgnoreCase)
           && condition.HasProvider.Contains(match.HasProvider, StringComparer.OrdinalIgnoreCase)
           && condition.HasOtherProvider.Contains(match.HasOtherProvider, StringComparer.OrdinalIgnoreCase);

    private int ConvertBoolToInt(bool value) => value ? 1 : 0;
}