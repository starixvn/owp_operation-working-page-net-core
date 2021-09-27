using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services
{
    public interface InsertOrUpdateRecipient
    {
        Task<bool> Execute(
            IList<Recipient> recipients,
            int ticketId,
            RecipientAction action);
    }

    public class InsertOrUpdateRecipientImp : InsertOrUpdateRecipient
    {
        private readonly Repository repository;
        public InsertOrUpdateRecipientImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Execute(
            IList<Recipient> recipients,
            int ticketId,
            RecipientAction action)
        {
            var recipientDic = GroupRecipientsByType(recipients);
            return await AddOrDeleteRecipientSByType(recipientDic, action, ticketId);
        }

        public async Task<bool> AddOrDeleteRecipientSByType(
            Dictionary<int, List<Recipient>> recipientDic,
            RecipientAction action,
            int ticketId)
        {
            foreach (var key in recipientDic.Keys)
            {
                foreach (var recipient in recipientDic[key])
                {
                    var recipientIds = recipient.RecipientIds
                        .Select(r => r);
                    switch (action)
                    {
                        case RecipientAction.Add:
                            await repository.AddNewRecipients(ticketId, string.Join(',', recipientIds), recipient.RecipientType);
                            break;
                        case RecipientAction.Delete:
                            await repository.DeleteRecipients(ticketId, string.Join(',', recipientIds), recipient.RecipientType);
                            break;
                    }

                }
            }
            return await Task.FromResult(true);
        }

        public Dictionary<int, List<Recipient>> GroupRecipientsByType(IList<Recipient> recipients)
        => recipients
            .GroupBy(r => r.RecipientType)
            .ToDictionary(k => k.Key, x => x.ToList());
    }
}
