namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using OWP.Extensions;
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.LinkedMatch.Models;
    using OWP.PreMatchNotification.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface ChangedLiveMatchGenerator
    {
        List<ChangedValue> Create(ChangedSnapshot changedSnapshot, IList<Assignee> allPrematchEmployees, List<Assignee> prematchAssignees);
    }

    public class ChangedLiveMatchGeneratorImp : ChangedLiveMatchGenerator
    {
        private const string EVENT_VS = " -vs- ";

        public List<ChangedValue> Create(ChangedSnapshot changedSnapshot, IList<Assignee> allPrematchEmployees, List<Assignee> prematchAssignees)
        {
            var cancelLiveMatchesChangedValue = CreateDeletedMatchesChangedValue(changedSnapshot, allPrematchEmployees, prematchAssignees);
            var addLiveMatchesChangedValue = CreateDeletedMatchesChangedValue(changedSnapshot, allPrematchEmployees, prematchAssignees);

            return cancelLiveMatchesChangedValue.Union(addLiveMatchesChangedValue).ToList();
        }

        public IEnumerable<ChangedValue> CreateDeletedMatchesChangedValue(ChangedSnapshot changedSnapshot, IList<Assignee> allPrematchEmployees, List<Assignee> prematchAssignees)
            => CreateMatchesChangedValue(true, changedSnapshot, allPrematchEmployees, prematchAssignees);

        public IEnumerable<ChangedValue> CreateInsertedMatchesChangedValue(ChangedSnapshot changedSnapshot, IList<Assignee> allPrematchEmployees, List<Assignee> prematchAssignees)
            => CreateMatchesChangedValue(false, changedSnapshot, allPrematchEmployees, prematchAssignees);

        public IEnumerable<ChangedValue> CreateMatchesChangedValue(
            bool isDeletedMatches, ChangedSnapshot changedSnapshot, 
            IList<Assignee> allPrematchEmployees, List<Assignee> prematchAssignees)
        {
            var matches = isDeletedMatches ? changedSnapshot.DeletedMatches : changedSnapshot.InsertedMatches;
            var messageTypeId = isDeletedMatches
                ? (int)MsgTemplateCode.LGACancelLiveMatch
                : (int)MsgTemplateCode.LGAAddLiveMatch;

            foreach (var match in matches)
            {
                var prematchLeague = changedSnapshot.PrematchLeagues.GetValue(match.InfoSiteMatchId, new PrematchLeague());
                var prematchAssigneesByInfosite = prematchAssignees.Where(x => x.PreMatchLeagueId == prematchLeague.PreMatchLeagueId);

                yield return new ChangedValue
                {
                    MessageTypeId = messageTypeId,
                    League = match.LeagueName,
                    EventName = match.Home + EVENT_VS + match.Away,
                    ReceivedUserIds = prematchAssigneesByInfosite.Count() == 0
                        ? allPrematchEmployees.Select(x => x.AssigneeId).ToList()
                        : prematchAssigneesByInfosite.Select(x => x.AssigneeId).ToList()
                };
            }
        }
    }
}
