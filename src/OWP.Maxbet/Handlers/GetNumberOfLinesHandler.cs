namespace OWP.Maxbet.Handlers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.Maxbet.Criterias;
    using OWP.Maxbet.Models;
    using OWP.Enums;

    public class GetNumberOfLinesHandler
    {
        public class Request : IRequest<NumberOfLines>
        {
            public int OWPMatchId { get; set; }
        }

        public class Handler : IRequestHandler<Request, NumberOfLines>
        {
            private static readonly ReadOnlyDictionary<EnumNoOfHdp, NumberOfLines> numberOfLinesDict = new ReadOnlyDictionary<EnumNoOfHdp, NumberOfLines>(
                new Dictionary<EnumNoOfHdp, NumberOfLines>
                {
                    { EnumNoOfHdp.TwoPlusFourLines, new NumberOfLines { CreditFt = 2 , CreditFirstHalf = 2, LicenseeFt = 4, LicenseeFirstHalf = 3 } },
                    { EnumNoOfHdp.ThreePlusThreeLines, new NumberOfLines { CreditFt = 3 , CreditFirstHalf = 2, LicenseeFt = 3, LicenseeFirstHalf = 2 } },
                    { EnumNoOfHdp.ThreePlusFourLines, new NumberOfLines { CreditFt = 3 , CreditFirstHalf = 2, LicenseeFt = 4, LicenseeFirstHalf = 3 } },
                    { EnumNoOfHdp.FourPlusFourLines, new NumberOfLines { CreditFt = 4 , CreditFirstHalf = 3, LicenseeFt = 4, LicenseeFirstHalf = 3 } },
                    { EnumNoOfHdp.Empty, new NumberOfLines { CreditFt = 2 , CreditFirstHalf = 2, LicenseeFt = 3, LicenseeFirstHalf = 2 } }
                }
            );

            private readonly IDynamicRepository dynamicRepository;

            public Handler(IDynamicRepository dynamicRepository)
            {
                this.dynamicRepository = dynamicRepository;
            }

            public async Task<NumberOfLines> Handle(Request request, CancellationToken cancellationToken)
            {
                var noOfHdpId = await GetMatchNoOfHDPId(request.OWPMatchId);
                return BuildNumberOfLines(noOfHdpId);
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