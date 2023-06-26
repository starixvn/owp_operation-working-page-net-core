using OWP.AssignmentUpload.Validation.Constants;
using OWP.AssignmentUpload.Validation.Helper;
using OWP.AssignmentUpload.Validation.Models;
using System.Collections.Immutable;

namespace OWP.AssignmentUpload.Validation.Services;

public interface ExplicitMinorMatchRule2
{
    bool DetectMinorBettypeIdRule(AssignmentModel assignment, List<AssignmentModel> assignments);
}

public class ExplicitMinorMatchRule2Imp : ExplicitMinorMatchRule2
{
    private static readonly ImmutableList<MinorMatch> minorMatchesHome = ExplicitMinorMatchRuleConstants.MinorMatchRules2Home;
    private static readonly ImmutableList<MinorMatch> minorMatchesAway = ExplicitMinorMatchRuleConstants.MinorMatchRules2Away;

    public bool DetectMinorBettypeIdRule(AssignmentModel assignment, List<AssignmentModel> assignments)
    {
        return DetectMinorBettypeIdRuleHome(assignment, assignments) ||
               DetectMinorBettypeIdRuleAway(assignment, assignments);
    }

    public static bool DetectMinorBettypeIdRuleHome(AssignmentModel assignment, List<AssignmentModel> assignments)
    {
        foreach (var minorMatch in minorMatchesHome)
        {
            if (!IsMinorMatch(assignment, minorMatch))
            {
                continue;
            }

            var mainMatch = FindMainMatchByHomeOrAway(assignment, assignments, minorMatch, TargetFinder.Home);
            if (mainMatch == null)
            {
                return false;
            }

            assignment.ParentHashKey = mainMatch.HashKey;
            assignment.MinorBettypeId = minorMatch.MinorBettypeId;
            assignment.IsNeutral = false;

            return true;
        }
        return false;
    }

    private static bool IsMinorMatch(AssignmentModel assignment, MinorMatch minorMatch)
        => ValidationHelper.CheckLeagueRule(minorMatch, assignment.League) &&
           ValidationHelper.CheckHomeRule(minorMatch, assignment.Home) &&
           ValidationHelper.CheckAwayRule(minorMatch, assignment.Away);

    public static AssignmentModel FindMainMatchByHomeOrAway(AssignmentModel assignment,
        List<AssignmentModel> assignments,
        MinorMatch minorMatch,
        TargetFinder finder)
    {
        var leagueLength = assignment.League.Length - minorMatch.LeagueChars;
        var homeLength = assignment.Home.Length - minorMatch.HomeChars;
        var awayLength = assignment.Away.Length - minorMatch.AwayChars;
        if (leagueLength <= 0 || homeLength <= 0 || awayLength <= 0)
        {
            return null;
        }

        var league = assignment.League[..leagueLength].TrimEnd();
        var home = assignment.Home[..homeLength].TrimEnd();
        var away = assignment.Away[..awayLength].TrimEnd();
        return FindMainMatchByHomeOrAway(assignments, league, home, away, finder);

    }

    public static AssignmentModel FindMainMatchByHomeOrAway(
        List<AssignmentModel> assignments,
        string league,
        string home,
        string away,
        TargetFinder finder)
    {
        return finder switch
        {
            TargetFinder.Home => assignments?.FirstOrDefault(p =>
                string.Compare(p.League, league, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                string.Compare(p.Home, home, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                string.Compare(p.Home, away, StringComparison.CurrentCultureIgnoreCase) == 0),
            TargetFinder.Away => assignments?.FirstOrDefault(p =>
                string.Compare(p.League, league, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                string.Compare(p.Away, home, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                string.Compare(p.Away, away, StringComparison.CurrentCultureIgnoreCase) == 0),
            _ => null
        };
    }

    public static bool DetectMinorBettypeIdRuleAway(AssignmentModel assignment, List<AssignmentModel> assignments)
    {
        foreach (var minorMatch in minorMatchesAway)
        {
            if (!IsMinorMatch(assignment, minorMatch))
            {
                continue;
            }

            var mainMatch = FindMainMatchByHomeOrAway(assignment, assignments, minorMatch, TargetFinder.Away);
            if (mainMatch == null)
            {
                return false;
            }

            assignment.ParentHashKey = mainMatch.HashKey;
            assignment.MinorBettypeId = minorMatch.MinorBettypeId;

            return true;
        }

        return false;
    }
}