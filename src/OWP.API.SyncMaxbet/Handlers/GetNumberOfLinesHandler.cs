namespace OWP.API.SyncMaxbet.Handlers
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.API.SyncMaxbet.Criterias;
    using OWP.API.SyncMaxbet.Models;
    using OWP.Enums;

    public class GetNumberOfLinesHandler
    {
        public class Request : IRequest<NumberOfLines>
        {
            public int OWPMatchId { get; set; }
        }

        public class RequestHandler/* : IRequestHandler<Request, NumberOfLines>*/
        {
            //private static readonly ReadOnlyDictionary<EnumNoOfHdp, NumberOfLines> dictionary = new ReadOnlyDictionary<EnumNoOfHdp, NumberOfLines>(
            //    );

            //private readonly IDynamicRepository dynamicRepository;

            //private RequestHandler(IDynamicRepository dynamicRepository)
            //{
            //    this.dynamicRepository = dynamicRepository;
            //}

            //public async Task<NumberOfLines> Handle(Request request, CancellationToken cancellationToken)
            //{
            //    var noOfHdp = await GetMatchNoOfHDP(request.OWPMatchId);


            //}

            //private async Task<MatchResource> GetMatchNoOfHDP(int matchId)
            //{
            //    var criteria = new MatchNoOfHdpGetCriteria
            //    { 
            //        OWPMatchId = matchId 
            //    };

            //    var result = await dynamicRepository.FetchAsync<MatchResource>(criteria);

            //    return result.FirstOrDefault();
            //}


        }
    }
}