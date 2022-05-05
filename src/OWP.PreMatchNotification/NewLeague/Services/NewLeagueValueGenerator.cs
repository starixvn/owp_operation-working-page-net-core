namespace OWP.PreMatchNotification.NewLeague.Services
{
  using Fanex.Logging;
  using Newtonsoft.Json;
  using OWP.PreMatchNotification.Constants;
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

        private readonly ILogger logger;

        public NewLeagueValueGeneratorImp(DateTimeHelper dateTimeHelper, ILogger logger)
        {
            this.dateTimeHelper = dateTimeHelper;
            this.logger = logger;
        }

        public IEnumerable<ChangedValue> Create(IList<PreMatchLeague> oldSnapshot,
            IList<PreMatchLeague> newSnapshot,
            IList<Assignee> assignees,
            IList<InfoSiteMatch> infoSiteMatches)
        {
            var newLeagues = GetNewPreMatchLeagues(oldSnapshot, newSnapshot, infoSiteMatches);
            logger.Info($"New league: {JsonConvert.SerializeObject(newLeagues)}");
            foreach (var league in newLeagues)
            {
                yield return new ChangedValue
                {
                    League = league.OWPLeagueName,
                    //ReceivedUserIds = assignees.Select(x => x.AssigneeId).ToList(),
                    ReceivedUserIds= new List<int>{20902509},
                    MessageTypeId = (int)MsgTemplateCode.NewLeague
                };
            }
        }

        private IEnumerable<PreMatchLeague> GetNewPreMatchLeagues(IList<PreMatchLeague> oldSnapshot,
            IList<PreMatchLeague> newSnapshot, IList<InfoSiteMatch> infoSiteMatches)
        {
            var newLeagues = newSnapshot.Except(oldSnapshot, new PreMatchLeague()).ToList();
            logger.Info($"New league Except: {JsonConvert.SerializeObject(newLeagues)}");
           logger.Info($"infoSiteMatches: {JsonConvert.SerializeObject(infoSiteMatches)}");
           
            var currentDate = dateTimeHelper.GetCurrentDate();
            var fromDate = currentDate.AddHours(12);
            var toDate = currentDate.AddDays(1).AddHours(11).AddMinutes(59);
            logger.Info($"fromDate: {fromDate} - todate: {toDate}");
            foreach (var league in newLeagues)
            {
                var matches = infoSiteMatches.Where(x =>
                    x.OWPLeagueId == league.OWPLeagueId &&
                    x.EventDateTime >= fromDate &&
                    x.EventDateTime <= toDate);

                if (matches.Any())
                {
                    yield return league;
                }
            }
        }
    }
}