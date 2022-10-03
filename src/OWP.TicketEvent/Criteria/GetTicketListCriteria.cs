using Fanex.Data.Repository;

namespace OWP.TicketEvent.Criteria;

public class GetTicketListCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override string GetSettingKey() => "OWP_TicketEvent_GetTicketByOwpMatchId";

    public override bool IsValid() => OWPMatchId > 0;

}