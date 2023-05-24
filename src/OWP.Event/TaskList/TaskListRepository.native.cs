using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.Event.EventShared;
using OWP.Event.EventShared.Models;
using OWP.Event.TaskList.Criteria;

namespace OWP.Event.TaskList;

public interface TaskListRepository
{
    EventInformation GetTaskListInfo(DateTime eventDate);

    public Task<DateTime> UpdateTradeStatus(int matchId);
}

public class TaskListRepositoryImp : TaskListRepository
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly EventRepository eventRepository;

    private readonly INapSession session;

    public TaskListRepositoryImp(IDynamicRepository dynamicRepository, INapSession session, EventRepository eventRepository)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
        this.eventRepository = eventRepository;
    }

    public EventInformation GetTaskListInfo(DateTime eventDate)
    {
        var criteria = new GetTaskListCriteria
        {
            UserId = session.UserId,
            EventDate = eventDate
        };

        return eventRepository.GetEventInformation(criteria);
    }

    public async Task<DateTime> UpdateTradeStatus(int matchId)
    {
        var criteria = new UpdateReadyTradeStatusCriteria
        {
            OWPMatchId = matchId,
            UserId = session.UserId
        };
        return (await dynamicRepository.FetchAsync<DateTime>(criteria)).FirstOrDefault();
    }
}