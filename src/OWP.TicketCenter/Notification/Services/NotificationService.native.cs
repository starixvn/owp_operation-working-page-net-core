namespace OWP.TicketCenter.Notification.Services
{
    using Hubs;
    using Microsoft.AspNetCore.SignalR;

    public interface NotificationService
    {

        Task<bool> NotifyNewMessageIncome(int ticketId);

        Task<bool> NotifyNewMessageLga(int ticketId, int owpMatchId, int actorId);

        Task<bool> NotifyNewAddRecipient(int ticketId, int actorId);

        Task<bool> NotifyNewAddRecipientLga(int ticketId, int owpMatchId, int actorId);

        Task<bool> NotifyChangeStatusIncome(int ticketId);

        Task<bool> NotifyChangeSubTypeIncome(int ticketId);

        Task<bool> NotifyUserClickOnToast(int userId, int ticketId, int eventType);
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

        public async Task<bool> NotifyNewMessageIncome(int ticketId)
        {
            var userIds = await repository.GetUserIdsByTicketId(ticketId);
            var userIdsString = userIds.Distinct().Select(u => u.ToString()).ToList();
            await notifyMyTicket.Clients
                .All
                .NotifyNewMessageIncome();
            await notifyPreMatch.Clients
                .Users(userIdsString)
                .NotifyNewMessageIncome();
            return await Task.FromResult(true);
        }

        public async Task<bool> NotifyNewMessageLga(int ticketId, int owpMatchId, int actorId)
        {
            var userIds = await repository.GetUserIdsByTicketId(ticketId);
            var userIdsString = userIds.Distinct().Select(u => u.ToString()).ToList();
            await notifyMyTicket.Clients
                .Users(userIdsString)
                .NotifyNewMessageLga(owpMatchId, actorId);
            return await Task.FromResult(true);
        }

        public async Task<bool> NotifyNewAddRecipient(int ticketId, int actorId)
        {
            var userIds = await repository.GetUserIdsByTicketId(ticketId);
            var userIdsString = userIds.Where(u => u != actorId)
                .Distinct().Select(u => u.ToString()).ToList();
            await notifyMyTicket.Clients
                .Users(userIdsString)
                .NotifyNewAddRecipient();
            await notifyPreMatch.Clients
                .Users(userIdsString)
                .NotifyNewAddRecipient();
            return await Task.FromResult(true);
        }

        public async Task<bool> NotifyNewAddRecipientLga(int ticketId, int owpMatchId, int actorId)
        {
            var userIds = await repository.GetUserIdsByTicketId(ticketId);
            var userIdsString = userIds.Where(u => u != actorId)
                .Distinct().Select(u => u.ToString()).ToList();
            await notifyMyTicket.Clients
                .Users(userIdsString)
                .NotifyNewAddRecipientLga(owpMatchId, actorId);
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

        public async Task<bool> NotifyChangeSubTypeIncome(int ticketId)
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