using MediatR;
using OWP.Extensions;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Models;
using System.Text.RegularExpressions;

namespace OWP.Maxbet.Handlers;

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
        private readonly MaxbetRepository maxbetRepository;

        public Handler(MaxbetRepository maxbetRepository)
        {
            this.maxbetRepository = maxbetRepository;
        }

        public async Task<int?> Handle(Request request, CancellationToken cancellationToken)
        {
            var matchOffers = await maxbetRepository.GetMatchOfferId(request.OWPMatchId);
            var validOfferIds = matchOffers.Where(x => IsValidSecondAcceptValue(x.Value)).ToList();

            return validOfferIds.Count switch
            {
                0 => await BuildSecondAcceptValueDefault(request),
                1 => GetSecondAcceptValue(validOfferIds.FirstOrDefault()!.Value),
                _ => null
            };
        }

        private static int? GetSecondAcceptValue(string matchOffer)
        {
            if (!IsValidSecondAcceptValue(matchOffer))
            {
                return null;
            }

            var resultString = Regex.Match(matchOffer, @"\d+").Value;
            return int.Parse(resultString);
        }

        private static bool IsValidSecondAcceptValue(string matchOffer)
        {
            const string pattern = @"(?i)\d+(s)\s(Accept)";
            return Regex.IsMatch(matchOffer, pattern);
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
            if (bettype is SpecialBettype.BETTYPE_ID_NEXT_CORNERS or SpecialBettype.BETTYPE_ID_NO_OF_CORNERS)
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
    }
}