namespace OWP.PreMatchNotification.NewLeague.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.NewLeague.Models;

    public interface NewLeagueValueGenerator
    {
        IEnumerable<ChangedValue> Create(IList<PreMatchLeague> oldSnapshot, IList<PreMatchLeague> newSnapshot, IList<Assignee> assignees);
    }

    public class NewLeagueValueGeneratorImp : NewLeagueValueGenerator
    {
        public IEnumerable<ChangedValue> Create(IList<PreMatchLeague> oldSnapshot, IList<PreMatchLeague> newSnapshot, IList<Assignee> assignees)
        {
            var newLeagues = GetNewPreMatchLeagues(oldSnapshot, newSnapshot);

            foreach(var league in newLeagues)
            {
                var leagueAssignees = assignees.Where(x => x.PreMatchLeagueId == league.PreMatchLeagueId);

                foreach(var assignee in assignees)
                {
                    yield return new ChangedValue
                    {
                        IsBlinking = true,
                        League = league.OWPLeagueName,
                        UserId = assignee.AssigneeId,
                        Username = assignee.AssigneeName
                    };
                }
            }
        }

        private List<PreMatchLeague> GetNewPreMatchLeagues(IList<PreMatchLeague> oldSnapshot, IList<PreMatchLeague> newSnapshot)
            => newSnapshot.Except(oldSnapshot, new PreMatchLeague()).Distinct(new PreMatchLeague()).ToList();
    }
}