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
    {
        var filteredMatches = matches.Where(x => request.FilterMatch.Contains(x.RowColor, StringComparer.OrdinalIgnoreCase));
        var filteredSelectGame = filteredMatches.Where(x => request.SelectGame.Contains(ConvertBoolToInt(x.IsNoOffer)));

        return filteredSelectGame.ToList();
    }

    private int ConvertBoolToInt(bool value) => value ? 1 : 0;
}