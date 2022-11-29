using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.Constants;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Models;
using OWP.Services;

namespace OWP.LiveGameList.Services;

public interface EventInformationService
{
    Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId);
}

public class EventInformationServiceImp : EventInformationService
{
    private readonly Repository repository;
    private readonly INapSession currentSession;
    private PermissionService permissionService;
    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;


    public EventInformationServiceImp(Repository repository, INapSession currentSession, GetAdminSiteMatchIdService getAdminSiteMatchIdService, PermissionService permissionService)
    {
        this.repository = repository;
        this.currentSession = currentSession;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        this.permissionService = permissionService;
    }

    public async Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId)
    {
        var matches = repository
            .GetEventInformation(
                new GetEventsCriteria
                {
                    EventDate = eventDate,
                    MatchStatusIds = statusId,
                    ActorId = currentSession.UserId,
                    ShowAll = IsShowFullEventInformation()
                })
            .ToList();
        // await getAdminSiteMatchIdService.UpdateMinorAdminMatchId(matches);
        return matches;
    }

    private bool IsShowFullEventInformation()
    {
        return permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
    }
}

