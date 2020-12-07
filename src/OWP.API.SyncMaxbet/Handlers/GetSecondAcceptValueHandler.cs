namespace OWP.API.SyncMaxbet.Handlers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.API.SyncMaxbet.Criterias;
    using OWP.API.SyncMaxbet.Models;
    using OWP.Enums;

    public class GetSecondAcceptValueHandler
    {
        public class Request: IRequest<int>
        {
            public int OWPMatchId { get; set; }
        }

        public class Handler : IRequestHandler<Request, int>
        {
            private static readonly ReadOnlyDictionary<EnumMatchOffer, int> numberOfLinesDict = new ReadOnlyDictionary<EnumMatchOffer, int>(
                new Dictionary<EnumMatchOffer, int>
                {
                    { EnumMatchOffer.Accept10s, 10 },
                    { EnumMatchOffer.Accept15s, 15  },
                    { EnumMatchOffer.Accept20s, 20 },
                    { EnumMatchOffer.Accept30s, 30 },
                    { EnumMatchOffer.Accept5s, 5 }
                }
            );

            private readonly IDynamicRepository dynamicRepository;

            public Handler(IDynamicRepository dynamicRepository)
            {
                this.dynamicRepository = dynamicRepository;
            }

            public async Task<int> Handle(Request request, CancellationToken cancellationToken)
            {
                var matchOfferId = await GetMatchOfferId(request.OWPMatchId);
                return BuildSecondAcceptValue(matchOfferId);
            }

            private async Task<int> GetMatchOfferId(int matchId)
            {
                var criteria = new MatchOfferGetCriteria
                {
                    OWPMatchId = matchId
                };

                var matchOffer = await dynamicRepository.FetchAsync<MatchResource>(criteria);

                return matchOffer.Any() ? matchOffer.First().ResourceId : 0;
            }

            private int BuildSecondAcceptValue(int matchOfferId)
            {
                if (!numberOfLinesDict.TryGetValue((EnumMatchOffer)matchOfferId, out int result))
                {
                    return 0;
                }

                return result;
            }
        }
    }
}