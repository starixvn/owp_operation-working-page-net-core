using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.Helper
{
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
            Parallel.ForEach(matchsAssignments, matchAssignment =>
            {
                var tradersAssignment = DicHelper.GetValueFromDicBykey(dicMatchAssignment, matchAssignment.OSMatchId);
                var existsAssignment = tradersAssignment?.Any(t =>
                 string.Equals(matchAssignment.OSMatchId, t.OSMatchId)
                 && t.Username.Contains(employee));
                if (existsAssignment.HasValue && existsAssignment.Value)
                {
                    matchsResult.Add(matchAssignment);
                }
            });
            return matchsResult;
        }
    }
}
