namespace OWP.TicketCenter.Services
{
    public interface PermissionService
    {
        Task<bool> HasPermission(int ticketId, int creatorId);

        Task<bool> HasPermissionRemoveLastRecipient(int ticketId);

        Task<bool> HasExistedTicket(int ticketId);
    }

    public class PermissionServiceImp : PermissionService
    {
        private readonly Repository repository;

        public PermissionServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HasPermission(int ticketId, int creatorId)
        {
            var recipientsId = await repository.GetUserIdsByTicketId(ticketId);
            return recipientsId.Any(i => i == creatorId);
        }

        public async Task<bool> HasPermissionRemoveLastRecipient(int ticketId)
        {
            var recipients = await repository.GetRecipientByTicketId(ticketId);
            return recipients.Count() > 1;
        }

        public async Task<bool> IsDeletedTicket(int ticketId)
        {
            var recipients = await repository.GetRecipientByTicketId(ticketId);
            return recipients.Count() > 1;
        }

        public async Task<bool> HasExistedTicket(int ticketId)
           => await repository.QueryExistedTicketId(ticketId);
    }
}