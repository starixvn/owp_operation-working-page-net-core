using OWP.EventPicking.Api.Models;
using OWP.Helpers;

namespace OWP.EventPicking.Api.Helper;

public static class MainMatchHelper
{
    public static List<T> Filter<T>(IEnumerable<T> mainMatches, bool includeTestMatches = false) where T : class, EventProductCommon
    {
        return RemoveTestingMatches(mainMatches, includeTestMatches)
            .Where(x => !ChildMatchHelper.IsChildLeagues(x.LeagueName) &&
                        !SpecialProductHelper.CheckMatchHaveBeenExclude(x.HomeName) &&
                        !x.LeagueName.Contains(ReportConstant.HomeTeamAwayTeam, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static IEnumerable<T> RemoveTestingMatches<T>(IEnumerable<T> mainMatches, bool includeTestMatches = false) where T : EventProductCommon
        => mainMatches.Where(x => !x.IsTestMatch || includeTestMatches);
}