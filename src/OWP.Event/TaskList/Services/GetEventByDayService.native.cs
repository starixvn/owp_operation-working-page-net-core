using Newtonsoft.Json;
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

        var matchInfo = taskListInfo
            .MatchInfo
            .Where(m=>m.AdminSiteId.HasValue)
            .Select(x => new
            {
                matchId= x.AdminSiteId,
                league= x.League
            });
        var completedMatchIds = await taskListRepository.GetCompletedMatchFromMainDb(JsonConvert.SerializeObject(matchInfo));

        var matchInfos = BuildEvents(taskListInfo, completedMatchIds);
        foreach (var match in matchInfos)
        {
            match.League = "SABA VIETNAM V LEAGUE 1 Virtual PES 24 - 15 Mins Play";
           
        }
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
            matchInfo.IsCompletedMatch = GetMainMatchCompleteStatus(matchInfo, completedMatchIds);

            yield return matchInfo;
        }
    }

    private static bool GetMainMatchCompleteStatus(MatchInfo matchInfo, List<int> completedMatchIds)
    {
        var adminsiteId = matchInfo.ParentAdminSiteId;

        return adminsiteId.HasValue && completedMatchIds.Contains((int)adminsiteId);
    }
}
