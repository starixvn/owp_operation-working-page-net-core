namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;
public class GetTicketIdHaveSavedMessageCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_TicketCenter_GetTicketIsSaved";
    public override bool IsValid() => true;
}