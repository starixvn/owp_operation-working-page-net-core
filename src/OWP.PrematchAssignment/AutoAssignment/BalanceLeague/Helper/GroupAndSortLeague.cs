using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.AutoAssignment.BalanceLeague.Helper;
public class GroupAndSortLeague
{
    public readonly List<string> mjLeagueGroups = new() { "MJ", "MJ.T1", "MJ.T2", "CL.MJ.Q", "INT.MJ", "World Cup", "EURO", "Others 1" };

    public readonly List<string> minorLeagueGroups = new() { "M1", "M2", "M3", "M4", "M5" };

    public void Build(IList<PreMatchLeague> leagues)
    {
        if (leagues == null || !leagues.Any()) return;

        var fantasySabaLeague = leagues.Where(x=>x.LeagueName.Contains("SABA") || x.LeagueName.Contains("FANTASY")).ToList();
        var remainingLeagues = GetLeaguesExcluding(leagues, fantasySabaLeague);
        var mjLeagues = BuildForMjLeagues(remainingLeagues);
    }

    private List<PreMatchLeague> BuildForMjLeagues(IList<PreMatchLeague> leagues)
    {
        var mjLeagues = FilterMjLeagues(leagues);
        if (mjLeagues.Count < 4)
        {
            var remainingLeagues = GetLeaguesExcluding(leagues, mjLeagues);
            AddLeaguesToMjGroup(remainingLeagues, mjLeagues, 5 - mjLeagues.Count);
        }
        return mjLeagues;
    }

    private List<PreMatchLeague> FilterMjLeagues(IList<PreMatchLeague> leagues)
    {
        return leagues.Where(x => mjLeagueGroups.Contains(x.LeagueGroup)).ToList();
    }

    private List<PreMatchLeague> GetLeaguesExcluding(IList<PreMatchLeague> leagues, IList<PreMatchLeague> leagueGroups)
    {
        var leagueGroupIds = leagueGroups.Select(x => x.LeagueId).ToList();
        return leagues.ExceptBy(leagueGroupIds, x => x.LeagueId).ToList();
    }

    private void AddLeaguesToMjGroup(IList<PreMatchLeague> leagues, IList<PreMatchLeague> mjLeagues, int countToAdd)
    {
        var sortedLeagues = leagues.OrderBy(x => x.IsEarlyOffered)
            .ThenBy(x => minorLeagueGroups.IndexOf(x.LeagueGroup))
            .ToList();

        for (var i = 0; i < countToAdd && i < sortedLeagues.Count; i++)
        {
            mjLeagues.Add(sortedLeagues[i]);
        }
    }
}
