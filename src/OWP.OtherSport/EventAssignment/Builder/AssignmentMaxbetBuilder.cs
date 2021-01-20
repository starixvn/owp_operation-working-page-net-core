using OWP.OtherSport.EventAssignment.Helper;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;
using System.Collections.Generic;

namespace OWP.OtherSport.EventAssignment.Builder
{
    public interface AssignmentMaxbetBuilder
    {
        public void Build(
            List<EventAssignmentModel> eventAssignmentModels,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment);
    }

    public class AssignmentMaxbetBuilderImp : AssignmentMaxbetBuilder
    {
        private readonly MaxbetBuilder maxbetBuilder;

        private readonly AssignmentBuilder assignmentBuilder;

        public AssignmentMaxbetBuilderImp(
            MaxbetBuilder maxbetBuilder,
            AssignmentBuilder assignmentBuilder)
        {
            this.maxbetBuilder = maxbetBuilder;
            this.assignmentBuilder = assignmentBuilder;
        }

        public void Build(
            List<EventAssignmentModel> eventAssignmentModels,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment)
        {
            foreach (var assignmentModel in eventAssignmentModels)
            {
                var maxbets = DicHelper.GetValueFromDicBykey(dicMatchMaxBet, assignmentModel.OSMatchId);
                var assignments = DicHelper.GetValueFromDicBykey(dicMatchAssignment, assignmentModel.OSMatchId);
                maxbetBuilder.BuildMaxbet(assignmentModel.TraderMaxbetModels, maxbets);
                assignmentBuilder.BuildAssignment(assignmentModel.TraderMaxbetModels, assignments);
            }
        }
    }
}