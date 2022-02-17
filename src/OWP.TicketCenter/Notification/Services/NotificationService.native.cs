using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Notification.Services
{
  using Hubs;
  using Microsoft.AspNetCore.SignalR;
  using Models;
  using OWP.Services;
  using OWP.Utils;

  public interface NotificationService
  {
    Task Notify(NotificationSource source);

    Task<bool> NotifyNewMessageIncome(int ticketId);

    Task<bool> NotifyNewAddRecipient(int ticketId);

    Task<bool> NotifyChangeStatusIncome(int ticketId);

    Task<bool> NotifyUserClickOnToast(int userId, int ticketId, int eventType);
  }

  public class NotificationServiceImp : NotificationService
  {
    private readonly NotifyEngine notifyEngine;

    private readonly IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notifyMyTicket;

    private readonly IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch;

    private readonly Repository repository;

    public NotificationServiceImp(
        NotifyEngine notifyEngine,
        Repository repository,
        IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notifyMyTicket,
        IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch)
    {
      this.notifyEngine = notifyEngine;
      this.notifyMyTicket = notifyMyTicket;
      this.notifyPreMatch = notifyPreMatch;
      this.repository = repository;
    }

    public async Task Notify(NotificationSource source)
    {
      await notifyEngine.Notify(source.SessionId, messageTuple =>
      {
        var userIds = string.Join(",", messageTuple.Item1.Select(x => x.UserId));

        var awaiter = repository
                  .GetMessageUnreadByMultipleUserId(userIds)
                  .ConfigureAwait(true);

        var messageUnread = awaiter.GetAwaiter().GetResult();

        return messageTuple.Item2.Select(x => new NotificationModel
        {
          IsImportant = x.IsImportant,
          MessageType = x.MessageType,
          MessageContent = x.MessageContent,
          TicketId = source.TicketId,
          EventType = source.EventType,
          TotalMessageUnread = messageUnread?.FirstOrDefault(m => m.UserId == x.UserId)!.Total,
          IsNewMessage = source.IsNewMessage
        }).DistinctBy(x => new
        {
          x.IsImportant,
          x.MessageType,
          x.MessageContent,
          x.TicketId,
          x.EventType,
          x.TotalMessageUnread,
          x.IsNewMessage
        }).ToList();
      });
    }

    public async Task<bool> NotifyNewMessageIncome(int ticketId)
    {
      var userIds = await repository.GetUserIdsByTicketId(ticketId);
      var userIdsString = userIds.Distinct().Select(u => u.ToString()).ToList();
      await notifyMyTicket.Clients
          .Users(userIdsString)
          .NotifyNewMessageIncome();
      await notifyPreMatch.Clients
          .Users(userIdsString)
          .NotifyNewMessageIncome();
      return await Task.FromResult(true);
    }

    public async Task<bool> NotifyNewAddRecipient(int ticketId)
    {
      var userIds = await repository.GetUserIdsByTicketId(ticketId);
      var userIdsString = userIds.Distinct().Select(u => u.ToString()).ToList();
      await notifyMyTicket.Clients
          .Users(userIdsString)
          .NotifyNewAddRecipient();
      await notifyPreMatch.Clients
          .Users(userIdsString)
          .NotifyNewAddRecipient();
      return await Task.FromResult(true);
    }

    public async Task<bool> NotifyUserClickOnToast(int userId, int ticketId, int eventType)
    {
      await notifyMyTicket.Clients
          .User(userId.ToString())
          .NotifyUserOnClickToast(ticketId, eventType);
      await notifyPreMatch.Clients
          .User(userId.ToString())
          .NotifyUserOnClickToast(ticketId, eventType);
      return await Task.FromResult(true);
    }

    public async Task<bool> NotifyChangeStatusIncome(int ticketId)
    {
      var userIds = await repository.GetUserIdsByTicketId(ticketId);
      var userIdsString = userIds.Distinct().Select(u => u.ToString()).ToList();
      await notifyMyTicket.Clients
          .Users(userIdsString)
          .NotifyChangeStatusIncome();
      await notifyPreMatch.Clients
          .Users(userIdsString)
          .NotifyChangeStatusIncome();
      return await Task.FromResult(true);
    }
  }
}