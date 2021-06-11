namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using System.Collections.Generic;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.Models;

    public interface ChangedMatchChangedValueGenerator
    {
        IList<ChangedValue> Create(List<int> matcheIds, IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot);
    }

    public class ChangedMatchChangedValueGeneratorImp : ChangedMatchChangedValueGenerator
    {
        public IList<ChangedValue> Create(List<int> matcheIds, IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot)
        {
            return new List<ChangedValue>();
        }
    }
}