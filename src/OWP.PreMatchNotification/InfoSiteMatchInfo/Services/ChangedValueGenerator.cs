namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.Models;

    internal class InfoSiteMatchComparer : IEqualityComparer<InfoSiteMatch>
    {
        public bool Equals(InfoSiteMatch x, InfoSiteMatch y)
        {
            var isChangedValue = x.InfoSiteMatchId == y.InfoSiteMatchId && (x.LeagueName != y.LeagueName || x.EventDateTime != y.EventDateTime || x.HomeName != y.HomeName || x.AwayName != y.AwayName);
            return isChangedValue;
        }

        public int GetHashCode(InfoSiteMatch obj)
        {
            return obj.InfoSiteMatchId.GetHashCode();
        }
    }
    public interface ChangedValueGenerator
    {
        IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees);

        IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> preMatchAssignees);
    }

    public class ChangedValueGeneratorImp : ChangedValueGenerator
    {
        public IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees)
        {
            var detectListChanged = newSnapshot.Intersect(oldSnapshot, new InfoSiteMatchComparer()).ToList();
            return new List<ChangedValue>();
        }

        public IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> preMatchAssignees)
        {
            return new List<ChangedValue>();
        }
    }
}