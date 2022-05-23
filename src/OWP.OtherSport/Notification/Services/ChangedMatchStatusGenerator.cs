namespace OWP.OtherSport.Notification.Services
{
    using Constants;
    using Models;
    using OWP.OtherSport.Notification.Extensions;
    using System.Globalization;

    public interface ChangedMatchStatusGenerator
    {
        List<ChangedValue> Create(ChangedMatchSnapshot snapshot, List<Assignee> existingAssignees);
    }

    public class ChangedMatchStatusGeneratorImp : ChangedMatchStatusGenerator
    {
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
                EventName = UpdatedValueHelper.CreateEventName(snapshot.NewMatch),
                UserId = a.AssigneeId,
                Username = a.AssigneeName
            }).ToList();
        }
    }
}