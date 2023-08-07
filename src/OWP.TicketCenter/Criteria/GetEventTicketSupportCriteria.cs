using OWP.TicketCenter.Const;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketSupportCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.SUPPORT;

    public override string GetSettingKey() => "OWP_TicketCenterSupport_Get";
}