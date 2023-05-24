using Fanex.NAP.Client.Helpers.Web;
using OWP.Event.EventShared;
using OWP.Event.EventShared.Models;
using OWP.Event.LiveGameList.Criteria;

namespace OWP.Event.LiveGameList;

public interface LiveGameListRepository
{
    EventInformation GetEventInformation(DateTime eventDate, string statusId, bool isShowAll);
}

public class LiveGameListRepositoryImp : LiveGameListRepository
{
    private readonly EventRepository eventRepository;

    private readonly INapSession currentSession;

    public LiveGameListRepositoryImp(EventRepository eventRepository, INapSession currentSession)
    {
        this.eventRepository = eventRepository;
        this.currentSession = currentSession;
    }

    public EventInformation GetEventInformation(DateTime eventDate, string statusId, bool isShowAll)
    {
        var criteria = new GetEventsCriteria
        {
            EventDate = eventDate,
            MatchStatusIds = statusId,
            ActorId = currentSession.UserId,
            ShowAll = isShowAll
        };

        return eventRepository.GetEventInformation(criteria);
    }
}
