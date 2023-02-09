namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class MarkAllTicketsCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_UpdateTicketsAsReadByActorId";

    public override bool IsValid() => true;
}