using OWP.TicketCenter.Const;

namespace OWP.TicketCenter.Criteria;

public class GetEventTicketLeagueCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)EventTypeEnum.LEAGUE;

    public override string GetSettingKey() => "OWP_TicketCenterLeague_Get";
}