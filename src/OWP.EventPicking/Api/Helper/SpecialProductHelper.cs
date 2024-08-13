using OWP.Constants;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Helper;
internal static class SpecialProductHelper
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
    public static  List<T> CombineMainAndChildMatchEvent<T>(IList<T> mainMatches, IList<T> childMatches)where T : class, EventProductCommon
    {
        var matches = new List<T>();

        foreach (var mainMatch in mainMatches)
        {
            matches.Add(mainMatch);
            var childMatchesOfThisMainMatch = childMatches
                .Where(childMatch => childMatch.InfoSiteMatchId == mainMatch.InfoSiteMatchId)
                .OrderByDescending(x => x.MaxBetHDPNumber);
            matches.AddRange(childMatchesOfThisMainMatch);
        }

        return matches
            .Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .OrderBy(x => x.IsSaba)
            .ThenBy(x => x.EventDateTime)
            .ToList();
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
