namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class UpdateTicketSubTypeCriteria : NonQueryCommand
{
    public int TicketId { get; set; }
    public int TicketSubTypeId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_UpdateTicketSubType";

    public override bool IsValid() => TicketId > 0;
}