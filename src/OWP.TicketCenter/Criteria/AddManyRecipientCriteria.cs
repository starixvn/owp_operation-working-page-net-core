using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class AddManyRecipientCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public int RecipientType { get; set; }

    public string RecipientIds { get; set; }

    public override string GetSettingKey() => "OWP_TicketIssue_AddManyRecipient";

    public override bool IsValid()
        => TicketId > 0 && RecipientIds.Length > 0;
}