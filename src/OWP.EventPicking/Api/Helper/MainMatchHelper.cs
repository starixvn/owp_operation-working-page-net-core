using OWP.EventPicking.Api.Models;
using OWP.Helpers;

namespace OWP.EventPicking.Api.Helper;

public static class MainMatchHelper
{
    public static List<T> Filter<T>(IEnumerable<T> mainMatches) where T : class, EventProductCommon
    {
        return RemoveTestingMatches(mainMatches)
            .Where(x => !ChildMatchHelper.IsChildLeagues(x.LeagueName) &&
                        !SpecialProductHelper.CheckMatchHaveBeenExclude(x.HomeName))
            .ToList();
    }

    public static IEnumerable<T> RemoveTestingMatches<T>(IEnumerable<T> mainMatches) where T : EventProductCommon
        => mainMatches.Where(x => !x.IsTestMatch);
}
