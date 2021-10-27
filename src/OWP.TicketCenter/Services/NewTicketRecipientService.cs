using System.Threading.Tasks;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services
{
    public interface NewTicketRecipientService
    {
        Task<bool> InsertRecipient(RecipientRequestMode request);
    }

    public class NewTicketRecipientServiceImp: NewTicketRecipientService
    {
        private readonly RecipientService recipientService;

        public NewTicketRecipientServiceImp(RecipientService recipientService)
        {
            this.recipientService = recipientService;
        }

        public async Task<bool> InsertRecipient(RecipientRequestMode request)
        {
            await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

            await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId);

            return await Task.FromResult(true);
        }
    }
}
