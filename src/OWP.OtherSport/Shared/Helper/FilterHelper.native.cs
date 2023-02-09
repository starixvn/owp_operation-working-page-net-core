namespace OWP.OtherSport.Shared.Helper;

using Extensions;
using Submit.Models;

public static class FilterHelper
{
    public static Dictionary<int, MatchInfo> FilterMatchesByEmployee(string employee, AssignmentSnapshot assignmentSnapshot)
    {
        if (string.IsNullOrEmpty(employee))
        {
            return assignmentSnapshot.Matches;
        }

        var matches = new Dictionary<int, MatchInfo>();
        foreach (var key in assignmentSnapshot.Matches.Keys)
        {
            var assignments = assignmentSnapshot.MatchAssignments.GetValue(key, new List<MatchAssignment>());
            var existsAssignment = assignments.Any(match =>
                (match.Username != null && match.Username.ToLower().Equals(employee?.ToLower()) ||
                 (match.Nickname != null && match.Nickname.ToLower().Equals(employee?.ToLower()))));
            if (existsAssignment)
            {
                matches.Add(key, assignmentSnapshot.Matches[key]);
            }
        }

        return matches;
    }

    public static Dictionary<int, MatchInfo> FilterMatchesByMatchStatus(IList<string> matchStatuses, Dictionary<int, MatchInfo> matchsInfos)
    {
        var matches = new Dictionary<int, MatchInfo>();
        foreach (var key in matchsInfos.Keys)
        {
            var match = matchsInfos[key];
            if (matchStatuses.Contains(match.MatchStatus, StringComparer.OrdinalIgnoreCase))
            {
                matches.Add(key, matchsInfos[key]);
            }
        }

        return matches;
    }
}