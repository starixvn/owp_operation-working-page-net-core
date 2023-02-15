namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class RemoveManyRecipientReaderCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public string GroupIds { get; set; }

    public string IndividualIds { get; set; }

    public string SectionIds { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_DeleteManyRecipientReader";

    public override bool IsValid() => TicketId > 0;
}