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
        Task<bool> Execute(
            IList<RecipientModel> recipients,
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

        public async Task<bool> Execute(
            IList<RecipientModel> recipients,
            int ticketId,
            RecipientAction action)
        {
            foreach (var recipient in recipients)
            {
                var recipientIds = recipient.RecipientIds
                    .Select(r => r);
                var recipientIdsString = string.Join(',', recipientIds);
                switch (action)
                {
                    case RecipientAction.Add:
                        await repository.AddNewRecipients(ticketId, recipientIdsString, recipient.RecipientType);
                        break;
                    case RecipientAction.Delete:
                        await repository.DeleteRecipients(ticketId, recipientIdsString, recipient.RecipientType);
                        break;
                    default:
                        break;
                }

            }
            return await Task.FromResult(true);
        }
    }
}
