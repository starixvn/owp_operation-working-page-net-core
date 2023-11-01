
namespace OWP.TicketCenter.Criteria;

public class GetEventTicketLogWorkCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)Enum.EventType.LogWork;

    public override string GetSettingKey() => "OWP_TicketCenterLogWork_Get";
}