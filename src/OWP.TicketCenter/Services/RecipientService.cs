using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services
{
    public interface RecipientService
    {
        Task<bool> InsertManyRecipient(
            IList<InsertOrRemoveManyRecipientModel> recipients,
            int ticketId);

        Task<bool> InsertManyRecipientReader(IList<InsertOrRemoveManyRecipientModel> recipients,
            int ticketId);

        Task<bool> RemoveManyRecipientReader(IList<InsertOrRemoveManyRecipientModel> recipients,
            int ticketId);
    }

    public class RecipientServiceImp : RecipientService
    {
        private readonly Repository repository;
        public RecipientServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> InsertManyRecipient(
            IList<InsertOrRemoveManyRecipientModel> recipients,
            int ticketId)
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


        public async Task<bool> RemoveManyRecipientReader(IList<InsertOrRemoveManyRecipientModel> recipients,
            int ticketId)
        {
            var recipientIds = $"{await BuildRecipientIdsFromGroup(recipients)},{BuildRecipientIdsIndividual(recipients)}";

            await repository.RemoveManyRecipientReader(recipientIds, ticketId);

            return await Task.FromResult(true);
        }

        public async Task<bool> InsertManyRecipientReader(IList<InsertOrRemoveManyRecipientModel> recipients,
            int ticketId)
        {
            var recipientIds = $"{await BuildRecipientIdsFromGroup(recipients)},{BuildRecipientIdsIndividual(recipients)}";
            
            await repository.InsertManyRecipientReader(recipientIds, ticketId, false);

            return await Task.FromResult(true);
        }

        private async Task<string> BuildRecipientIdsFromGroup(IList<InsertOrRemoveManyRecipientModel> recipients)
        {
            var group = recipients
                .FirstOrDefault(r => r.RecipientType == (int)RecipientType.Group);

            var groupIdString = group != null
                ? string.Join(",", group.RecipientIds)
                : string.Empty;

            var recipientIdInGroup = await repository.GetRecipientIdInManyGroup(groupIdString);

            return string.Join(",", recipientIdInGroup);
        }

        private string BuildRecipientIdsIndividual(IList<InsertOrRemoveManyRecipientModel> recipients)
        {
            var recipientIndividual = recipients
                .FirstOrDefault(r => r.RecipientType == (int) RecipientType.Individual);
            return recipientIndividual != null
                ? string.Join(",", recipientIndividual.RecipientIds)
                : string.Empty;
        }
       
    }
}
