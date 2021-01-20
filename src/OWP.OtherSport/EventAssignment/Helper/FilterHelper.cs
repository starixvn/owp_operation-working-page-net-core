namespace OWP.OtherSport.EventAssignment.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.Extensions;
    using OWP.OtherSport.Submit.Models;

    public static class FilterHelper
    {
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
                var tradersAssignment = DicHelper.GetValueFromDicBykey(dicMatchAssignment, matchAssignment.OSMatchId);
                var existsAssignment = tradersAssignment?.Any(t => t.Username.IsContains(employee));

                if (existsAssignment.HasValue && existsAssignment.Value)
                {
                    matchsResult.Add(matchAssignment);
                }
            }

            return matchsResult;
        }
    }
}
