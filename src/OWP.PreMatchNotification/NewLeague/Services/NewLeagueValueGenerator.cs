namespace OWP.PreMatchNotification.NewLeague.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.NewLeague.Models;

    public interface NewLeagueValueGenerator
    {
        IEnumerable<ChangedValue> Create(IList<PreMatchLeague> oldSnapshot, IList<PreMatchLeague> newSnapshot);
    }

    public class NewLeagueValueGeneratorImp : NewLeagueValueGenerator
    {
        public IEnumerable<ChangedValue> Create(IList<PreMatchLeague> oldSnapshot, IList<PreMatchLeague> newSnapshot)
        {
            var newLeagues = GetNewPreMatchLeagues(oldSnapshot, newSnapshot);

            foreach(var league in newLeagues)
            {
                yield return new ChangedValue
                {
                    IsBlinking = true,
                    League = league.OWPLeagueName
                };
            }
        }

        private IEnumerable<PreMatchLeague> GetNewPreMatchLeagues(IList<PreMatchLeague> oldSnapshot, IList<PreMatchLeague> newSnapshot)
            => newSnapshot.Except(oldSnapshot, new PreMatchLeague()).Distinct(new PreMatchLeague());
    }
}