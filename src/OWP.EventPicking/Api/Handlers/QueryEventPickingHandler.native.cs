using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class QueryEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>;

public class QueryEventPickingHandler : IRequestHandler<QueryEventPickingRequest, EventPickingResult>
{
    private readonly QueryEventPickingService queryEventPickingService;

    public QueryEventPickingHandler(QueryEventPickingService queryEventPickingService)
    {
        this.queryEventPickingService = queryEventPickingService;
    }

    public async Task<EventPickingResult> Handle(QueryEventPickingRequest request, CancellationToken cancellationToken)
    => await queryEventPickingService.Execute(request);

}