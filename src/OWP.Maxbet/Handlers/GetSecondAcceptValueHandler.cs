namespace OWP.Maxbet.Handlers;

using System.Collections.ObjectModel;
using Constants;
using Enums;
using Extensions;
using MediatR;
using Models;

public class GetSecondAcceptValueHandler
{
    public class Request : IRequest<int?>
    {
        public int OWPMatchId { get; set; }

        public decimal? MaxbetHdp { get; set; }

        public int Bettype { get; set; }
    }

    public class Handler : IRequestHandler<Request, int?>
    {
        private static readonly ReadOnlyDictionary<EnumMatchOffer, int> numberOfLinesDict = new(
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
            var matchOfferIds = (await maxbetRepository.GetMatchOfferId(request.OWPMatchId))
                .ToList();

            var validOfferIds = GetMatchOfferIdValid(matchOfferIds);

            if (validOfferIds.Count > 1 || IsOnlyExistIgnoreIds(matchOfferIds))
            {
                return null;
            }

            var offerId = validOfferIds.FirstOrDefault(new MatchResource());

            return GetSecondAcceptValue(offerId.ResourceId) ?? await BuildSecondAcceptValueDefault(request);
        }

        private static int? GetSecondAcceptValue(int matchOfferId)
        {
            if (!numberOfLinesDict.TryGetValue((EnumMatchOffer)matchOfferId, out var result))
            {
                return null;
            }

            return result;
        }

        private async Task<int?> BuildSecondAcceptValueDefault(Request request)
        {
            var (secondAccept, leagueName) =
                await maxbetRepository.GetSecondAcceptValueDefaultInfo(request.MaxbetHdp, request.OWPMatchId);
            if (leagueName.IsContains("FANTASY"))
            {
                return GetSecondAcceptValue(secondAccept.Fantasy);
            }

            return BuildSecondAcceptValueSpecialDefault(request.Bettype, secondAccept);
        }

        private int? BuildSecondAcceptValueSpecialDefault(int bettype,
            SecondAcceptDefault secondAccept)
        {
            if (bettype == SpecialBettype.BETTYPE_ID_NEXT_CORNERS ||
                bettype == SpecialBettype.BETTYPE_ID_NO_OF_CORNERS)
            {
                return GetSecondAcceptValue(secondAccept.Corner);
            }

            if (bettype == SpecialBettype.BETTYPE_ID_TOTAL_BOOKINGS)
            {
                return GetSecondAcceptValue(secondAccept.TotalBooking);
            }

            if (bettype == SpecialBettype.BETTYPE_ID_MAIN_MATCH)
            {
                return GetSecondAcceptValue(secondAccept.Main);
            }

            return null;
        }

        private bool IsOnlyExistIgnoreIds(IList<MatchResource> matchOfferIds)
            => matchOfferIds.Count > 0 &&
               matchOfferIds.All(x => Array.Exists(MaxBetSetting.IGNORE_MATCH_OFFER_ID, i => i == x.ResourceId));

        private List<MatchResource> GetMatchOfferIdValid(IList<MatchResource> matchOfferIds)
            => matchOfferIds
                .Where(x => x.ResourceId != (int)EnumMatchOffer.KO15 && x.ResourceId != (int)EnumMatchOffer.FastMarket)
                .ToList();

    }
}