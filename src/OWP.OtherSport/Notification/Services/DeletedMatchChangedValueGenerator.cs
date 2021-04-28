namespace OWP.OtherSport.Notification.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Extensions;
    using Models;
    using OWP.Extensions;
    using Submit.Models;

    public interface DeletedMatchChangedValueGenerator
    {
        IEnumerable<ChangedValue> Create(List<int> matcheIds, AssignmentSnapshot snapshot);
    }

    public class DeletedMatchChangedValueGeneratorImp : DeletedMatchChangedValueGenerator
    {
        private const string EVENT_VS = " -vs- ";

        public IEnumerable<ChangedValue> Create(List<int> matcheIds, AssignmentSnapshot snapshot)
        {
            foreach (var matchId in matcheIds)
            {
                var match = snapshot.Matches.GetValue(matchId);
                var assignments = snapshot.MatchAssignments.GetValue(matchId, new List<MatchAssignment>())
                                          .Where(x => x.UserId != 0)
                                          .ToList();
                foreach (var assignment in assignments)
                {
                    yield return GetDeletedAssignmentOfMatchChanged(assignment, match);
                }
            }
        }

        public ChangedValue GetDeletedAssignmentOfMatchChanged(MatchAssignment assignment, MatchInfo match)
        {
            var messageTypeId = assignment.AssignmentType == 1
                ? (int)MsgTemplateCode.AssignmentDelete
                : (int)MsgTemplateCode.OtherAssignmentDelete;
            var oldValue = UpdatedValueHelper.GetValue(assignment.AssignmentType, assignment.Username, assignment.AssignmentNote);
            return new ChangedValue
            {
                MessageTypeId = messageTypeId,
                OldValue = oldValue,
                NewValue = null,
                BetTypeName = assignment.BetTypeName,
                OSMatchId = match.OSMatchId,
                MatchNo = match.MatchNo.ToString(),
                League = match.League,
                EventName = match.Home + EVENT_VS + match.Away,
                AssigneeId = assignment.UserId,
                AssigneeName = assignment.Username,
                AssignmentType = assignment.AssignmentType,
                AssignmentNote = assignment.AssignmentNote
            };
        }
    }
}