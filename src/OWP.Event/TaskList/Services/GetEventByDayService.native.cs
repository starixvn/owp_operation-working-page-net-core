using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using OWP.Event.TaskList.Model;

namespace OWP.Event.TaskList.Services;

public interface GetEventByDayService
{
    Task<TaskListModel> GetEventByDay(DateTime eventDate);
}

public class GetEventByDayServiceImp : GetEventByDayService
{
    private readonly TaskListRepository taskListRepository;

    private readonly EventInformationService eventService;

    public GetEventByDayServiceImp(
        TaskListRepository taskListRepository,
        EventInformationService eventService)
    {
        this.taskListRepository = taskListRepository;
        this.eventService = eventService;
    }

    public async Task<TaskListModel> GetEventByDay(DateTime eventDate)
    {
        var taskListInfo = await taskListRepository.GetTaskListInfo(eventDate);
        await eventService.GetInfositeAndAdminsiteInfo(taskListInfo.MatchInfo.ToList());

        var adminsiteIds = taskListInfo.MatchInfo.Select(x => x.AdminSiteId);
        var completedMatchIds = await taskListRepository.GetCompletedMatchFromMainDb(adminsiteIds);

        var matchInfos = BuildEvents(taskListInfo, completedMatchIds);

        return new TaskListModel
        {
            Matches = matchInfos,
            HasDetailInformationPermission = true
        };
    }

    public IEnumerable<MatchInfo> BuildEvents(EventInformation eventInfos, List<int> completedMatchIds)
    {
        foreach (var matchInfo in eventInfos.MatchInfo)
        {
            eventService.BuildEvent(eventInfos, matchInfo);

            matchInfo.Phone = matchInfo.Assignment.LA.Any(x => x.IsCurrentUser && x.BettypeId < 0 && matchInfo.IsOneHourToKickOffTime)
                ? matchInfo.Phone
                : string.Empty;
            matchInfo.IsCompletedMatch = GetMainMatchCompleteStatus(matchInfo, completedMatchIds, eventInfos.MatchInfo);

            yield return matchInfo;
        }
    }

    private static bool GetMainMatchCompleteStatus(MatchInfo matchInfo, List<int> completedMatchIds, IEnumerable<MatchInfo> matchInfos)
    {
        var adminsiteId = matchInfo.ParentAdminSiteId;

        return adminsiteId.HasValue && completedMatchIds.Contains((int)adminsiteId);
    }
}
