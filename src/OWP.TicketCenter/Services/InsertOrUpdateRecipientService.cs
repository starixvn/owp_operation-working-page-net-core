using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services
{
    public interface InsertOrUpdateRecipientService
    {
        Task<bool> InsertManyRecipient(
            IList<InsertManyRecipientModel> recipients,
            int ticketId,
            RecipientAction action);
    }

    public class InsertOrUpdateRecipientServiceImp : InsertOrUpdateRecipientService
    {
        private readonly Repository repository;
        public InsertOrUpdateRecipientServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> InsertManyRecipient(
            IList<InsertManyRecipientModel> recipients,
            int ticketId,
            RecipientAction action)
        {
            foreach (var recipient in recipients)
            {
                var recipientIds = recipient.RecipientIds
                    .Select(r => r);
                var recipientIdsString = string.Join(',', recipientIds);
                await repository.AddManyRecipients(ticketId, recipientIdsString, recipient.RecipientType);

            }
            return await Task.FromResult(true);
        }
    }
}
