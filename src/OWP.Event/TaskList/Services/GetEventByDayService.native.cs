using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using OWP.Event.TaskList.Model;
using OWP.Services;

namespace OWP.Event.TaskList.Services;

public interface GetEventByDayService
{
    Task<TaskListModel> GetEventByDay(DateTime eventDate);
}

public class GetEventByDayServiceImp : GetEventByDayService
{
    private readonly TaskListRepository taskListRepository;

    private readonly EventInformationService eventService;

    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;

    public GetEventByDayServiceImp(
        TaskListRepository taskListRepository,
        EventInformationService eventService,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService)
    {
        this.taskListRepository = taskListRepository;
        this.eventService = eventService;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
    }

    public async Task<TaskListModel> GetEventByDay(DateTime eventDate)
    {
        var taskListInfo = taskListRepository.GetTaskListInfo(eventDate);
        await eventService.GetInfositeAndAdminsiteInfo(taskListInfo.MatchInfos.ToList());

        var matchInfos = BuildEvents(taskListInfo);

        return new TaskListModel
        {
            Matches = matchInfos,
            HasDetailInformationPermission = true
        };
    }

    public IEnumerable<MatchInfo> BuildEvents(EventInformation eventInfos)
    {
        foreach (var matchInfo in eventInfos.MatchInfos)
        {
            eventService.BuildEvent(eventInfos, matchInfo);

            matchInfo.Phone = matchInfo.Assignment.LA.Any(x => x.IsCurrentUser && x.BettypeId < 0 && matchInfo.IsOneHourToKickOffTime)
                ? matchInfo.Phone
                : string.Empty;
            matchInfo.IsCompletedMatch = GetMainMatchCompleteStatus(matchInfo, new List<MainMatchInfo>());

            yield return matchInfo;
        }
    }

    private static bool GetMainMatchCompleteStatus(MatchInfo matchInfo, IEnumerable<MainMatchInfo> mainMatchInfos)
    {
        if (!matchInfo.MinorBettypeId.HasValue)
        {
            return matchInfo.IsCompletedMatch;
        }

        var mainMatch = mainMatchInfos.FirstOrDefault(x => x.OWPMatchId == matchInfo.ParentMatchId);

        return mainMatch?.IsCompletedMatch ?? matchInfo.IsCompletedMatch;
    }
}
