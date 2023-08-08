using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketMatchCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.Match;

    public override string GetSettingKey() => "OWP_TicketCenterEvent_Get";
}