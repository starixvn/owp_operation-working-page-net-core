namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using System.Collections.Generic;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.Models;

    public interface ChangedValueGenerator
    {
        IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees);

        IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> preMatchAssignees);
    }

    public class ChangedValueGeneratorImp : ChangedValueGenerator
    {
        public IList<ChangedValue> CreateAssigneeMessages(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot, IList<Assignee> preMatchAssignees)
        {
            return new List<ChangedValue>();
        }

        public IList<ChangedValue> CreateLSTMessages(IList<ChangedValue> changedValues, IList<Assignee> preMatchAssignees)
        {
            return new List<ChangedValue>();
        }
    }
}