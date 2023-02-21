using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class InsertIssueCreatorCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public int CreatorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketIssueCreator_Create";

    public override bool IsValid() => TicketId > 0 && CreatorId > 0;
}