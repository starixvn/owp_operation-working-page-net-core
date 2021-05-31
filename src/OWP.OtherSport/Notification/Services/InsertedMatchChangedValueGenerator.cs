namespace OWP.OtherSport.Notification.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Extensions;
    using Models;
    using OWP.Extensions;
    using Submit.Models;

    public interface InsertedMatchChangedValueGenerator
    {
        IEnumerable<ChangedValue> Create(List<int> matcheIds, AssignmentSnapshot snapshot);
    }

    public class InsertedMatchChangedValueGeneratorImp : InsertedMatchChangedValueGenerator
    {
        private const string EVENT_VS = " -vs- ";

        public IEnumerable<ChangedValue> Create(List<int> matcheIds, AssignmentSnapshot snapshot)
        {
            return CreateEnumerable(matcheIds, snapshot).SelectMany(v => v).Distinct().ToList();
        }

        public static IEnumerable<List<ChangedValue>> CreateEnumerable(List<int> matcheIds, AssignmentSnapshot snapshot)
        {
            foreach (var matchId in matcheIds)
            {
                var assignments = snapshot.MatchAssignments.GetValue(matchId, new List<MatchAssignment>())
                                          .Where(x => x.UserId != 0)
                                          .ToList();
                var assignmentChangedValues = GetInsertedAssignmentChangedValues(snapshot.Matches, assignments).ToList();
                yield return assignmentChangedValues.Distinct(new AssignmentChangedValueComparer()).ToList();
            }
        }

        public static IEnumerable<ChangedValue> GetInsertedAssignmentChangedValues(Dictionary<int, MatchInfo> allMatches, List<MatchAssignment> assignments)
        {
            foreach (var assignment in assignments)
            {
                var match = allMatches[assignment.OSMatchId];

                var messageTypeId = assignment.AssignmentType == 1
                    ? (int)MsgTemplateCode.AssignmentInsert
                    : (int)MsgTemplateCode.OtherAssignmentInsert;
                var newValue = UpdatedValueHelper.GetValue(assignment.Username, assignment.AssignmentNote);

                yield return new ChangedValue
                {
                    MessageTypeId = messageTypeId,
                    OldValue = null,
                    NewValue = newValue,
                    BetTypeName = assignment.BetTypeName,
                    OSMatchId = match.OSMatchId,
                    MatchNo = match.MatchNo.ToString(),
                    League = match.League,
                    EventName = match.Home + EVENT_VS + match.Away,
                    UserId = assignment.UserId,
                    Username = assignment.Username,
                    AssignmentType = assignment.AssignmentType,
                    AssignmentNote = assignment.AssignmentNote
                };
            }
        }
    }
}