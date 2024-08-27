using Microsoft.AspNetCore.SignalR;
using OWP.AccountSetting.Profile;
using OWP.TicketCenter.Hubs;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Notification.Services;

public interface NotificationService
{
    Task NotifyNewMessageIncome(List<string> userIds, TicketInfo ticketInfo, MessageDetailViewModel messageResponse = null);

    Task NotifyNewAddRecipient(List<string> userIds, TicketInfo ticketInfo);

    Task NotifyChangeStatusIncome(TicketInfo ticketInfo);

    Task NotifyChangeSubTypeIncome(TicketInfo ticketInfo);

    Task NotifyUserClickOnToast(int userId, int ticketId, int eventType);

    Task NotifyNewMessageLga(List<string> userIds, int parentMatchId, int actorId);

    Task NotifyNewAddRecipientLga(List<string> userIds, int parentMatchId, int actorId);

    Task NotifyUpdateReactIconMessage(List<string> userIds, MessageReactIcon messageReactIcon, string action, int ticketId);
}

public class NotificationServiceImp : NotificationService
{
    private readonly IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notifyMyTicket;

    private readonly IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch;

    private readonly Repository repository;

    private readonly ProfileRepository profileRepository;

    public NotificationServiceImp(
        Repository repository,
        ProfileRepository profileRepository,
        IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notifyMyTicket,
        IHubContext<PreMatchNotifyHubImp, PreMatchNotifyHub> notifyPreMatch)
    {
        this.notifyMyTicket = notifyMyTicket;
        this.notifyPreMatch = notifyPreMatch;
        this.repository = repository;
        this.profileRepository = profileRepository;
    }

    public async Task NotifyNewMessageIncome(List<string> userIds, TicketInfo ticketInfo, MessageDetailViewModel messageResponse = null)
    {
        var ticketReaders = await repository.GetTickerReaderInfoByTicketId(ticketInfo.TicketId);
        var lastComments = (await repository.GetTwoLastComment(new List<int> { ticketInfo.TicketId })).ToList();
        var (_, avatarFileName) = await profileRepository.QueryUserProfileInfo(messageResponse.UserId);
        messageResponse.AvatarFileName = avatarFileName;
        ticketInfo.Messages = lastComments;
        ticketInfo.LastCommentDate = lastComments.Count != 0
            ? lastComments[0].CreatedDate
            : new MessageInfoModel().CreatedDate;
        foreach (var ticketReader in ticketReaders)
        {
            ticketInfo.IsPinned = ticketReader.IsPinned;
            ticketInfo.IsRead = ticketReader.IsRead;

            await notifyMyTicket.Clients
                .User(ticketReader.RecipientId)
                .NotifyNewMessageIncome(messageResponse, ticketInfo);
            await NotifyHighlightSubtype(ticketReader.RecipientId, ticketInfo);
        }
    }

    public async Task NotifyNewMessageLga(List<string> userIds, int parentMatchId, int actorId)
    {
        await notifyMyTicket.Clients
            .Users(userIds)
            .NotifyNewMessageLga(parentMatchId, actorId);
    }

    public async Task NotifyNewAddRecipient(List<string> userIds, TicketInfo ticketInfo)
    {
        var ticketReaders = await repository.GetTickerReaderInfoByTicketId(ticketInfo.TicketId);
        var newReaders = ticketReaders.IntersectBy(userIds, x => x.RecipientId.ToString());
        foreach (var ticketReader in newReaders)
        {
            ticketInfo.IsPinned = ticketReader.IsPinned;
            ticketInfo.IsRead = ticketReader.IsRead;

            await notifyMyTicket.Clients
                .User(ticketReader.RecipientId)
                .NotifyNewAddRecipient(ticketInfo);

            await NotifyHighlightSubtype(ticketReader.RecipientId, ticketInfo);

        }
    }

    public async Task NotifyNewAddRecipientLga(List<string> userIds, int parentMatchId, int actorId)
    {
        await notifyMyTicket.Clients
            .Users(userIds)
            .NotifyNewAddRecipientLga(parentMatchId, actorId);
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
        var ticketReaders = await repository.GetTickerReaderInfoByTicketId(ticketInfo.TicketId);
        var userIdsString = ticketReaders.Select(x => x.RecipientId).Distinct().ToList();

        foreach (var ticketReader in ticketReaders)
        {
            ticketInfo.IsPinned = ticketReader.IsPinned;
            ticketInfo.IsRead = ticketReader.IsRead;

            await notifyMyTicket.Clients
            .User(ticketReader.RecipientId)
            .NotifyChangeStatusIncome(ticketInfo);
        }

        await notifyPreMatch.Clients
            .Users(userIdsString)
            .NotifyChangeStatusIncome(ticketInfo.TicketId, ticketInfo.TicketStatus);
    }

    public async Task NotifyChangeSubTypeIncome(TicketInfo ticketInfo)
    {
        var ticketReaders = await repository.GetTickerReaderInfoByTicketId(ticketInfo.TicketId);
        var userIdsString = ticketReaders.Select(x => x.RecipientId).Distinct().ToList();

        foreach (var ticketReader in ticketReaders)
        {
            ticketInfo.IsPinned = ticketReader.IsPinned;
            ticketInfo.IsRead = ticketReader.IsRead;

            await notifyMyTicket.Clients
            .User(ticketReader.RecipientId)
            .NotifyChangeSubTypeIncome(ticketInfo);
        }
        await notifyPreMatch.Clients
            .Users(userIdsString)
            .NotifyChangeSybTypeIncome(ticketInfo);
    }

    public async Task NotifyUpdateReactIconMessage(List<string> userIds, MessageReactIcon messageReactIcon, string action, int ticketId)
    {
        await notifyMyTicket.Clients
            .Users(userIds)
            .NotifyUpdateReactIconMessage(messageReactIcon, action, ticketId);
        await notifyPreMatch.Clients
            .Users(userIds)
            .NotifyUpdateReactIconMessage(messageReactIcon, action, ticketId);
    }

    private async Task NotifyHighlightSubtype(string userId, TicketInfo ticketInfo)
    {
        await notifyMyTicket.Clients
            .User(userId)
            .NotifyHighlightSubtype(new ConsolidationSubtypeModel()
            {
                TypeId = ticketInfo.TicketType,
                SubTypeId = ticketInfo.TicketSubTypeId,
                SubTypeName = $"{ticketInfo.TypeName} - {ticketInfo.TicketSubTypeName ?? "None"}",
                IsRead = ticketInfo.IsRead,
                EventType = ticketInfo.EventType
            });

    }
}