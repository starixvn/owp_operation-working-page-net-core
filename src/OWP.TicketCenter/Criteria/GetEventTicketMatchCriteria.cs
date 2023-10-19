namespace OWP.TicketCenter.Criteria;

public class GetEventTicketMatchCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)Enum.EventType.Match;

    public override string GetSettingKey() => "OWP_TicketCenterEvent_Get";
}