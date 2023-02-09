namespace OWP.TicketEvent.Criteria;

using Fanex.Data.Repository;

public class QueryAssigneesLgaCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_TicketEvent_GetAssigneesByOwpMatchId";

    public override bool IsValid() => OwpMatchId > 0;
}