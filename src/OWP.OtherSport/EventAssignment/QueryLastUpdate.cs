using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.OtherSport.Submit.Models;
using OWP.OtherSport.Submit.Services;

namespace OWP.OtherSport.EventAssignment
{
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
}
