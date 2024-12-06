using OWP.Constants;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.AutoAssignment.Helper;

public static class ExcludeUnSupportedLeagueHelper
{
    private static readonly IList<string> childMatchRules = [
        "- SPECIFIC 15 MINS", "- SPECIFIC 10 MINS", "- CORNERS", "- BOOKING", "- WINNER", "- PENALTY",
        "- GOAL KICK", "- OFFSIDE", "- THROW IN", "- FREE KICK", "- SPECIALS GOAL",
        "- SPECIFIC 15 MINS NUMBER OF CORNERS", "- SPECIFIC 15 MINS TOTAL BOOKINGS",
        "- BRONZE MEDAL WINNER", "- GOLD MEDAL WINNER", "- WHICH TEAM WILL ADVANCE TO NEXT ROUND",
        "- SINGLE TEAM OVER/UNDER", "- PENALTY SHOOT-OUT"
    ];

    public static QueryPreMatchLeagueViewModel Filter(QueryPreMatchLeagueViewModel model)
    {
        var filteredModel = new QueryPreMatchLeagueViewModel();
        var supportedSabas = new List<string>
        {
            "SABA CLUB FRIENDLY Virtual FC 24 - 16 Mins Play",
            "SABA INTERNATIONAL FRIENDLY Virtual PES 21 - 20 Mins Play",
            "SABA CLUB FRIENDLY Virtual PES 21 - 15 Mins Play"
        };

        filteredModel.InfoSiteMatches = model.InfoSiteMatches.Where(match => !match.IsTest).ToList();

        filteredModel.PreMatchLeagues = model.PreMatchLeagues
            .Where(league => !IsChildLeagues(league.LeagueName)
                && (!league.IsSaba || supportedSabas.Contains(league.LeagueName))
                && filteredModel.InfoSiteMatches.Any(m => m.LeagueId == league.LeagueId))
            .ToList();

        foreach (var league in filteredModel.PreMatchLeagues)
        {
            league.MatchCount = filteredModel.InfoSiteMatches.Count(m => m.LeagueId == league.LeagueId);
        }

        return filteredModel;
    }

    private static bool IsChildLeagues(string leagueName)
        => childMatchRules.Any(rule => leagueName.EndsWith(rule, StringComparison.OrdinalIgnoreCase));
}
