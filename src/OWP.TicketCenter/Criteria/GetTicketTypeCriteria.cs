namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetTicketTypeCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_TicketCenter_GetTicketType";

    public override bool IsValid()
        => true;
}