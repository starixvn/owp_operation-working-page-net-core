using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.QueryAndSave.Models;

namespace OWP.PrematchAssignment.AutoAssignment.Helper;

public static class ExcludeUnSupportedLeagueHelper
{
    public static QueryPreMatchLeagueViewModel Filter(QueryPreMatchLeagueViewModel model)
    {
        var filteredModel = new QueryPreMatchLeagueViewModel
        {
            InfoSiteMatches = model.InfoSiteMatches.Where(match => !match.IsTest).ToList()
        };

        filteredModel.PreMatchLeagues = model.PreMatchLeagues
            .Where(league => (!league.IsSaba || AutoAssignmentConstants.SupportedSabaLeagues.Contains(league.LeagueName))
                && filteredModel.InfoSiteMatches.Any(m => m.LeagueId == league.LeagueId))
            .ToList();

        foreach (var league in filteredModel.PreMatchLeagues)
        {
            league.MatchCount = filteredModel.InfoSiteMatches.Count(m => m.LeagueId == league.LeagueId);
        }

        return filteredModel;
    }
}
