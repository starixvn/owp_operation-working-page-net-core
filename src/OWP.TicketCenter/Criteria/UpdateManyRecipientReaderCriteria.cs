namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class UpdateManyRecipientReaderCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public bool IsRead { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_UpdateManyRecipientReader";

    public override bool IsValid() => TicketId > 0 && ActorId > 0;
}