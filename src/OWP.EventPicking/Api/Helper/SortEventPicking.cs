using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Helper;

public static class SortEventPicking
{
    public static List<T> SortList<T>(IList<T> matches) where T : class, EventProductCommon
    {
        const string saba15mins = " - 15 Mins Play";
        const string saba16mins = " - 16 Mins Play";
        const string saba20mins = " - 20 Mins Play";

        var orderPriority1stSaba = matches.Where(x =>
                x.IsSaba &&
                (x.LeagueName.IsContains(saba15mins)
                 || x.LeagueName.IsContains(saba16mins)
                 || x.LeagueName.IsContains(saba20mins)))
            .OrderBy(x => x.EventDateTime)
            .ThenByDescending(x => x.LeagueName.Contains(saba15mins))
            .ThenByDescending(x => x.LeagueName.Contains(saba20mins))
            .ThenByDescending(x => x.LeagueName.Contains(saba16mins));
        var orderPriority2ndSaba = matches.Where(x => x.IsSaba && x.LeagueName.IsContainsAll("PES", " - 5 Mins Play"))
            .OrderBy(x => x.EventDateTime);
        var orderPriority3rdSaba = matches.Where(x => x.IsSaba && x.LeagueName.IsContains(" - PENALTY SHOOTOUTS"))
            .OrderBy(x => x.EventDateTime);

        var sabaCombine = orderPriority1stSaba.Concat(orderPriority2ndSaba).Concat(orderPriority3rdSaba).ToList();

        var realMatches = matches.Except(sabaCombine).OrderBy(x => x.EventDateTime).ToList();

        return realMatches.Concat(sabaCombine).ToList();
    }
}