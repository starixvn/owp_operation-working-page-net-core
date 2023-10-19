namespace OWP.TicketCenter.Criteria;

public class GetEventTicketLiveIncidentCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)Enum.EventType.LiveIncident;

    public override string GetSettingKey() => "OWP_TicketCenterLiveIncident_Get";
}