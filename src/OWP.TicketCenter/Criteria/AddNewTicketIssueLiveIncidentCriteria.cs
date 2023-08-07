using Fanex.Data.Repository;
using OWP.TicketCenter.Const;

namespace OWP.TicketCenter.Criteria;

public class AddNewTicketIssueLiveIncidentCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public int OwpMatchId { get; set; }
    public int EventType => (int)EventTypeEnum.LIVE_INCIDENT;


    public override string GetSettingKey() => "OWP_TicketIssueLiveIncident_Create";

    public override bool IsValid()
        => TicketId > 0 && OwpMatchId > 0;
}