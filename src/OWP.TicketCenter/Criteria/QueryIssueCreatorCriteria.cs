namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class QueryIssueCreatorCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCreatorDetail_Get";

    public override bool IsValid() => TicketId > 0;
}