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
        var majorLeagues = GetMajorLeagues(remainingLeagues);

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

    private static List<LeagueAssignment> GetMajorLeagues(IEnumerable<LeagueAssignment> leagues)
        => leagues.Where(x => AutoAssignmentConstants.MajorLeagueGroups.Contains(x.LeagueGroup)).ToList();

    private static List<LeagueAssignment> GetLeaguesExcluding(IEnumerable<LeagueAssignment> leagues, IEnumerable<LeagueAssignment> leagueGroups)
    {
        var leagueGroupIds = leagueGroups.Select(x => x.LeagueId).ToList();
        return leagues.ExceptBy(leagueGroupIds, x => x.LeagueId).ToList();
    }

    private static bool IsChildLeagues(string leagueName)
        => AutoAssignmentConstants.ChildLeagueRules.Exists(rule => leagueName.EndsWith(rule, StringComparison.OrdinalIgnoreCase));
}
