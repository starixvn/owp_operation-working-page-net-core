namespace OWP.OtherSport.EventAssignment.Services;

using Extensions;
using Models;
using Submit.Contants;
using Submit.Models;

public interface AssignmentBuilder
{
    public void Build(List<EventAssignmentModel> eventAssignmentModels, Dictionary<int, List<MatchAssignment>> matchAssignments);
}

public class AssignmentBuilderImp : AssignmentBuilder
{
    public void Build(List<EventAssignmentModel> eventAssignmentModels, Dictionary<int, List<MatchAssignment>> matchAssignments)
    {
        foreach (var eventAssignmentModel in eventAssignmentModels)
        {
            var assignmentsByOWPMatchId = matchAssignments.GetValue(eventAssignmentModel.OSMatchId, new List<MatchAssignment>());

            CreateAssignments(assignmentsByOWPMatchId, eventAssignmentModel);
        }
    }

    public void CreateAssignments(List<MatchAssignment> assignments, EventAssignmentModel eventAssignmentModel)
    {
        foreach (var assignmentByBettype in assignments.GroupBy(x => x.BetTypeName))
        {
            var mainAssignment = assignmentByBettype.FirstOrDefault(x => x.AssignmentType == (int)AssignmentType.Assignment) ?? new MatchAssignment();
            var otherAssignment = assignmentByBettype.FirstOrDefault(x => x.AssignmentType == (int)AssignmentType.OtherAssignment) ?? new MatchAssignment();

            var trader = new TraderMaxBetModel
            {
                BetTypeName = assignmentByBettype.Key,
                TraderAssignment = new TraderAssignment
                {
                    MainTrader = DetectAssignment(mainAssignment),
                    AssignmentNote = DetectAssignment(otherAssignment)
                }
            };

            eventAssignmentModel.TraderMaxbetModels.Add(trader);
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