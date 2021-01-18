using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Builder
{
    public interface AssignmentBuilder
    {
        public void BuildAssignment(IList<TraderMaxBetModel> traderMaxBetModels,
           IList<MatchAssignment> matchAssignments);
    }

    public class AssignmentBuilderImp : AssignmentBuilder
    {
        private IList<MatchAssignment> matchAssignments;
        public void BuildAssignment(IList<TraderMaxBetModel> traderMaxBetModels,
           IList<MatchAssignment> matchAssignments)
        {
            this.matchAssignments = matchAssignments;
            foreach (var traderMaxBetModel in traderMaxBetModels)
            {
                CreateAssignment(traderMaxBetModel.TraderAssignment);
            }
        }

        public void CreateAssignment(TraderAssignment asssignment)
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
                return assignment.UserId != 0 
                    ? assignment.AssignmentNote.Replace("<user>", assignment.Username) 
                    : assignment.AssignmentNote.Replace("#<user>", "");
            }

            return string.Empty;
        }
    }
}
