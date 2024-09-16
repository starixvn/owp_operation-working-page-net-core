using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;
public class ConsolidationGetSubtypeCriteria: CriteriaBase
{
    public int ActorId { get; set; }
    public override string GetSettingKey() => "OWP_Consolidation_GetSubtype";

    public override bool IsValid() => true;
}
