

namespace OWP.EventPicking.Handlers;

public class QueryEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>
{ }

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