namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.LinkedMatch.Models;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ChangedLiveMatchGenerator
    {
        Task<List<ChangedValue>> Create(ChangedSnapshot changedSnapshot);
    }

    public class ChangedLiveMatchGeneratorImp : ChangedLiveMatchGenerator
    {
        private readonly CommandRepository commandRepository;
        private const string EVENT_VS = " -vs- ";

        public ChangedLiveMatchGeneratorImp(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<List<ChangedValue>> Create(ChangedSnapshot changedSnapshot)
        {

            return new List<ChangedValue>();
        }

        public async Task<List<ChangedValue>> CreateChangedValueDeletedMatches(ChangedSnapshot changedSnapshot)
        {
            var deletedMatches = changedSnapshot.DeletedMatches;
            var prematchLeagueIds = changedSnapshot.PrematchLeagues.Select(x => x.PreMatchLeagueId).ToList();
            var prematchAssignees = await commandRepository.GetPreMatchLeagueAssignees(prematchLeagueIds);
            var result = new List<ChangedValue>();

            foreach (var match in deletedMatches)
            {
                var prematchLeague = changedSnapshot.PrematchLeagues.FirstOrDefault(x => x.InfoSiteMatchId == match.InfoSiteMatchId);
                var prematchAssigneesByInfosite = prematchAssignees.Where(x => x.PreMatchLeagueId == prematchLeague.PreMatchLeagueId);
                
                result.Add(new ChangedValue
                {
                    MessageTypeId = (int)MsgTemplateCode.LGACancelLiveMatch,
                    League = match.LeagueName,
                    EventName = match.Home + EVENT_VS + match.Away
                });
            }

            return result;
        }
    }
}
