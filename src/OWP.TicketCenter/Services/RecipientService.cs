using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services;

public interface RecipientService
{
    Task<bool> InsertManyRecipient(
        IList<InsertOrRemoveManyRecipientModel> recipients,
        int ticketId);

    Task<bool> InsertManyRecipientReader(IList<InsertOrRemoveManyRecipientModel> recipients,
        int ticketId, int creatorId);

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

            var criteria = new AddManyRecipientCriteria
            {
                TicketId = ticketId,
                RecipientIds = recipientIdsString,
                RecipientType = recipient.RecipientType
            };
            await repository.AddManyRecipients(criteria);
        }
        return await Task.FromResult(true);
    }

    public async Task<bool> RemoveManyRecipientReader(IList<InsertOrRemoveManyRecipientModel> recipients,
        int ticketId)
    {
        var recipientGroupIds = BuildRecipientIdsByType(recipients, RecipientType.Group);
        var recipientIndividualIds = BuildRecipientIdsByType(recipients, RecipientType.Individual);
        var recipientSectionIds = BuildRecipientIdsByType(recipients, RecipientType.Section);

        var criteria = new RemoveManyRecipientReaderCriteria
        {
            TicketId = ticketId,
            GroupIds = recipientGroupIds,
            IndividualIds = recipientIndividualIds,
            SectionIds = recipientSectionIds
        };

        await repository.RemoveManyRecipientReader(criteria);

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertManyRecipientReader(IList<InsertOrRemoveManyRecipientModel> recipients,
        int ticketId, int creatorId)
    {
        var recipientGroupIds = BuildRecipientIdsByType(recipients, RecipientType.Group);
        var recipientIndividualIds = BuildRecipientIdsByType(recipients, RecipientType.Individual);
        var recipientSectionIds = BuildRecipientIdsByType(recipients, RecipientType.Section);
        var criteria = new InsertManyRecipientReaderCriteria
        {
            TicketId = ticketId,
            GroupIds = recipientGroupIds,
            SectionIds = recipientSectionIds,
            IndividualIds = recipientIndividualIds,
            CreatorId = creatorId
        };

        await repository.InsertManyRecipientReader(criteria);

        return await Task.FromResult(true);
    }

    private static string BuildRecipientIdsByType(IEnumerable<InsertOrRemoveManyRecipientModel> recipients, RecipientType recipientType)
    {
        var recipientsResult = recipients
            .FirstOrDefault(r => r.RecipientType == (int)recipientType);
        return recipientsResult != null
            ? string.Join(",", recipientsResult.RecipientIds)
            : string.Empty;
    }
}