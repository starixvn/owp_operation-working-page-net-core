using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.Helper
{
    public static class EventAssignmentBuildAssignment
    {
        public static void BuildAssignment(List<EventAssignmentModel> matchs,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment)
        {
            matchs.ForEach(match =>
            {
                var assignments = DicHelper.GetValueFromDicBykey(dicMatchAssignment, match.OSMatchId);
                Parallel.ForEach(match.TraderMaxbetModels, traderMaxBetModel =>
                {
                    CreateAssignment(assignments, traderMaxBetModel.TraderAssignment);
                });
            });
        }

        public static void CreateAssignment(
            IList<MatchAssignment> matchAssignments,
            TraderAssignment asssignment)
        {
            var mainAssignment = matchAssignments.FirstOrDefault(match =>
                string.Equals(match.BetTypeName, asssignment.BetTypeName) && !string.IsNullOrEmpty(match.Username));

            var otherAssignment = matchAssignments.FirstOrDefault(match =>
                 string.Equals(match.BetTypeName, asssignment.BetTypeName) && !string.IsNullOrEmpty(match.AssignmentNote));
            if (mainAssignment != null)
            {
                asssignment.MainTrader = mainAssignment.Username;
            }
            if (otherAssignment != null)
            {
                asssignment.AssignmentNote = DetectOtherAssignment(otherAssignment);
            }
        }

        public static string DetectOtherAssignment(MatchAssignment assignment)
        {
            if (assignment.AssignmentNote != null)
            {
                return assignment.AssignmentNote.Contains("#<user>")
                    ? assignment.AssignmentNote.Replace("#<user>", $"#{assignment.Username}")
                    : assignment.AssignmentNote;
            }

            return string.Empty;
        }
    }
}
