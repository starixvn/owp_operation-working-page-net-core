using OWP.Event.EventShared.Models;

namespace OWP.Event.LiveGameList.Builder;

public static class LengthPropertyBuilder
{
    public static void BuildLengthProperty(MatchInfo matchInfo)
    {
        var assignment = matchInfo.Assignment;
        matchInfo.LengthLeague = matchInfo.League.Length +
                                 matchInfo.MatchOffers.Select(x => x.MatchOfferValue.Length).Sum();
        matchInfo.LengthRemark = matchInfo.Remark.Length + matchInfo.Round.Length + matchInfo.Division.Length;

        matchInfo.LengthTraderHdp = (Math.Max(
            CountRowHeightTrader(assignment.Handicap, assignment.HandicapOthers),
            CountRowHeightTrader(assignment.Handicap1st, assignment.Handicap1stOthers)) + 2) * 20;
        matchInfo.LengthTraderOu = (Math.Max(
            CountRowHeightTrader(assignment.OverUnder, assignment.OverUnderOthers),
            CountRowHeightTrader(assignment.OverUnder1st, assignment.OverUnder1stOthers)) + 2) * 20;
        matchInfo.LengthOtherProductOneXTwo = (CountRowHeightTrader(assignment.OneXTwo, assignment.OneXTwo1stOthers) + 2) * 20;
        matchInfo.LengthOtherProduct1HOneXTwo = (CountRowHeightTrader(assignment.OneXTwo1st, assignment.OneXTwo1stOthers) + 2) * 20;
        var heightProductAllInOne = CountRowHeightTrader(assignment.AllInOne, assignment.AllInOneOthers);
        matchInfo.LengthOtherProductAllInOne = CalculateHeightRow(heightProductAllInOne);
        var heightProductFastMarket = CountRowHeightTrader(assignment.FastMarket, assignment.FastMarketOthers);
        matchInfo.LengthOtherProductFastMarket = CalculateHeightRow(heightProductFastMarket);
        var heightProductFastMarketFifteenMinutes = CountRowHeightTrader(assignment.Specific15Mins, assignment.Specific15MinsOthers);
        matchInfo.LengthOtherProductFifteenMinutes = CalculateHeightRow(heightProductFastMarketFifteenMinutes);
        var heightProductFastMarketTenMinutes = CountRowHeightTrader(assignment.Specific10Mins, assignment.Specific10MinsOthers);

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

    private static int CalculateHeightRow(int number) => Equals(number, 0) ? 0 : number * 20;
}