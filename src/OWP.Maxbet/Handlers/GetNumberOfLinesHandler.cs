using Fanex.Data.Repository;
using MediatR;
using OWP.Enums;
using OWP.Maxbet.Criteria;
using OWP.Maxbet.Models;
using System.Collections.ObjectModel;

namespace OWP.Maxbet.Handlers;

public class GetNumberOfLinesHandler
{
    public class Request : IRequest<NumberOfLines>
    {
        public int OWPMatchId { get; set; }
    }

    public class Handler : IRequestHandler<Request, NumberOfLines>
    {
        private static readonly ReadOnlyDictionary<EnumNoOfHdp, NumberOfLines> numberOfLinesDict = new(
            new Dictionary<EnumNoOfHdp, NumberOfLines>
            {
                { EnumNoOfHdp.TwoPlusFourLines, new NumberOfLines { CreditFt = 2 , CreditFirstHalf = 2, LicenseeFt = 4, LicenseeFirstHalf = 3 } },
                { EnumNoOfHdp.ThreePlusThreeLines, new NumberOfLines { CreditFt = 3 , CreditFirstHalf = 2, LicenseeFt = 3, LicenseeFirstHalf = 2 } },
                { EnumNoOfHdp.ThreePlusFourLines, new NumberOfLines { CreditFt = 3 , CreditFirstHalf = 2, LicenseeFt = 4, LicenseeFirstHalf = 3 } },
                { EnumNoOfHdp.FourPlusFourLines, new NumberOfLines { CreditFt = 4 , CreditFirstHalf = 3, LicenseeFt = 4, LicenseeFirstHalf = 3 } },
                { EnumNoOfHdp.OneLine, new NumberOfLines { CreditFt = 1 , CreditFirstHalf = 1, LicenseeFt = 0, LicenseeFirstHalf = 0 } },
                { EnumNoOfHdp.TwoLines, new NumberOfLines { CreditFt = 2 , CreditFirstHalf = 2, LicenseeFt = 0, LicenseeFirstHalf = 0 } },
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

            var noOfHdp = (await dynamicRepository.FetchAsync<MatchResource>(criteria)).ToList();

            return noOfHdp.Any() ? noOfHdp[0].ResourceId : 0;
        }

        private static NumberOfLines BuildNumberOfLines(int noOfHdpId)
        {
            return !numberOfLinesDict.TryGetValue((EnumNoOfHdp)noOfHdpId, out var result) ? new NumberOfLines() : result;
        }
    }
}