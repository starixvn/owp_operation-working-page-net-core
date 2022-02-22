namespace OWP.OtherSport.Notification.Services
{
    using Constants;
    using Models;
    using System.Globalization;

    public interface ChangedMatchStatusGenerator
    {
        List<ChangedValue> Create(ChangedMatchSnapshot snapshot, List<Assignee> existingAssignees);
    }

    public class ChangedMatchStatusGeneratorImp : ChangedMatchStatusGenerator
    {
        private const string EVENT_VS = " -vs- ";

        public List<ChangedValue> Create(ChangedMatchSnapshot snapshot, List<Assignee> existingAssignees)
        {
            var isMatchStatusChanged = !string.Equals(snapshot.OldMatch.OfferLive, snapshot.NewMatch.OfferLive, StringComparison.CurrentCultureIgnoreCase);
            if (!isMatchStatusChanged)
            {
                return new List<ChangedValue>();
            }

            return existingAssignees.Select(a => new ChangedValue
            {
                MessageTypeId = (int)MsgTemplateCode.EditMatchStatus,
                OldValue = snapshot.OldMatch.OfferLive,
                NewValue = snapshot.NewMatch.OfferLive,
                MatchNo = snapshot.NewMatch.MatchNo.ToString(CultureInfo.InvariantCulture),
                League = snapshot.NewMatch.League,
                EventName = snapshot.NewMatch.Home + EVENT_VS + snapshot.NewMatch.Away,
                UserId = a.AssigneeId,
                Username = a.AssigneeName
            }).ToList();
        }
    }
}