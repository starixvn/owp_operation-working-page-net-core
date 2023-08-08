
using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketSupportCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.Support;

    public override string GetSettingKey() => "OWP_TicketCenterSupport_Get";
}