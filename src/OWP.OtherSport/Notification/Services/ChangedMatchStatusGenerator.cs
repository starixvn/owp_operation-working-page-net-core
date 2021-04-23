namespace OWP.OtherSport.Notification.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Models;

    public interface ChangedMatchStatusGenerator
    {
        List<ChangedValue> Create(ChangedMatchSnapshot snapshot, List<Assignee> existingAssignees);
    }

    public class ChangedMatchStatusGeneratorImp : ChangedMatchStatusGenerator
    {
        private const string EVENT_VS = " -vs- ";

        public List<ChangedValue> Create(ChangedMatchSnapshot snapshot, List<Assignee> existingAssignees)
        {
            var isCancelSuspendedPostponedStatus = IsCancelSuspendedPostponedStatus(snapshot.NewMatch.MatchStatus);
            var isMatchStatusChanged = !string.Equals(snapshot.OldMatch.MatchStatus, snapshot.NewMatch.MatchStatus, StringComparison.CurrentCultureIgnoreCase);
            if (!isMatchStatusChanged || !isCancelSuspendedPostponedStatus)
            {
                return new List<ChangedValue>();
            }

            var changedValueList = new List<ChangedValue>();
            var isMinorMatch = snapshot.NewMatch.ParentMatchId != 0;
            if (!isMinorMatch)
            {
                var changedValues = existingAssignees.Select(a => new ChangedValue
                {
                    MessageTypeId = (int)MsgTemplateCode.MatchStatus,
                    OldValue = snapshot.OldMatch.MatchStatus,
                    NewValue = snapshot.NewMatch.MatchStatus,
                    MatchNo = snapshot.NewMatch.MatchNo,
                    League = snapshot.NewMatch.LeagueName,
                    EventName = snapshot.NewMatch.Home + EVENT_VS + snapshot.NewMatch.Away,
                    AssigneeId = a.AssigneeId,
                    AssigneeName = a.AssigneeName
                });

                changedValueList.AddRange(changedValues);
            }
            return changedValueList;
        }

        public static bool IsCancelSuspendedPostponedStatus(string matchStatus)
        {
            return matchStatus.IndexOf("CANCEL", StringComparison.CurrentCultureIgnoreCase) >= 0
              || matchStatus.IndexOf("SUSPENDED", StringComparison.CurrentCultureIgnoreCase) >= 0
              || matchStatus.IndexOf("POSTPONED", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}