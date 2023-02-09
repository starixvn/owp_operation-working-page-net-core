namespace OWP.OtherSport.EventAssignment;

using MediatR;
using Submit.Models;
using Submit.Services;

public class QueryLastUpdate : IRequest<LastUpdateInfo>
{
}

public class LastUpdateHandler : IRequestHandler<QueryLastUpdate, LastUpdateInfo>
{
    private readonly CacheManipulator cacheManipulator;

    public LastUpdateHandler(CacheManipulator cacheManipulator)
    {
        this.cacheManipulator = cacheManipulator;
    }

    public async Task<LastUpdateInfo> Handle(QueryLastUpdate request, CancellationToken cancellationToken)
        => await cacheManipulator.GetLastUpdate();
}