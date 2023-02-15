namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class PinTicketCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int TicketId { get; set; }

    public bool IsPinned { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_PinTicket";

    public override bool IsValid() => true;
}