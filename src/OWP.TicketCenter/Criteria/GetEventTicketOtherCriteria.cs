using OWP.TicketCenter.Const;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketOtherCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.OTHER;

    public override string GetSettingKey() => "OWP_TicketCenterOther_Get";
}