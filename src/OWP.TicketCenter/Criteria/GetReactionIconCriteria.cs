using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;
public class GetReactionIconCriteria: CriteriaBase
{
    public string MessageIds { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetReactionIcon";

    public override bool IsValid() => true;
}
