using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetStatusHistoryCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetTicketStatusHistory";

    public override bool IsValid() => TicketId > 0;
}