using Fanex.NAP.Client.Helpers.Web;
using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Hubs;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Notification.Services;

public interface PreMatchNotificationService
{
    Task NotifyToReloadTicketInPrematchTable(int ticketId, int ticketType, List<string> userIdsInString);
}

public class PreMatchNotificationServiceImp : PreMatchNotificationService
{
    private readonly IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch;

    private readonly NotificationRepository repository;

    private readonly INapSession session;

    public PreMatchNotificationServiceImp(
        NotificationRepository repository,
        IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch,
        INapSession session)
    {
        this.notifyPreMatch = notifyPreMatch;
        this.repository = repository;
        this.session = session;
    }

    public async Task NotifyToReloadTicketInPrematchTable(
        int ticketId,
         int ticketType,
          List<string> userIdsInString,
          MessageDetailViewModel messageResponse)
    {
        var isTicketLeague = ticketType is (int)EventType.League;
        var id = await repository.GetTicketInfoOfPreMatch(ticketId, isTicketLeague);
        var tickets = await QueryPrematchTicket(id, ticketId, isTicketLeague);

        // TODO
        await notifyPreMatch
            .Clients
            .All
            .NotifyNewMessageIncome(id, isTicketLeague, new { tickets, messageResponse });
    }

    private async Task<object> QueryPrematchTicket(int id, int ticketId, bool isTicketLeague)
    {
        var tickets = new List<PrematchTicketInfo>();
        if (isTicketLeague)
        {
            tickets.AddRange(await repository.QueryPrematchTicket<TicketLeagueBasicInfo>(new QueryAllLeaguesTicketCriteria
            {
                OWPLeagueIds = $"{id}",
                ActorId = session.UserId
            }));
        }
        else
        {
            tickets.AddRange(await repository.QueryPrematchTicket<TicketMatchBasicInfo>(new QueryAllMatchesTicketCriteria
            {
                InfoSiteMatchIds = $"{id}",
                ActorId = session.UserId
            }));
        }
        return tickets.Find(t => t.TicketId == ticketId);
    }
}