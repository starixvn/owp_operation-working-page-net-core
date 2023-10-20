using MediatR;
using OWP.EventPicking.Middleware.Models;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;

namespace OWP.EventPicking.Handlers;

public class QueryEventPickingRequest : EventPickingRequest, IRequest<List<EventPickingModel>>
{ }

public class QueryEventPickingHandler : IRequestHandler<QueryEventPickingRequest, List<EventPickingModel>>
{
    private readonly QueryEventPickingService queryEventPickingService;

    public QueryEventPickingHandler(QueryEventPickingService queryEventPickingService)
    {
        this.queryEventPickingService = queryEventPickingService;
    }

    public async Task<List<EventPickingModel>> Handle(QueryEventPickingRequest request, CancellationToken cancellationToken)
    => await queryEventPickingService.Execute(request);

}