namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetAllTicketSubTypeCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "OWP_TicketCenter_GetAllTicketSubType";

    public override bool IsValid()
        => true;
}