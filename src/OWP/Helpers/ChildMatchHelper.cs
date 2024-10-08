using OWP.Constants;
using System.Collections.Immutable;

namespace OWP.Helpers;

public static class ChildMatchHelper
{
    private static readonly ImmutableList<string> MinorLeagueMatches = GetAllMinorLeagueMatches();

    public static bool IsChildLeagues(string leagueName)
    {
        return MinorLeagueMatches.Exists(x => leagueName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
    }

    private static ImmutableList<string> GetAllMinorLeagueMatches()
    {
        return ExplicitMinorMatchRuleConstants.MinorMatchesRules1
            .Concat(ExplicitMinorMatchRuleConstants.MinorMatchRules2Away)
            .Concat(ExplicitMinorMatchRuleConstants.MinorMatchRules2Home)
            .Select(x => x.LeagueLike)
            .Where(a => !string.IsNullOrEmpty(a))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToImmutableList();
    }
}
