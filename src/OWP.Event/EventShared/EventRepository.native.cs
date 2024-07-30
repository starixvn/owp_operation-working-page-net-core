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
        var result = await dynamicRepository.FetchMultipleAsync<MatchInfo, Assignment, MatchSetting, Television, MatchOffer, HasTicketModel>(criteria);
        var matchInfo = result.Item1.ToList();
        var assignments = result.Item2.ToList();
        var matchSettings = result.Item3.ToList();
        var televisions = result.Item4.ToList();
        var matchOffers = result.Item5.ToList();
        var hasTicketModel = result.Item6.ToList();

        return EventHelper.BuildEventInfoInPair(assignments, matchSettings, televisions, matchOffers, matchInfo, hasTicketModel);
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
