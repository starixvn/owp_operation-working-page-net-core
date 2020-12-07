namespace OWP.API.SyncMaxbet.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.API.SyncMaxbet.Criterias;
    using OWP.API.SyncMaxbet.Models;

    public class GetMaxbetCN88Handler
    {
        public class Request : IRequest<MaxbetSpread>
        {
            public int OWPMatchId { get; set; }
        }

        public class Handler : IRequestHandler<Request, MaxbetSpread>
        {
            private readonly IDynamicRepository dynamicRepository;

            public Handler(IDynamicRepository dynamicRepository)
            {
                this.dynamicRepository = dynamicRepository;
            }

            public Task<MaxbetSpread> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }

            private async Task<IEnumerable<Maxbet>> GetMaxbet(int matchId)
                => await dynamicRepository.FetchAsync<Maxbet>(new MaxbetGetCriteria { OWPMatchId = matchId, IsCN88 = true });
        }
    }
}