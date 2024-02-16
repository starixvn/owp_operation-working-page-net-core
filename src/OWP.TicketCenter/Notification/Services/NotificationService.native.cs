using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Hubs;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Notification.Services;

public interface NotificationService
{
    Task NotifyNewMessageIncome(List<string> userIds, MessageDetailViewModel messageResponse = null);
    Task NotifyNewAddRecipient(List<string> userIds, TicketInfo ticketInfo = null);

    Task NotifyChangeStatusIncome(TicketInfo ticketInfo);

    Task NotifyChangeSubTypeIncome(TicketInfo ticketInfo);

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
            .Users(userIds)
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

    public async Task NotifyChangeStatusIncome(TicketInfo ticketInfo)
    {
        var userIdsString = await GetUserIdsInStringByTicketId(ticketInfo.TicketId);

        await notifyMyTicket.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome(ticketInfo);
        await notifyPreMatch.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome();
    }

    public async Task NotifyChangeSubTypeIncome(TicketInfo ticketInfo)
    {
        var userIdsString = await GetUserIdsInStringByTicketId(ticketInfo.TicketId);

        await notifyMyTicket.Clients
            .Users(userIdsString)
            .NotifyChangeSubTypeIncome(ticketInfo);
        await notifyPreMatch.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome();
    }

    private async Task<List<string>> GetUserIdsInStringByTicketId(int ticketId, int actorId = 0)
    {
        return await repository.GetUserIdsByTicketId(ticketId, actorId);
    }
}