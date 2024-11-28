using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class QueryEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>;

public class QueryEventPickingHandler(QueryEventPickingService queryEventPickingService)
    : IRequestHandler<QueryEventPickingRequest, EventPickingResult>
{
    public async Task<EventPickingResult> Handle(QueryEventPickingRequest request, CancellationToken cancellationToken)
        => await queryEventPickingService.Execute(request);
}