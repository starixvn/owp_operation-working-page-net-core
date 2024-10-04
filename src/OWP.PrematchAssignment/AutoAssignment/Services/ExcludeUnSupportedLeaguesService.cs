using OWP.Constants;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.AutoAssignment.Services;

public interface ExcludeUnSupportedLeaguesService
{
    QueryPreMatchLeagueViewModel Handle(QueryPreMatchLeagueViewModel model);
}

public class ExcludeUnSupportedLeaguesServiceImp : ExcludeUnSupportedLeaguesService
{
    public QueryPreMatchLeagueViewModel Handle(QueryPreMatchLeagueViewModel model)
    {
        return FilterPreMatchLeagues(model);
    }

    public QueryPreMatchLeagueViewModel FilterPreMatchLeagues(QueryPreMatchLeagueViewModel model)
    {
        var filteredModel = new QueryPreMatchLeagueViewModel();
        var supportedSabas = new List<string>
        {
            "SABA CLUB FRIENDLY Virtual FC 24 - 16 Mins Play",
            "SABA INTERNATIONAL FRIENDLY Virtual PES 21 - 20 Mins Play",
            "SABA CLUB FRIENDLY Virtual PES 21 - 15 Mins Play"
        };

        filteredModel.PreMatchLeagues = model.PreMatchLeagues
            .Where(league => !IsChildLeagues(league.LeagueName) && (!league.IsSaba || supportedSabas.Contains(league.LeagueName)))
            .ToList();

        filteredModel.InfoSiteMatches = model.InfoSiteMatches
            .Where(match => !match.IsTest)
            .ToList();

        return filteredModel;
    }

    public static bool IsChildLeagues(string leagueName)
    {
        var allMinorMatches = ExplicitMinorMatchRuleConstants.MinorMatchesRules1
            .Concat(ExplicitMinorMatchRuleConstants.MinorMatchRules2Away)
            .Concat(ExplicitMinorMatchRuleConstants.MinorMatchRules2Home);

        return allMinorMatches
            .Any(m => leagueName.EndsWith(m.LeagueLike, StringComparison.OrdinalIgnoreCase));
    }
}
