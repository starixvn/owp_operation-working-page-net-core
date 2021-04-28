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
            var isMatchStatusChanged = !string.Equals(snapshot.OldMatch.MatchStatus, snapshot.NewMatch.MatchStatus, StringComparison.CurrentCultureIgnoreCase);
            if (!isMatchStatusChanged)
            {
                return new List<ChangedValue>();
            }

           return existingAssignees.Select(a => new ChangedValue
           {
               MessageTypeId = (int)MsgTemplateCode.EditMatchStatus,
               OldValue = snapshot.OldMatch.MatchStatus,
               NewValue = snapshot.NewMatch.MatchStatus,
               MatchNo = snapshot.NewMatch.MatchNo.ToString(),
               League = snapshot.NewMatch.League,
               EventName = snapshot.NewMatch.Home + EVENT_VS + snapshot.NewMatch.Away,
               AssigneeId = a.AssigneeId,
               AssigneeName = a.AssigneeName
           }).ToList();
        }
    }
}