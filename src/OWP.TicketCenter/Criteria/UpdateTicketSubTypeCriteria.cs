using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class UpdateTicketSubTypeCriteria : NonQueryCommand
{
    public int TicketId { get; set; }
    public int TicketSubTypeId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_UpdateTicketSubType";

    public override bool IsValid() => TicketId > 0;
}