using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetUserIdsByTicketIdCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenterEvent_GetUserIdsByTicketId";

    public override bool IsValid() => TicketId > 0;
}