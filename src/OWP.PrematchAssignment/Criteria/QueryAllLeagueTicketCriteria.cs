using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class QueryAllLeaguesTicketCriteria : CriteriaBase
{
    public string OWPLeagueIds { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_PrematchAssignment_GetAllLeaguesTicket";

    public override bool IsValid() => ActorId > 0;
}