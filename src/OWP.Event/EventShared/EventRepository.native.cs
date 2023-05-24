using Fanex.Data;
using Fanex.Data.Repository;
using Newtonsoft.Json;
using OWP.Event.EventShared.Criteria;
using OWP.Event.EventShared.Helper;
using OWP.Event.EventShared.Models;

namespace OWP.Event.EventShared;

public interface EventRepository
{
    EventInformation GetEventInformation(CriteriaBase criteria);

    Task<IEnumerable<MinorMatchInfo>> GetMinorAdminSiteIds(IEnumerable<MinorMatch> minorMatches);
}

public class EventRepositoryImp : EventRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public EventRepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public EventInformation GetEventInformation(CriteriaBase criteria)
    {
        using var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey());
        using var reader = db.QueryMultiple(criteria);

        var matchInfos = reader.Read<MatchInfo>();
        var assignments = reader.Read<Assignment>();
        var matchSettings = reader.Read<MatchSetting>();
        var televisions = reader.Read<Television>();
        var matchOffers = reader.Read<MatchOffer>();
        var tickets = reader.Read<TicketInfo>();

        return EventHelper.BuildEventInfoInDict(assignments, matchSettings, televisions, matchOffers, tickets, matchInfos);
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
