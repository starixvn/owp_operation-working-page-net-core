namespace OWP.OtherSport.Shared.Helper
{
    using Extensions;
    using Submit.Models;
    using System.Collections.Generic;
    using System.Linq;

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
                    match.Username.IsContains(employee) ||
                    match.Nickname.IsContains(employee));
                if (existsAssignment)
                {
                    matches.Add(key, assignmentSnapshot.Matches[key]);
                }
            }

            return matches;
        }
    }
}
