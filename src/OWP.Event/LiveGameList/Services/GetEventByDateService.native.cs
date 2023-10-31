using OWP.Constants;
using OWP.Event.EventShared.Const;
using OWP.Event.EventShared.Helper;
using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using OWP.Event.LiveGameList.Builder;
using OWP.Services;

namespace OWP.Event.LiveGameList.Services;

using Fanex.NAP.Client.Helpers.Business;

public interface GetEventByDateService
{
    Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId, string userName, string matchType);
}

public class GetEventByDateServiceImp : GetEventByDateService
{
    private readonly PermissionService permissionService;
    private readonly LiveGameListRepository liveGameListRepository;
    private readonly EventInformationService eventService;

    public GetEventByDateServiceImp(
        LiveGameListRepository liveGameListRepository,
        PermissionService permissionService,
        EventInformationService eventService)
    {
        this.liveGameListRepository = liveGameListRepository;
        this.permissionService = permissionService;
        this.eventService = eventService;
    }

    public async Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId, string userName, string matchType)
    {
        var eventByDates = liveGameListRepository.GetEventInformation(eventDate, statusId, IsShowFullEventInformation());
        await eventService.GetInfositeAndAdminsiteInfo(eventByDates.MatchInfos.ToList());

        var eventNew = BuildEvents(eventByDates);
        var filterEventUserName = FilterEventByUsername(eventNew, userName);
        return FilterEventByMatchType(filterEventUserName, matchType);
    }

    public IEnumerable<MatchInfo> BuildEvents(EventInformation eventInfos)
    {
        foreach (var matchInfo in eventInfos.MatchInfos)
        {
            eventService.BuildEvent(eventInfos, matchInfo);
            LengthPropertyBuilder.BuildLengthProperty(matchInfo);

            yield return matchInfo;
        }

    }

    private IEnumerable<MatchInfo> FilterEventByUsername(IEnumerable<MatchInfo> matchInfos, string userName)
    {
        return string.IsNullOrEmpty(userName) ? matchInfos : matchInfos.Where(e => IsUserAssignedToEvent(userName, e));
    }

    private IEnumerable<MatchInfo> FilterEventByMatchType(IEnumerable<MatchInfo> matchInfos, string matchType)
    {
        var types = matchType.Split(',').ToList();
        if (EventHelper.IsSelectAllMatchType(types))
        {
            return matchInfos;
        }
     
        return types.Exists(x => x== EventConst.MatchVirtual)? 
            matchInfos.Where(x => x.IsSaba):
            matchInfos.Where(x => !x.IsSaba);
    }

    private static bool IsUserAssignedToEvent(string username, MatchInfo e)
    {
        var assignments = new List<IEnumerable<Assignment>>
        {
            e.Assignment.OneXTwo,
            e.Assignment.OneXTwoOthers,
            e.Assignment.FastMarket,
            e.Assignment.FastMarketOthers,
            e.Assignment.AllInOne,
            e.Assignment.AllInOneOthers,
            e.Assignment.Handicap,
            e.Assignment.HandicapOthers,
            e.Assignment.Handicap1st,
            e.Assignment.Handicap1stOthers,
            e.Assignment.OverUnder,
            e.Assignment.OverUnderOthers,
            e.Assignment.OverUnder1st,
            e.Assignment.OverUnder1stOthers,
            e.Assignment.OneXTwo1st,
            e.Assignment.OneXTwo1stOthers,
            e.Assignment.Specific15Mins,
            e.Assignment.Specific15MinsOthers,
            e.Assignment.Specific10Mins,
            e.Assignment.Specific10MinsOthers,
            e.Assignment.LA,
            e.Assignment.LAOthers
        };

        foreach (var assignment in assignments)
        {
            if (assignment.Any(i => i.Username == username))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsShowFullEventInformation() => permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
}