
using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketLogWorkCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.LogWork;

    public override string GetSettingKey() => "OWP_TicketCenterLogWork_Get";
}