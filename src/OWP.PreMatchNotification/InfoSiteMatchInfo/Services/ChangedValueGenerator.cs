namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using System.Collections.Generic;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.Models;

    public interface ChangedValueGenerator
    {
        IList<ChangedValue> Create(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot);
    }

    public class ChangedValueGeneratorImp : ChangedValueGenerator
    {
        public IList<ChangedValue> Create(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot)
        {
            return new List<ChangedValue>();
        }
    }
}