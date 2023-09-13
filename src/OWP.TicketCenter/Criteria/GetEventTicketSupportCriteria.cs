
using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketSupportCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)Enum.EventType.Support;

    public override string GetSettingKey() => "OWP_TicketCenterSupport_Get";
}