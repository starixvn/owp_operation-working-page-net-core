namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using OWP.Extensions;
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.Models;

    public interface ChangedValueGenerator
    {
        IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees);

        IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> lst, IList<InfoSiteMatch> linkedMatches);
    }

    public class ChangedValueGeneratorImp : ChangedValueGenerator
    {
        private static readonly List<string> infoSiteMatchPropertyNames = new List<string>
        {
            "EventDateTime",
            "HomeName",
            "AwayName",
        };

        private static readonly Dictionary<string, int> messageTypeDictionary = new Dictionary<string, int>
        {
            { "EventDateTime" , (int)MsgTemplateCode.UpdateKOTime },
            { "HomeName" , (int)MsgTemplateCode.UpdateHome },
            { "AwayName" , (int)MsgTemplateCode.UpdateAway }
        };

        public IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees)
        {
            var changedValueList = new List<ChangedValue>();

            foreach (var item in newSnapshot)
            {
                var oldInfositeMatch = oldSnapshot.FirstOrDefault(x => x.InfoSiteMatchId == item.InfoSiteMatchId);

                if (oldInfositeMatch != null)
                {
                    var assigneeIds = preMatchAssignees.Where(i => i.OWPLeagueId == item.OWPLeagueId).Select(x => x.AssigneeId).ToList();
                    changedValueList.AddRange(GetListValueChangedByProperty(oldInfositeMatch, item, assigneeIds));
                }
            }

            return changedValueList;
        }

        public IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> lst, IList<InfoSiteMatch> linkedMatches)
        {
            return new List<ChangedValue>();
        }

        private IEnumerable<ChangedValue> GetListValueChangedByProperty(InfoSiteMatch oldMatch, InfoSiteMatch newMatch, List<int> receivedUserIds)
        {
            foreach (var propertyName in infoSiteMatchPropertyNames)
            {
                var property = GetProperty(oldMatch, propertyName);
                var oldValue = property.GetValue(oldMatch)?.ToString() ?? string.Empty;
                var newValue = property.GetValue(newMatch)?.ToString() ?? string.Empty;

                if (oldValue.Equals(newValue))
                {
                    continue;
                }

                yield return new ChangedValue
                {
                    OldValue = oldValue,
                    NewValue = newValue,
                    InfoSiteMatchId = newMatch.InfoSiteMatchId,
                    Home = oldMatch.HomeName,
                    Away = oldMatch.AwayName,
                    League = oldMatch.LeagueName,
                    ReceivedUserIds = receivedUserIds,
                    MessageTypeId = messageTypeDictionary.GetValue(propertyName)
                };
            }
        }

        private static PropertyInfo GetProperty(InfoSiteMatch match, string propertyName)
        {
            return match.GetType().GetProperties().Single(p => p.Name == propertyName);
        }
    }
}