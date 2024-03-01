using Fanex.Data.Repository;
using Newtonsoft.Json;
using OWP.Event.EventShared.Criteria;
using OWP.Event.EventShared.Helper;
using OWP.Event.EventShared.Models;

namespace OWP.Event.EventShared;

public interface EventRepository
{
    Task<EventInformation> GetEventInformation(CriteriaBase criteria);

    Task<IEnumerable<MinorMatchInfo>> GetMinorAdminSiteIds(IEnumerable<MinorMatch> minorMatches);
}

public class EventRepositoryImp : EventRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public EventRepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<EventInformation> GetEventInformation(CriteriaBase criteria)
    {
        var result = await dynamicRepository.FetchMultipleAsync(criteria,
            new[]
            {
                typeof(MatchInfo),
                typeof(Assignment),
                typeof(MatchSetting),
                typeof(Television),
                typeof(MatchOffer),
                typeof(TicketInfo)
            });
        var matchInfos = result[0].Cast<MatchInfo>().ToList();
        var assignments = result[1].Cast<Assignment>().ToList();
        var matchSettings = result[2].Cast<MatchSetting>().ToList();
        var televisions = result[3].Cast<Television>().ToList();
        var matchOffers = result[4].Cast<MatchOffer>().ToList();
        var tickets = result[5].Cast<TicketInfo>().ToList();

        return EventHelper.BuildEventInfoInPair(assignments, matchSettings, televisions, matchOffers, tickets, matchInfos);
    }

    public async Task<IEnumerable<MinorMatchInfo>> GetMinorAdminSiteIds(IEnumerable<MinorMatch> minorMatches)
    {
        var criteria = new GetMinorAdminSiteIdsCriteria
        {
            MinorMatches = JsonConvert.SerializeObject(minorMatches)
        };

        return await dynamicRepository.FetchAsync<MinorMatchInfo>(criteria);
    }
}
