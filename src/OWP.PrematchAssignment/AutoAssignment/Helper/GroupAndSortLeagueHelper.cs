namespace OWP.PrematchAssignment.AutoAssignment.Helper;

using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.Models;

public static class GroupAndSortLeagueHelper
{
    public static GroupPreMatchLeagueModel Build(IEnumerable<LeagueAssignment> leagues)
    {
        if (leagues == null || !leagues.Any())
        {
            return new GroupPreMatchLeagueModel();
        }

        var childLeagues = leagues.Where(league => IsChildLeagues(league.LeagueName)).ToList();

        var remainingLeagues = GetLeaguesExcluding(leagues, childLeagues);
        var fantasySabaLeagues = remainingLeagues.Where(league => league.LeagueName.Contains("SABA") || league.LeagueName.Contains("FANTASY"))
            .OrderByDescending(x => x.IsEarlyOffered)
            .ThenByDescending(x => x.MatchCount).ToList();

        remainingLeagues = GetLeaguesExcluding(remainingLeagues, fantasySabaLeagues);
        var majorLeagues = BuildForMajorLeagues(remainingLeagues);

        remainingLeagues = GetLeaguesExcluding(remainingLeagues, majorLeagues);
        var greenDotLeagues = remainingLeagues.Where(x => x.IsEarlyOffered).OrderByDescending(x => x.MatchCount).ToList();
        var noGreenDotLeagues = GetLeaguesExcluding(remainingLeagues, greenDotLeagues).OrderByDescending(x => x.MatchCount).ToList();

        return new GroupPreMatchLeagueModel
        {
            MajorLeagues = majorLeagues,
            GreenDotLeagues = greenDotLeagues,
            NoGreenDotLeagues = noGreenDotLeagues,
            FantasySabaLeagues = fantasySabaLeagues,
            ChildLeagues = childLeagues
        };
    }

    private static List<LeagueAssignment> BuildForMajorLeagues(IEnumerable<LeagueAssignment> leagues)
    {
        var majorLeagues = leagues.Where(x => AutoAssignmentConstants.MajorLeagueGroups.Contains(x.LeagueGroup)).ToList();
        var totalMajorLeagues = majorLeagues.Count;
        const byte MinMajorLeague = 4;
        if (totalMajorLeagues > 0 && totalMajorLeagues < MinMajorLeague)
        {
            var remainingLeagues = GetLeaguesExcluding(leagues, majorLeagues);

            var additional = 5 - totalMajorLeagues;
            AddLeaguesToMajorGroup(remainingLeagues, majorLeagues, additional);
        }

        return majorLeagues;
    }

    private static List<LeagueAssignment> GetLeaguesExcluding(IEnumerable<LeagueAssignment> leagues, IEnumerable<LeagueAssignment> leagueGroups)
    {
        var leagueGroupIds = leagueGroups.Select(x => x.LeagueId).ToList();
        return leagues.ExceptBy(leagueGroupIds, x => x.LeagueId).ToList();
    }

    private static void AddLeaguesToMajorGroup(IEnumerable<LeagueAssignment> leagues, IList<LeagueAssignment> majorLeagues, int countToAdd)
    {
        var sortedLeagues = leagues.OrderByDescending(x => x.IsEarlyOffered)
            .ThenBy(x => x.LeagueGroup switch
            {
                "M1" => 1,
                "M2" => 2,
                "M3" => 3,
                "M4" => 4,
                "M5" => 5,
                _ => 6
            })
            .ThenBy(x => x.PreMatchLeagueId).ToList();

        for (var i = 0; i < countToAdd && i < sortedLeagues.Count; i++)
        {
            majorLeagues.Add(sortedLeagues[i]);
        }
    }

    private static bool IsChildLeagues(string leagueName)
        => AutoAssignmentConstants.ChildLeagueRules.Any(rule => leagueName.EndsWith(rule, StringComparison.OrdinalIgnoreCase));
}
