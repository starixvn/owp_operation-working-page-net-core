
using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketOtherCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)Enum.EventType.Other;

    public override string GetSettingKey() => "OWP_TicketCenterOther_Get";
}