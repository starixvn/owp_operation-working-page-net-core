namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using OWP.Extensions;
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public interface ChangedValueGenerator
    {
        IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees);

        IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> lst, IList<InfoSiteMatch> linkedMatches);
    }

    internal class InfoSiteMatchComparer : IEqualityComparer<InfoSiteMatch>
    {
        public bool Equals(InfoSiteMatch x, InfoSiteMatch y)
        {
            var isSameId = x.InfoSiteMatchId == y.InfoSiteMatchId;
            var isSameValue = x.LeagueName == y.LeagueName && x.EventDateTime == y.EventDateTime && x.HomeName == y.HomeName && x.AwayName == y.AwayName;
            return isSameId && isSameValue;
        }

        public int GetHashCode(InfoSiteMatch obj)
        {
            return obj.InfoSiteMatchId.GetHashCode();
        }
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
            var detectListChanged = newSnapshot.Except(oldSnapshot, new InfoSiteMatchComparer()).ToList();

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
            var changedValueLSTList = new List<ChangedValue>();

            foreach (var item in linkedMatches)
            {
                var assigneeLSTIds = lst.Where(i => i.OWPLeagueId == item.OWPLeagueId).Select(x => x.AssigneeId).ToList();
                var changeValues = changedValues.Where(i => i.InfoSiteMatchId == item.InfoSiteMatchId).Select(x => new ChangedValue
                {
                    OldValue = x.OldValue,
                    NewValue = x.NewValue,
                    InfoSiteMatchId = x.InfoSiteMatchId,
                    Home = x.Home,
                    Away = x.Away,
                    League = x.League,
                    ReceivedUserIds = assigneeLSTIds,
                    MessageTypeId = x.MessageTypeId
                });
                changedValueLSTList.AddRange(changeValues);
            }

            return changedValueLSTList;
        }

        private IEnumerable<ChangedValue> GetListValueChangedByProperty(InfoSiteMatch oldMatch, InfoSiteMatch newMatch, List<int> receivedUserIds)
        {
            foreach (var propertyName in infoSiteMatchPropertyNames)
            {
                var property = GetProperty(oldMatch, propertyName);

                var oldValue = GetValueByProperty(property, oldMatch);
                var newValue = GetValueByProperty(property, newMatch);

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

        private static string GetValueByProperty(PropertyInfo property, InfoSiteMatch match)
        {
            return property.PropertyType == typeof(DateTime) ? ((DateTime)property.GetValue(match)).ToString("MM/dd/yyyy hh:mm tt") ?? string.Empty
                    : property.GetValue(match)?.ToString() ?? string.Empty;
        }

        private static PropertyInfo GetProperty(InfoSiteMatch match, string propertyName)
        {
            return match.GetType().GetProperties().Single(p => p.Name == propertyName);
        }
    }
}