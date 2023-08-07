using OWP.TicketCenter.Const;

namespace OWP.TicketCenter.Criteria;

internal class GetEventTicketLiveIncidentCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.LIVE_INCIDENT;

    public override string GetSettingKey() => "OWP_TicketCenterLiveIncident_Get";
}