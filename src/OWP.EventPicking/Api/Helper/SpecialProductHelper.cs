using OWP.Constants;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Helper;

public static class SpecialProductHelper
{
    public static void BuildSpecialProduct<T>(IList<T> events) where T : class, EventProductCommon
    {
        foreach (var @event in events)
        {
            var (leaguePostfix, teamPostfix) = BuildPostFixSpecialProduct(@event.MinorBettypeId);
            @event.LeagueName = $"{@event.LeagueName} {leaguePostfix}";
            @event.HomeName = $"{@event.HomeName} {teamPostfix}";
            @event.AwayName = $"{@event.AwayName} {teamPostfix}";
        }
    }

    public static List<T> CombineMainAndChildMatchAndAddMatchNo<T>(IList<T> mainMatches, IList<T> childMatches) where T : class, EventProductCommon
    {
        var matches = new List<T>();
        mainMatches = SortMatches(mainMatches);

        var matchNo = 1;
        foreach (var mainMatch in mainMatches)
        {
            var childMatchesOfThisMainMatch = childMatches.Where(childMatch => childMatch.InfoSiteMatchId == mainMatch.InfoSiteMatchId).ToList();
            if (AssignMatchNo(mainMatch, matchNo, childMatchesOfThisMainMatch))
            {
                matchNo++;
            }

            matches.Add(mainMatch);
            matches.AddRange(childMatchesOfThisMainMatch);
        }

        return matches.ToList();
    }

    private static bool AssignMatchNo<T>(T mainMatch, int matchNo, List<T> childMatchesOfThisMainMatch) where T : class, EventProductCommon
    {
        if (mainMatch.IsSaba || mainMatch.IsNoOffer)
        {
            return false;
        }

        mainMatch.MatchNo = MatchNoHelper.BuildMainMatchNo(matchNo);
        foreach (var childMatch in childMatchesOfThisMainMatch)
        {
            childMatch.MatchNo = MatchNoHelper.BuildChildMatchNo(matchNo, childMatch.MinorBettypeId);
        }

        childMatchesOfThisMainMatch.Sort((x, y) => string.CompareOrdinal(x.MatchNo, y.MatchNo));
        return true;
    }

    private static List<T> SortMatches<T>(IList<T> mainMatches) where T : class, EventProductCommon
    {
        return mainMatches.Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .OrderBy(x => x.IsSaba)
            .ThenBy(x => x.EventDateTime).ToList();
    }

    public static (string leaguePostfix, string teamPostfix) BuildPostFixSpecialProduct(int? minorBettypeId)
    {
        return minorBettypeId switch
        {
            Bettype.NoOfCorner => ("- CORNERS", "- No.of Corners"),
            Bettype.NextCorner => ("- CORNERS", "- Next Corners"),
            Bettype.TotalBooking => ("- BOOKING", "- Total Bookings"),
            Bettype.NextBooking => ("- BOOKING", "- Next Booking"),
            _ => (string.Empty, string.Empty)
        };
    }

    public static bool CheckMatchHaveBeenExclude(string homeName)
        => homeName.IsContains("(ET)") || homeName.IsContains("(PEN)");
}
