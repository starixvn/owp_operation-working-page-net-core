using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Hubs;

namespace OWP.TicketCenter.Notification.Services;

public interface NotificationService
{
    Task NotifyNewMessageIncome(List<string> userIds);
    Task NotifyNewAddRecipient(List<string> userIds);

    Task NotifyChangeStatusIncome(int ticketId);

    Task NotifyChangeSubTypeIncome(int ticketId);

    Task NotifyUserClickOnToast(int userId, int ticketId, int eventType);

    Task NotifyNewMessageLga(List<string> userIds, int owpMatchId, int actorId);

    Task NotifyNewAddRecipientLga(List<string> userIds, int owpMatchId, int actorId);
}

public class NotificationServiceImp : NotificationService
{
    private readonly IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notifyMyTicket;

    private readonly IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch;

    private readonly Repository repository;

    public NotificationServiceImp(
        Repository repository,
        IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notifyMyTicket,
        IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch)
    {
        this.notifyMyTicket = notifyMyTicket;
        this.notifyPreMatch = notifyPreMatch;
        this.repository = repository;
    }

    public async Task NotifyNewMessageIncome(List<string> userIds)
    {
        await notifyMyTicket.Clients
            .All
            .NotifyNewMessageIncome();
    }

    public async Task NotifyNewMessageLga(List<string> userIds, int owpMatchId, int actorId)
    {
        await notifyMyTicket.Clients
            .Users(userIds)
            .NotifyNewMessageLga(owpMatchId, actorId);
    }

    public async Task NotifyNewAddRecipient(List<string> userIdsString)
    {
        await notifyMyTicket.Clients
            .Users(userIdsString)
            .NotifyNewAddRecipient();
    }

    public async Task NotifyNewAddRecipientLga(List<string> userIds, int owpMatchId, int actorId)
    {
        await notifyMyTicket.Clients
            .Users(userIds)
            .NotifyNewAddRecipientLga(owpMatchId, actorId);
    }

    public async Task NotifyUserClickOnToast(int userId, int ticketId, int eventType)
    {
        await notifyMyTicket.Clients
            .User(userId.ToString())
            .NotifyUserOnClickToast(ticketId, eventType);
        await notifyPreMatch.Clients
            .User(userId.ToString())
            .NotifyUserOnClickToast(ticketId, eventType);
    }

    public async Task NotifyChangeStatusIncome(int ticketId)
    {
        var userIdsString = await GetUserIdsInStringByTicketId(ticketId);

        await notifyMyTicket.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome();
        await notifyPreMatch.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome();
    }

    public async Task NotifyChangeSubTypeIncome(int ticketId)
    {
        var userIdsString = await GetUserIdsInStringByTicketId(ticketId);

        await notifyMyTicket.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome();
        await notifyPreMatch.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome();
    }

    private async Task<List<string>> GetUserIdsInStringByTicketId(int ticketId, int actorId = 0)
    {
        return await repository.GetUserIdsByTicketId(ticketId, actorId);
    }
}