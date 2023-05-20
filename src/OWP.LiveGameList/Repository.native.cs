using Fanex.Data;
using Fanex.Data.Repository;
using Newtonsoft.Json;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Models;

namespace OWP.LiveGameList;

public interface Repository
{
    EventInformation GetEventInformation(DateTime eventDate, string statusId, int userId, bool isShowAll);

    Task<IEnumerable<MinorMatchInfo>> GetMinorAdminSiteIds(IEnumerable<MinorMatch> minorMatches);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public EventInformation GetEventInformation(DateTime eventDate, string statusId, int userId, bool isShowAll)
    {
        var criteria = new GetEventsCriteria
        {
            EventDate = eventDate,
            MatchStatusIds = statusId,
            ActorId = userId,
            ShowAll = isShowAll
        };
        using var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey());
        using var reader = db.QueryMultiple(criteria);

        var matchInfos = reader.Read<MatchInfo>();
        var assignments = reader.Read<Assignment>();
        var matchSettings = reader.Read<MatchSetting>();
        var televisions = reader.Read<Television>();
        var matchOffers = reader.Read<MatchOffer>();
        var tickets = reader.Read<TicketInfo>();

        var (assignmentsDict, minorAssignmentsDict) = GetAssignmentsInDict(assignments);
        var (matchSettingsDict, minorMatchSettingsDict) = GetMatchSettingsInDict(matchSettings);
        var televisionsDict = GetTelevisionsDict(televisions);
        var matchOffersDict = GetMatchOffersDict(matchOffers);
        var ticketsDict = GetTicketsDict(tickets);

        return new EventInformation
        {
            MatchInfos = matchInfos,
            AssignmentsDict = assignmentsDict,
            MinorAssignmentsDict = minorAssignmentsDict,
            MatchSettingsDict = matchSettingsDict,
            MinorMatchSettingsDict = minorMatchSettingsDict,
            TelevisionsDict = televisionsDict,
            MatchOffersDict = matchOffersDict,
            TicketInfosDict = ticketsDict
        };
    }

    public async Task<IEnumerable<MinorMatchInfo>> GetMinorAdminSiteIds(IEnumerable<MinorMatch> minorMatches)
    {
        var criteria = new GetMinorAdminSiteIdsCriteria
        {
            MinorMatches = JsonConvert.SerializeObject(minorMatches)
        };

        return await dynamicRepository.FetchAsync<MinorMatchInfo>(criteria);
    }

    private static Dictionary<int, List<Television>> GetTelevisionsDict(IEnumerable<Television> televisions)
        => televisions.GroupBy(a => a.OWPMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static Dictionary<int, List<MatchOffer>> GetMatchOffersDict(IEnumerable<MatchOffer> matchOffers)
        => matchOffers.GroupBy(a => a.OWPMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static Dictionary<int, List<TicketInfo>> GetTicketsDict(IEnumerable<TicketInfo> ticketInfos)
        => ticketInfos.GroupBy(a => a.OwpMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static (Dictionary<int, List<Assignment>>, Dictionary<int, List<Assignment>>) GetAssignmentsInDict(
        IEnumerable<Assignment> assignments)
    {
        var assignmentList = assignments.ToList();
        var assignmentsDict = assignmentList
            .GroupBy(a => a.OWPMatchId)
            .ToDictionary(a => a.Key, a => a.ToList());
        var minorAssignmentsDict = assignmentList
            .Where(x => x.ParentMatchId > 0)
            .GroupBy(a => a.ParentMatchId).ToDictionary(a => a.Key, a => a.ToList());

        return (assignmentsDict, minorAssignmentsDict);
    }

    private static (Dictionary<int, List<MatchSetting>>, Dictionary<int, List<MatchSetting>>) GetMatchSettingsInDict(
        IEnumerable<MatchSetting> matchSettings)
    {
        var matchSettingList = matchSettings.ToList();
        var matchSettingsDict = matchSettingList
            .GroupBy(a => a.OWPMatchId)
            .ToDictionary(a => a.Key, a => a.ToList());
        var minorMatchSettingsDict = matchSettingList
            .Where(x => x.ParentMatchId > 0)
            .GroupBy(a => a.ParentMatchId).ToDictionary(a => a.Key, a => a.ToList());

        return (matchSettingsDict, minorMatchSettingsDict);
    }
}
