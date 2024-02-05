using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Hubs;
using OWP.TicketCenter.ViewModels;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Notification.Services;

public interface NotificationService
{
    Task NotifyNewMessageIncome(List<string> userIds, MessageDetailViewModel messageResponse = null);
    Task NotifyNewAddRecipient(List<string> userIds, TicketInfo ticketInfo = null);

    Task NotifyChangeStatusIncome(int ticketId);

    Task NotifyChangeSubTypeIncome(int ticketId, int ticketSubTypeId, string ticketSubTypeName);

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

    public async Task NotifyNewMessageIncome(List<string> userIds, MessageDetailViewModel messageResponse = null)
    {
        await notifyMyTicket.Clients
            .All
            .NotifyNewMessageIncome(messageResponse);
    }

    public async Task NotifyNewMessageLga(List<string> userIds, int owpMatchId, int actorId)
    {
        await notifyMyTicket.Clients
            .Users(userIds)
            .NotifyNewMessageLga(owpMatchId, actorId);
    }

    public async Task NotifyNewAddRecipient(List<string> userIdsString, TicketInfo ticketInfo)
    {
        await notifyMyTicket.Clients
            .Users(userIdsString)
            .NotifyNewAddRecipient(ticketInfo);
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

    public async Task NotifyChangeSubTypeIncome(int ticketId, int ticketSubTypeId, string ticketSubTypeName)
    {
        var userIdsString = await GetUserIdsInStringByTicketId(ticketId);

        await notifyMyTicket.Clients
            .Users(userIdsString)
            .NotifyChangeSubTypeIncome(ticketId, ticketSubTypeId, ticketSubTypeName);
        await notifyPreMatch.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome();
    }

    private async Task<List<string>> GetUserIdsInStringByTicketId(int ticketId, int actorId = 0)
    {
        return await repository.GetUserIdsByTicketId(ticketId, actorId);
    }
}