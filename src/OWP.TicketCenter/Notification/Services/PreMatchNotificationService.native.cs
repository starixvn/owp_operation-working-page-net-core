using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Hubs;

namespace OWP.TicketCenter.Notification.Services;

public interface PreMatchNotificationService
{
    Task NotifyToReloadTicketInPrematchTable(int ticketId, int ticketType, List<string> userIdsInString);
}

public class PreMatchNotificationServiceImp : PreMatchNotificationService
{
    private readonly IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch;

    private readonly NotificationRepository repository;

    public PreMatchNotificationServiceImp(
        NotificationRepository repository,
        IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch)
    {
        this.notifyPreMatch = notifyPreMatch;
        this.repository = repository;
    }

    public async Task NotifyToReloadTicketInPrematchTable(int ticketId, int ticketType, List<string> userIdsInString)
    {
        var isTicketLeague = IsTicketLeague(ticketType);
        var id = await repository.GetTicketInfoOfPreMatch(ticketId, isTicketLeague);

        await notifyPreMatch
            .Clients
            .All
            //.Users(userIdsInString)
            .NotifyNewMessageIncome(id, isTicketLeague);
    }

    private static bool IsTicketLeague(int ticketType) => ticketType is (int)EventType.League;
}