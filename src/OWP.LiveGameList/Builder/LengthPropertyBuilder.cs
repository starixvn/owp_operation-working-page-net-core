using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.LiveGameList.Builder
{
    using Models;

    public static class LengthPropertyBuilder
    {
        public static void BuildLengthProperty(MatchInfo matchInfo)
        {
            matchInfo.LengthLeague = matchInfo.League.Length +
                                     matchInfo.MatchOffers.Select(x => x.MatchOfferValue.Length).Sum();

            matchInfo.LengthRemark = matchInfo.Remark.Length + matchInfo.Round.Length + matchInfo.Division.Length;


            matchInfo.LengthTraderHdp = (Math.Max(
                CountRowHeightTrader(matchInfo.Assignment.Handicap, matchInfo.Assignment.HandicapOthers)
                , CountRowHeightTrader(matchInfo.Assignment.Handicap1st, matchInfo.Assignment.Handicap1stOthers)) + 2) * 20;

            matchInfo.LengthTraderOu = (Math.Max(
                CountRowHeightTrader(matchInfo.Assignment.OverUnder, matchInfo.Assignment.OverUnderOthers)
                , CountRowHeightTrader(matchInfo.Assignment.OverUnder1st, matchInfo.Assignment.OverUnder1stOthers)) + 2) * 20;

            matchInfo.LengthOtherProductOneXTwo =
                (CountRowHeightTrader(matchInfo.Assignment.OneXTwo, matchInfo.Assignment.OneXTwo1stOthers) + 2) * 20;

            matchInfo.LengthOtherProduct1HOneXTwo =
                (CountRowHeightTrader(matchInfo.Assignment.OneXTwo1st, matchInfo.Assignment.OneXTwo1stOthers) + 2) * 20;

            var heightProductAllInOne =
                CountRowHeightTrader(matchInfo.Assignment.AllInOne, matchInfo.Assignment.AllInOneOthers);

            matchInfo.LengthOtherProductAllInOne = CalculateHeightRow(heightProductAllInOne);

            var heightProductFastMarket =
                CountRowHeightTrader(matchInfo.Assignment.FastMarket, matchInfo.Assignment.FastMarketOthers);

            matchInfo.LengthOtherProductFastMarket = CalculateHeightRow(heightProductFastMarket);

            var heightProductFastMarketFifteenMinutes = CountRowHeightTrader(matchInfo.Assignment.Specific15Mins,
                matchInfo.Assignment.Specific15MinsOthers);

            matchInfo.LengthOtherProductFifteenMinutes = CalculateHeightRow(heightProductFastMarketFifteenMinutes);

            var heightProductFastMarketTenMinutes = CountRowHeightTrader(matchInfo.Assignment.Specific10Mins,
                matchInfo.Assignment.Specific10MinsOthers);

            matchInfo.LengthOtherProductTenMinutes = CalculateHeightRow(heightProductFastMarketTenMinutes);

            matchInfo.LengthIpTv = matchInfo.Televisions.Select(x => x.TVCode.Length * 4).Sum();

            var lengthProvider = string.IsNullOrEmpty(matchInfo.Provider) ? 0 : matchInfo.Provider.Length * 4;

            matchInfo.LengthProvider = lengthProvider + matchInfo.WatchBet.Length + matchInfo.Phone.Length;
        }
        private static int CountRowHeightTrader(IEnumerable<Assignment> assignments, IEnumerable<Assignment> assignmentsOther)
        {
            return assignments.Count(x => x.Username != null) +
                   assignmentsOther.Count(x => x.AssignmentNote != null);
        }

        private static int CalculateHeightRow(int number)
        {
            return Equals(number, 0) ? 0 : (number + 2) * 20;
        }
    }
}
