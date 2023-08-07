using OWP.TicketCenter.Const;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketLogWorkCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.LOG_WORK;

    public override string GetSettingKey() => "OWP_TicketCenterLogWork_Get";
}