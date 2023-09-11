using OWP.Constants;
using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using OWP.Event.LiveGameList.Builder;
using OWP.Services;

namespace OWP.Event.LiveGameList.Services;

using Fanex.NAP.Client.Helpers.Business;

public interface GetEventByDateService
{
    Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId);
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

    public async Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId)
    {
        var eventByDates = liveGameListRepository.GetEventInformation(eventDate, statusId, IsShowFullEventInformation());
        await eventService.GetInfositeAndAdminsiteInfo(eventByDates.MatchInfos.ToList());

        return BuildEvents(eventByDates);
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

    private bool IsShowFullEventInformation() => permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
}