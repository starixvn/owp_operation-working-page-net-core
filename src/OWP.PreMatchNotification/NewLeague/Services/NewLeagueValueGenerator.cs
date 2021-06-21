namespace OWP.PreMatchNotification.NewLeague.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.NewLeague.Models;
    using OWP.PreMatchNotification.Services;

    public interface NewLeagueValueGenerator
    {
        IEnumerable<ChangedValue> Create(IList<PreMatchLeague> oldSnapshot,
            IList<PreMatchLeague> newSnapshot, IList<Assignee> assignees,
            IList<InfoSiteMatch> infoSiteMatches);
    }

    public class NewLeagueValueGeneratorImp : NewLeagueValueGenerator
    {
        private readonly DateTimeHelper dateTimeHelper;

        public NewLeagueValueGeneratorImp(DateTimeHelper dateTimeHelper)
        {
            this.dateTimeHelper = dateTimeHelper;
        }

        public IEnumerable<ChangedValue> Create(IList<PreMatchLeague> oldSnapshot,
            IList<PreMatchLeague> newSnapshot,
            IList<Assignee> assignees,
            IList<InfoSiteMatch> infoSiteMatches)
        {
            var newLeagues = GetNewPreMatchLeagues(oldSnapshot, newSnapshot, infoSiteMatches);

            foreach (var league in newLeagues)
            {
                yield return new ChangedValue
                {
                    League = league.OWPLeagueName,
                    ReceivedUserIds = assignees.Select(x => x.AssigneeId).ToList()
                };
            }
        }

        private IEnumerable<PreMatchLeague> GetNewPreMatchLeagues(IList<PreMatchLeague> oldSnapshot,
            IList<PreMatchLeague> newSnapshot, IList<InfoSiteMatch> infoSiteMatches)
        {
            var newLeagues = newSnapshot.Except(oldSnapshot, new PreMatchLeague()).Distinct(new PreMatchLeague()).ToList();
            var currentDate = dateTimeHelper.GetCurrentDate();
            var fromDate = currentDate.AddHours(12);
            var toDate = currentDate.AddDays(1).AddHours(11).AddMinutes(59);

            foreach (var league in newLeagues)
            {
                var matches = infoSiteMatches.Where(x =>
                    x.OWPLeagueId == league.OWPLeagueId &&
                    x.EventDateTime.AddHours(8) >= fromDate &&
                    x.EventDateTime.AddHours(8) <= toDate);

                if (matches.Any())
                {
                    yield return league;
                }
            }
        }
    }
}