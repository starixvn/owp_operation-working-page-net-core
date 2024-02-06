using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class QueryAllMatchesTicketCriteria : CriteriaBase
{
    public string InfoSiteMatchIds { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_PrematchAssignment_GetAllMatchesTicket";

    public override bool IsValid() => ActorId > 0;
}