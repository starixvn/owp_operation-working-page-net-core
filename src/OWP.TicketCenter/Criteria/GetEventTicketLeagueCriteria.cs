
using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketLeagueCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.League;

    public override string GetSettingKey() => "OWP_TicketCenterLeague_Get";
}