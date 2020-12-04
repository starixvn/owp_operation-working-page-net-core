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

    public class GetNumberOfLinesHandler
    {
        public class Request : IRequest<NumberOfLines>
        {
            public int OWPMatchId { get; set; }
        }

        public class RequestHandler : IRequestHandler<Request, NumberOfLines>
        {
            private static readonly ReadOnlyDictionary<EnumNoOfHdp, NumberOfLines> numberOfLinesDict = new ReadOnlyDictionary<EnumNoOfHdp, NumberOfLines>(
                new Dictionary <EnumNoOfHdp, NumberOfLines>
                {
                    { EnumNoOfHdp.TwoPlusFourLines, new NumberOfLines { CreditFt = 2 , CreditFirstHaft = 2, LicenseeFt = 4, LicenseeFirstHaft = 3 } },
                    { EnumNoOfHdp.ThreePlusThreeLines, new NumberOfLines { CreditFt = 3 , CreditFirstHaft = 2, LicenseeFt = 3, LicenseeFirstHaft = 2 } },
                    { EnumNoOfHdp.ThreePlusFourLines, new NumberOfLines { CreditFt = 3 , CreditFirstHaft = 2, LicenseeFt = 4, LicenseeFirstHaft = 3 } },
                    { EnumNoOfHdp.FourPlusFourLines, new NumberOfLines { CreditFt = 4 , CreditFirstHaft = 3, LicenseeFt = 4, LicenseeFirstHaft = 3 } },
                    { EnumNoOfHdp.Empty, new NumberOfLines { CreditFt = 2 , CreditFirstHaft = 2, LicenseeFt = 3, LicenseeFirstHaft = 2 } }
                }
            );

            private readonly IDynamicRepository dynamicRepository;

            public RequestHandler(IDynamicRepository dynamicRepository)
            {
                this.dynamicRepository = dynamicRepository;
            }

            public async Task<NumberOfLines> Handle(Request request, CancellationToken cancellationToken)
            {
                var noOfHdp = await GetMatchNoOfHDPId(request.OWPMatchId);
                return BuildNumberOfLines(noOfHdp);
            }

            private async Task<int> GetMatchNoOfHDPId(int matchId)
            {
                var criteria = new MatchNoOfHdpGetCriteria
                {
                    OWPMatchId = matchId
                };

                var noOfHdp = await dynamicRepository.FetchAsync<MatchResource>(criteria);

                return noOfHdp.Any() ? noOfHdp.First().ResourceId : 0;
            }

            private NumberOfLines BuildNumberOfLines(int noOfHdpId)
            {
                if (!numberOfLinesDict.TryGetValue((EnumNoOfHdp)noOfHdpId, out NumberOfLines result))
                {
                    return new NumberOfLines();
                }

                return result;
            }
        }
    }
}