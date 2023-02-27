using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetAllTicketSubTypeCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "OWP_TicketCenter_GetAllTicketSubType";

    public override bool IsValid()
        => true;
}