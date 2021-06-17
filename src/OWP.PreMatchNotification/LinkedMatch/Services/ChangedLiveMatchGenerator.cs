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
        List<ChangedValue> Create(ChangedSnapshot changedSnapshot, Employee employees);
    }

    public class ChangedLiveMatchGeneratorImp : ChangedLiveMatchGenerator
    {
        private const string EVENT_VS = " -vs- ";

        public List<ChangedValue> Create(ChangedSnapshot changedSnapshot, Employee employees)
        {
            var cancelLiveMatchesChangedValue = CreateDeletedMatchesChangedValue(changedSnapshot, employees);
            var addLiveMatchesChangedValue = CreateDeletedMatchesChangedValue(changedSnapshot, employees);

            return cancelLiveMatchesChangedValue.Union(addLiveMatchesChangedValue).ToList();
        }

        public IEnumerable<ChangedValue> CreateDeletedMatchesChangedValue(ChangedSnapshot changedSnapshot, Employee employees)
            => CreateMatchesChangedValue(true, changedSnapshot, employees);

        public IEnumerable<ChangedValue> CreateInsertedMatchesChangedValue(ChangedSnapshot changedSnapshot, Employee employees)
            => CreateMatchesChangedValue(false, changedSnapshot, employees);

        public IEnumerable<ChangedValue> CreateMatchesChangedValue(bool isDeletedMatches, ChangedSnapshot changedSnapshot, Employee employees)
        {
            var matches = isDeletedMatches ? changedSnapshot.DeletedMatches : changedSnapshot.InsertedMatches;
            var messageTypeId = isDeletedMatches
                ? (int)MsgTemplateCode.LGACancelLiveMatch
                : (int)MsgTemplateCode.LGAAddLiveMatch;

            foreach (var match in matches)
            {
                var prematchInfosite = employees.PrematchInfosites.GetValue(match.InfoSiteMatchId);
                var prematchAssigneeIds = prematchInfosite.PrematchAssigneeIds;

                yield return new ChangedValue
                {
                    IsBlinking = true,
                    MessageTypeId = messageTypeId,
                    League = match.LeagueName,
                    EventName = match.Home + EVENT_VS + match.Away,
                    ReceivedUserIds = prematchAssigneeIds.Count() == 0
                        ? employees.AllPrematchEmployees.Union(employees.AllLSTEmployees).Select(x => x.AssigneeId).ToList()
                        : prematchAssigneeIds.Union(employees.AllLSTEmployees.Select(x => x.AssigneeId)).ToList()
                };
            }
        }
    }
}
