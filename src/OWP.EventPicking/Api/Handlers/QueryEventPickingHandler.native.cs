using Olympus.OWP.Constants;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class QueryEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>;

public class QueryEventPickingHandler(
    QueryEventPickingService queryEventPickingService,
    Repository repository,
    SyncEventPickingInfo syncEventPickingInfo) : IRequestHandler<QueryEventPickingRequest, EventPickingResult>
{
    private static readonly short[] _excludedUpdateTypes = [
        UpdateTypeConstant.UPDATE_REMARK2,
        UpdateTypeConstant.CANCEL_REMARK2,
        UpdateTypeConstant.UPDATE_WEB1,
        UpdateTypeConstant.CANCEL_WEB1,
        UpdateTypeConstant.UPDATE_WEB2,
        UpdateTypeConstant.CANCEL_WEB2,
        UpdateTypeConstant.UPDATE_WEB3,
        UpdateTypeConstant.CANCEL_WEB3,
        UpdateTypeConstant.UPDATE_WEB4,
        UpdateTypeConstant.CANCEL_WEB4,
        UpdateTypeConstant.UPDATE_LINK,
        UpdateTypeConstant.CANCEL_LINK
    ];

    public async Task<EventPickingResult> Handle(QueryEventPickingRequest request, CancellationToken cancellationToken)
    {
        var mainMatches = await queryEventPickingService.Execute(request, _excludedUpdateTypes);
        var leagueSetting = (await repository.QueryLeagueSetting()).ToList();
        var tvList = (await repository.GetTVList()).ToList();
        var matchFiltered = mainMatches.FilteredMatches;

        syncEventPickingInfo.Execute(matchFiltered, leagueSetting, tvList);

        return mainMatches;
    }
}