namespace OWP.OtherSport.EventAssignment.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.Extensions;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;

    public static class FilterHelper
    {
        public static List<MatchAssignment> RemoveAssignmentHasNoteInvalid(IList<MatchAssignment> assignments)
        {
            var assignmentResult = new List<MatchAssignment>();
            foreach (var assignment in assignments)
            {
                if (assignment.AssignmentType == (int)AssignmentType.Assignment)
                {
                    assignmentResult.Add(assignment);
                }
                else
                {
                    if (!string.Equals(assignment.AssignmentNote, "#<user>")
                   && !string.IsNullOrWhiteSpace(assignment.Username))
                    {
                        assignmentResult.Add(assignment);
                    }
                }
            }
            return assignmentResult;
        }

    public static IList<MatchInfo> FilterMatchByEmployee(string employee,
        List<MatchInfo> matchsAssignments,
        Dictionary<int, List<MatchAssignment>> dicMatchAssignment)
    {
        if (string.IsNullOrWhiteSpace(employee))
        {
            return matchsAssignments;
        }

        var matchsResult = new List<MatchInfo>();

        foreach (var matchAssignment in matchsAssignments)
        {
            var tradersAssignment = dicMatchAssignment.GetValue(matchAssignment.OSMatchId, new List<MatchAssignment>());
            var existsAssignment = tradersAssignment.Any(match =>
            match.Username.IsContains(employee) ||
            match.Nickname.IsContains(employee));
            if (existsAssignment)
            {
                matchsResult.Add(matchAssignment);
            }
        }

        return matchsResult;
    }
}
}
