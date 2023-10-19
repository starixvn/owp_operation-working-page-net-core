
namespace OWP.TicketCenter.Criteria;

public class GetEventTicketLeagueCriteria : GetEventTicketCriteriaBase
{
    public int EventType => (int)Enum.EventType.League;

    public override string GetSettingKey() => "OWP_TicketCenterLeague_Get";
}