using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Contants;
using OWP.OtherSport.Submit.Models;
using System.Collections.Generic;
using System.Linq;

namespace OWP.OtherSport.EventAssignment.Builder
{
    public interface AssignmentBuilder
    {
        public void BuildAssignment(IList<TraderMaxBetModel> traderMaxBetModels,
           IList<MatchAssignment> matchAssignments);

        public void CreateAssignment(TraderAssignment asssignment, IList<MatchAssignment> matchAssignments);

        string DetectAssignment(MatchAssignment assignment);
    }

    public class AssignmentBuilderImp : AssignmentBuilder
    {
        public void BuildAssignment(
            IList<TraderMaxBetModel> traderMaxBetModels,
            IList<MatchAssignment> matchAssignments)
        {
            foreach (var traderMaxBetModel in traderMaxBetModels)
            {
                CreateAssignment(traderMaxBetModel.TraderAssignment, matchAssignments);
            }
        }

        public void CreateAssignment(TraderAssignment asssignment, IList<MatchAssignment> matchAssignments)
        {
            var mainAssignment = matchAssignments.FirstOrDefault(match =>
                string.Equals(match.BetTypeName, asssignment.BetTypeName)
                && !string.IsNullOrEmpty(match.AssignmentNote)
                && match.AssignmentType == (int)AssignmentType.Assignment);

            var otherAssignment = matchAssignments.FirstOrDefault(match =>
                 string.Equals(match.BetTypeName, asssignment.BetTypeName)
                 && !string.IsNullOrEmpty(match.AssignmentNote)
                 && match.AssignmentType == (int)AssignmentType.OtherAssignment);

            if (mainAssignment != null)
            {
                asssignment.MainTrader = DetectAssignment(mainAssignment);
            }

            if (otherAssignment != null)
            {
                asssignment.AssignmentNote = DetectAssignment(otherAssignment);
            }
        }

        public string DetectAssignment(MatchAssignment assignment)
        {
            if (!string.IsNullOrEmpty(assignment.AssignmentNote))
            {
                return assignment.UserId != 0
                    ? assignment.AssignmentNote.Replace("<user>", assignment.Username)
                    : assignment.AssignmentNote.Replace("#<user>", "");
            }

            return string.Empty;
        }
    }
}