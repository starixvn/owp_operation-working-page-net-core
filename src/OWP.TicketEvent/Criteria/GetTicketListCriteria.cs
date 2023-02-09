namespace OWP.TicketEvent.Criteria;

using Fanex.Data.Repository;

public class GetTicketListCriteria : CriteriaBase
{
    public string OwpMatchIds { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketEvent_GetTicketByOwpMatchId";

    public override bool IsValid() => true;

}