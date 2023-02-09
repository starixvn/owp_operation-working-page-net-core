namespace OWP.AssignmentUpload.Validation.Services;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Constants;
using Helper;
using Models;

public interface ExplicitMinorMatchRule1
{
    bool DetectMinorBettypeIdRule(AssignmentModel assignment, List<AssignmentModel> assignments);
}

public class ExplicitMinorMatchRule1Imp : ExplicitMinorMatchRule1
{
    public static readonly ImmutableList<MinorMatch> minorMatches = ExplicitMinorMatchRuleConstants.MinorMatcheRules1;

    public bool DetectMinorBettypeIdRule(AssignmentModel assignment, List<AssignmentModel> assignments)
    {
        foreach (var minorMatch in minorMatches)
        {
            if (IsMinorMatch(assignment, minorMatch))
            {
                var mainMatch = FindMainMatch(assignment, assignments, minorMatch);
                if (mainMatch == null)
                {
                    return false;
                }

                assignment.ParentHashKey = mainMatch.HashKey;
                assignment.MinorBettypeId = minorMatch.MinorBettypeId;
                assignment.IsNeutral = false;

                return true;
            }
        }

        return false;
    }

    private static bool IsMinorMatch(AssignmentModel assignment, MinorMatch minorMatch)
        => ValidationHelper.CheckLeagueRule(minorMatch, assignment.League) &&
           ValidationHelper.CheckHomeRule(minorMatch, assignment.Home) &&
           ValidationHelper.CheckAwayRule(minorMatch, assignment.Away);

    public static AssignmentModel FindMainMatch(AssignmentModel assignment, List<AssignmentModel> assignments, MinorMatch minorMatch)
    {
        var leagueLength = assignment.League.Length - minorMatch.LeagueChars;
        var homeLength = assignment.Home.Length - minorMatch.HomeChars;
        var awayLength = assignment.Away.Length - minorMatch.AwayChars;
        if (leagueLength > 0 && homeLength > 0 && awayLength > 0)
        {
            var league = assignment.League.Substring(0, leagueLength).TrimEnd();
            var home = assignment.Home.Substring(0, homeLength).TrimEnd();
            var away = assignment.Away.Substring(0, awayLength).TrimEnd();

            return assignments.FirstOrDefault(p =>
                string.Compare(p.League, league, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                string.Compare(p.Home, home, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                string.Compare(p.Away, away, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        return null;
    }
}