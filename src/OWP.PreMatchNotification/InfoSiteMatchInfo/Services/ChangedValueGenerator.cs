namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.Models;


    public interface ChangedValueGenerator
    {
        IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees);

        IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> preMatchAssignees);
    }

    public class ChangedValueGeneratorImp : ChangedValueGenerator
    {

        private static readonly List<string> InfoSiteMatchPropertyNames = new List<string>
        {
            "EventDateTime",
            "HomeName",
            "AwayName",
        };
        private const string EVENT_VS = " -vs- ";
        public IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees)
        {
            var changedValueList = new List<ChangedValue>();
            foreach (var item in newSnapshot)
            {
                var oldInfositeMatch = oldSnapshot.FirstOrDefault(x => x.InfoSiteMatchId == item.InfoSiteMatchId);
                if (oldInfositeMatch != null)
                {
                    var assigneesIDs = preMatchAssignees.Where(i => i.OWPLeagueId == item.OWPLeagueId).Select(x => x.AssigneeId).ToList();
                    changedValueList.AddRange(GetListValueChangedByProPerty(oldInfositeMatch, item, assigneesIDs));
                }
            }
            return changedValueList;    
        }

        public IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> preMatchAssignees)
        {
            return new List<ChangedValue>();
        }


        List<ChangedValue> GetListValueChangedByProPerty(InfoSiteMatch oldMatch, InfoSiteMatch newMatch, List<int> receivedUserIds)
        {
            var changedValues = new List<ChangedValue>();
            foreach (var propertyName in InfoSiteMatchPropertyNames)
            {

                var property = GetProperty(oldMatch, propertyName);
                var oldValue = property.GetValue(oldMatch)?.ToString() ?? string.Empty;
                var newValue = property.GetValue(newMatch)?.ToString() ?? string.Empty;
                if (oldValue.Equals(newValue))
                {
                    continue;
                }
                var changeValue = new ChangedValue
                {
                    OldValue = oldValue,
                    NewValue = newValue,
                    InfoSiteMatchId = newMatch.InfoSiteMatchId,
                    EventName = newMatch.HomeName + EVENT_VS + newMatch.AwayName,
                    League = newMatch.LeagueName,
                    ReceivedUserIds = receivedUserIds
                };

                changedValues.Add(changeValue);
            }

            return changedValues.ToList();
        }
        static PropertyInfo GetProperty(InfoSiteMatch match, string propertyName)
        {
            return match.GetType().GetProperties().Single(p => p.Name == propertyName);
        }
    }



}