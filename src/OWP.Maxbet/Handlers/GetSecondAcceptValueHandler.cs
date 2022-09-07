using OWP.Maxbet.Models;

namespace OWP.Maxbet.Handlers;

using MediatR;
using OWP.Enums;
using OWP.Extensions;
using OWP.Maxbet.Constants;

using System.Collections.ObjectModel;

public class GetSecondAcceptValueHandler
{
    public class Request : IRequest<int?>
    {
        public int OWPMatchId { get; set; }

        public decimal? MaxbetHdp { get; set; }

        public int bettype { get; set; }
    }

    public class Handler : IRequestHandler<Request, int?>
    {
        private static readonly ReadOnlyDictionary<EnumMatchOffer, int> numberOfLinesDict = new ReadOnlyDictionary<EnumMatchOffer, int>(
            new Dictionary<EnumMatchOffer, int>
            {
                    { EnumMatchOffer.Accept10s, 10 },
                    { EnumMatchOffer.Accept15s, 15  },
                    { EnumMatchOffer.Accept20s, 20 },
                    { EnumMatchOffer.Accept30s, 30 },
                    { EnumMatchOffer.Accept5s, 5 },
                    { EnumMatchOffer.Accept8s, 8 },
                    { EnumMatchOffer.Accept12s, 12 },
                    { EnumMatchOffer.Accept6s, 6 }
            }
        );

        private readonly MaxbetRepository maxbetRepository;

        public Handler(MaxbetRepository maxbetRepository)
        {
            this.maxbetRepository = maxbetRepository;
        }

        public async Task<int?> Handle(Request request, CancellationToken cancellationToken)
        {
            var matchOfferId = await maxbetRepository.GetMatchOfferId(request.OWPMatchId);
            return BuildSecondAcceptValue(matchOfferId) ?? await BuildSecondAcceptValueDefault(request);
        }

        private static int? BuildSecondAcceptValue(int matchOfferId)
        {
            if (!numberOfLinesDict.TryGetValue((EnumMatchOffer)matchOfferId, out int result))
            {
                return null;
            }

            return result;
        }

        private async Task<int?> BuildSecondAcceptValueDefault(Request request)
        {
            if (!request.MaxbetHdp.HasValue)
            {
                return null;
            }
            var (secondAccept, leagueName) =
                await maxbetRepository.GetSecondAcceptValueDefaultInfo(request.MaxbetHdp.Value, request.OWPMatchId);
            if (leagueName.IsContains("FANTASY"))
            {
                return secondAccept?.Fantasy;
            }

            return BuildSecondAcceptValueSpecialDefault(request.bettype, secondAccept);
        }

        private int? BuildSecondAcceptValueSpecialDefault(int bettype,
            SecondAcceptDefault secondAccept)
        {
            if (bettype == SpecialBettype.BETTYPE_ID_NEXT_CORNERS ||
                bettype == SpecialBettype.BETTYPE_ID_NO_OF_CORNERS)
            {
                return secondAccept?.Corner;
            }

            if (bettype == SpecialBettype.BETTYPE_ID_TOTAL_BOOKINGS)
            {
                return secondAccept?.TotalBooking;
            }

            if (bettype == SpecialBettype.BETTYPE_ID_MAIN_MATCH)
            {
                return secondAccept?.Main;
            }

            return null;
        }
    }
}