using System.Linq;
using System.Threading.Tasks;

namespace OWP.TicketCenter.Services
{
    public interface PermissionService
    {
        Task<bool> CheckPermission(int ticketId, int Creator);
    }

    public class PermissionServiceImp : PermissionService
    {
        private readonly Repository repository;

        public PermissionServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CheckPermission(int ticketId, int CreatorId)
        {
            var recipientsId = await repository.GetUserIdsByTicketId(ticketId);
            return recipientsId.Any(i => i == CreatorId);
        }
    }
}