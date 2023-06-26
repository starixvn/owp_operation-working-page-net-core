using OWP.Extensions;
using OWP.OtherSport.Submit.Models;
// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator

namespace OWP.OtherSport.Shared.Helper;

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
                (match.Username != null && match.Username.ToLower().Equals(employee.ToLower())) ||
                (match.Nickname != null && match.Nickname.ToLower().Equals(employee.ToLower())));
            if (existsAssignment)
            {
                matches.Add(key, assignmentSnapshot.Matches[key]);
            }
        }

        return matches;
    }

    public static Dictionary<int, MatchInfo> FilterMatchesByMatchStatus(IList<string> matchStatuses, Dictionary<int, MatchInfo> matchInfos)
    {
        var matches = new Dictionary<int, MatchInfo>();
        foreach (var key in matchInfos.Keys)
        {
            var match = matchInfos[key];
            if (matchStatuses.Contains(match.MatchStatus, StringComparer.OrdinalIgnoreCase))
            {
                matches.Add(key, matchInfos[key]);
            }
        }

        return matches;
    }
}