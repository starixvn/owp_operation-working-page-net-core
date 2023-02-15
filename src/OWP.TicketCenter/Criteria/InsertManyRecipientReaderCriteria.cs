namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class InsertManyRecipientReaderCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public string GroupIds { get; set; }

    public string IndividualIds { get; set; }

    public string SectionIds { get; set; }

    public int CreatorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_AddManyRecipientReader";

    public override bool IsValid()
        => TicketId > 0 && CreatorId > 0;
}