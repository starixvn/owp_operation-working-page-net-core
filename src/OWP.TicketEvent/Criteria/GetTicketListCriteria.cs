using Fanex.Data.Repository;

namespace OWP.TicketEvent.Criteria;

public class GetTicketListCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketEvent_GetTicketByOwpMatchId";

    public override bool IsValid() => OwpMatchId > 0;

}