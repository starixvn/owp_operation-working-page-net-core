
using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Criteria;

internal class GetEventTicketLiveIncidentCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)Enum.EventType.LiveIncident;

    public override string GetSettingKey() => "OWP_TicketCenterLiveIncident_Get";
}