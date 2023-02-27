using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetTicketIdHaveSavedMessageCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_TicketCenter_GetTicketIsSaved";
    public override bool IsValid() => true;
}