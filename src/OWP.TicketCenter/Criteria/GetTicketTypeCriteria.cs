using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetTicketTypeCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_TicketCenter_GetTicketType";

    public override bool IsValid()
        => true;
}