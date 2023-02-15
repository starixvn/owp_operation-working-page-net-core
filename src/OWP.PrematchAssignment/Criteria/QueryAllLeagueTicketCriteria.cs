namespace OWP.PreMatchAssignment.Criteria;

using Fanex.Data.Repository;

public class QueryAllLeaguesTicketCriteria : CriteriaBase
{
    public string OWPLeagueIds { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_PrematchAssignment_GetAllLeaguesTicket";

    public override bool IsValid() => ActorId > 0;
}