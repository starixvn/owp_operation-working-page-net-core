namespace OWP.OtherSport.Notification.Services
{
    
    using System.Globalization;
        using System.Reflection;
    using Models;
    using OWP.OtherSport.Notification.Constants;
    using OWP.OtherSport.Submit.Models;

    public interface ChangedMatchInfoGenerator
    {
        List<ChangedValue> Create(MatchInfo oldMatch, MatchInfo newMatch, List<Assignee> existingAssignees);
    }

    public class ChangedMatchInfoGeneratorImp : ChangedMatchInfoGenerator
    {
        private const string EVENT_VS = " -vs- ";

        private static readonly List<string> MatchInfoPropertyNames = new List<string>
        {
            "MatchNo",
            "Category",
            "MatchOffer",
            "NoHDP",
            "AutoLink",
            "OtherProvider",
            "IPTV",
            "WB",
            "Remark",
            "Reporter",
            "Provider"
        };

        public List<ChangedValue> Create(MatchInfo oldMatch, MatchInfo newMatch, List<Assignee> existingAssignees)
        {
            var changedValueList = new List<ChangedValue>();
            var changedList = GetListValueChanged(oldMatch, newMatch);
            foreach (var changed in changedList)
            {
                var changedValues = existingAssignees.Select(a => new ChangedValue
                {
                    MessageTypeId = changed.MessageTypeId,
                    NewValue = GetNewValue(newMatch, changed),
                    OldValue = GetOldValue(oldMatch, changed),
                    League = newMatch.League,
                    MatchNo = newMatch.MatchNo.ToString(CultureInfo.InvariantCulture),
                    EventName = newMatch.Home + EVENT_VS + newMatch.Away,
                    UserId = a.AssigneeId,
                    Username = a.AssigneeName
                });

                changedValueList.AddRange(changedValues);
            }

            return changedValueList;
        }

        public static string GetNewValue(MatchInfo newMatch, ChangedValue changed)
            => changed.MessageTypeId == MessageType.GetMessageTypeIdForChangedMatchInfo("Home")
                ? changed.NewValue + EVENT_VS + newMatch.Away
                : changed.NewValue;

        public static string GetOldValue(MatchInfo newMatch, ChangedValue changed)
        => changed.MessageTypeId == MessageType.GetMessageTypeIdForChangedMatchInfo("Home")
                ? changed.OldValue + EVENT_VS + newMatch.Away
                : changed.OldValue;

        public List<ChangedValue> GetListValueChanged(MatchInfo oldMatch, MatchInfo newMatch)
        {
            var changedValues = new List<ChangedValue>();
            foreach (var propertyName in MatchInfoPropertyNames)
            {
                var property = GetProperty(oldMatch, propertyName);
                var oldValue = property.GetValue(oldMatch)?.ToString() ?? string.Empty;
                var newValue = property.GetValue(newMatch)?.ToString() ?? string.Empty;
                if (oldValue.Equals(newValue))
                {
                    continue;
                }

                var messageModel = new ChangedValue
                {
                    OldValue = oldValue,
                    NewValue = newValue,
                    MessageTypeId = MessageType.GetMessageTypeIdForChangedMatchInfo(propertyName)
                };

                changedValues.Add(messageModel);
            }

            return changedValues.ToList();
        }

        public static PropertyInfo GetProperty(MatchInfo match, string propertyName)
        {
            return match.GetType().GetProperties().Single(p => p.Name == propertyName);
        }
    }
}