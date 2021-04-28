namespace OWP.OtherSport.Notification.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Models;
    using OWP.Extensions;
    using OWP.OtherSport.Notification.Constants;
    using Submit.Models;

    public interface ChangedAssignmentGenerator
    {
        List<ChangedValue> Create(ChangedMatchSnapshot snapshot, List<Assignee> existingAssignees);
    }

    public class ChangedAssignmentGeneratorImp : ChangedAssignmentGenerator
    {
        private const string EVENT_VS = " -vs- ";

        private readonly ChangedDetector changeDetector;

        public ChangedAssignmentGeneratorImp(ChangedDetector changeDetector)
        {
            this.changeDetector = changeDetector;
        }

        public List<ChangedValue> Create(ChangedMatchSnapshot snapshot, List<Assignee> existingAssignees)
        {
            var deletedAssignmentChangedValues = GetDeletedAssignmentChangedValues(snapshot).ToList();
            var insertedAssignmentChangedValues = GetInsertedAssignmentChangedValues(snapshot).ToList();

            var allChangedValues = deletedAssignmentChangedValues.Union(insertedAssignmentChangedValues).ToList();
            var indirectChangedValues = GetIndirectChangedValues(snapshot, allChangedValues, existingAssignees).ToList();

            return allChangedValues.Where(x => x.AssigneeId != 0)
                .Union(indirectChangedValues)
                .Distinct(new AssignmentChangedValueComparer())
                .ToList();
        }

        public IEnumerable<ChangedValue> GetDeletedAssignmentChangedValues(ChangedMatchSnapshot snapshot)
        {
            var oldAssignments = snapshot.OldSnapshot.MatchAssignments.GetValue(snapshot.OldMatch.MatchId, new List<MatchAssignment>());
            var newAssignments = snapshot.NewSnapshot.MatchAssignments.GetValue(snapshot.NewMatch.MatchId, new List<MatchAssignment>());
            var oldMatches = snapshot.OldSnapshot.Matches;

            var deletedAssignments = changeDetector.GetDeletedAssignments(oldAssignments, newAssignments);

            foreach (var assignment in deletedAssignments)
            {
                var match = oldMatches.GetValue(assignment.OSMatchId);

                var messageTypeId = assignment.AssignmentType == 1
                    ? (int)MsgTemplateCode.AssignmentDelete
                    : (int)MsgTemplateCode.OtherAssignmentDelete;
                var oldValue = UpdatedValueHelper.GetValue(assignment.AssignmentType, assignment.Username, assignment.AssignmentNote);
                yield return new ChangedValue
                {
                    MessageTypeId = messageTypeId,
                    OldValue = oldValue,
                    AssignmentNote = assignment.AssignmentNote,
                    AssignmentType = assignment.AssignmentType,
                    NewValue = null,
                    BetTypeName = assignment.BetTypeName,
                    MatchId = match.OSMatchId,
                    MatchNo = match.MatchNo.ToString(),
                    League = match.League,
                    EventName = match.Home + EVENT_VS + match.Away,
                    AssigneeId = assignment.UserId,
                    AssigneeName = assignment.Username
                };
            }
        }

        public IEnumerable<ChangedValue> GetInsertedAssignmentChangedValues(ChangedMatchSnapshot snapshot)
        {
            var oldAssignments = GetAssignmentsOfMatchWhichIsChangedToLive(snapshot);
            var newAssignments = snapshot.NewSnapshot.MatchAssignments.GetValue(snapshot.NewMatch.MatchId, new List<MatchAssignment>());
            var oldMatches = snapshot.OldSnapshot.Matches;

            var insertedAssignments = changeDetector.GetInsertedAssignments(oldAssignments, newAssignments);

            foreach (var assignment in insertedAssignments)
            {
                var match = oldMatches.GetValue(assignment.OSMatchId);

                var messageTypeId = assignment.AssignmentType == 1
                    ? (int)MsgTemplateCode.AssignmentInsert
                    : (int)MsgTemplateCode.OtherAssignmentInsert;
                var newValue = UpdatedValueHelper.GetValue(assignment.AssignmentType, assignment.Username, assignment.AssignmentNote);
                yield return new ChangedValue
                {
                    MessageTypeId = messageTypeId,
                    OldValue = null,
                    NewValue = newValue,
                    AssignmentNote = assignment.AssignmentNote,
                    AssignmentType = assignment.AssignmentType,
                    BetTypeName = assignment.BetTypeName,
                    MatchId = match.OSMatchId,
                    MatchNo = match.MatchNo.ToString(),
                    League = match.League,
                    EventName = match.Home + EVENT_VS + match.Away,
                    AssigneeId = assignment.UserId,
                    AssigneeName = assignment.Username,
                    IsChangedToLiveMatchStatus = IsChangedToLiveMatchStatus(snapshot)
                };
            }
        }

        public static List<MatchAssignment> GetAssignmentsOfMatchWhichIsChangedToLive(ChangedMatchSnapshot snapshot)
        {
            return IsChangedToLiveMatchStatus(snapshot)
                ? new List<MatchAssignment>()
                : snapshot.OldSnapshot.MatchAssignments.GetValue(snapshot.OldMatch.MatchId, new List<MatchAssignment>());
        }

        private static bool IsChangedToLiveMatchStatus(ChangedMatchSnapshot snapshot)
        {
            return IsCancelSuspendedPostponedStatus(snapshot.OldMatch.MatchStatus) && IsLiveStatus(snapshot.NewMatch.MatchStatus);
        }

        private static bool IsCancelSuspendedPostponedStatus(string matchStatus)
        {
            return matchStatus.IndexOf("CANCEL", StringComparison.CurrentCultureIgnoreCase) >= 0
              || matchStatus.IndexOf("SUSPENDED", StringComparison.CurrentCultureIgnoreCase) >= 0
              || matchStatus.IndexOf("POSTPONED", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private static bool IsLiveStatus(string matchStatus)
        {
            return matchStatus.IndexOf("LIVE", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public static IEnumerable<ChangedValue> GetIndirectChangedValues(
            ChangedMatchSnapshot snapshot,
            List<ChangedValue> allChangedValues,
            List<Assignee> existingAssignees)
        {
            return existingAssignees
                .Select(t => allChangedValues.Where(x => !x.IsChangedToLiveMatchStatus)
                                             .Select(a => GetIndirectChangedValue(a, t)))
                .SelectMany(a => a);
        }

        public static ChangedValue GetIndirectChangedValue(ChangedValue changedValue, Assignee assignee)
        {
            return new ChangedValue
            {
                MessageTypeId = changedValue.MessageTypeId,
                OldValue = changedValue.OldValue,
                NewValue = changedValue.NewValue,
                BetTypeName = changedValue.BetTypeName,
                MatchId = changedValue.MatchId,
                MatchNo = changedValue.MatchNo,
                League = changedValue.League,
                EventName = changedValue.EventName,
                AssigneeId = assignee.AssigneeId,
                AssigneeName = assignee.AssigneeName,
                AssignmentType = changedValue.AssignmentType,
                AssignmentNote = changedValue.AssignmentNote,
                IsIndirectChangedAssignment = true
            };
        }
    }
}