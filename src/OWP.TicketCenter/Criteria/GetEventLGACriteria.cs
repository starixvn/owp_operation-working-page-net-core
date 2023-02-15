namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetEventLGACriteria : CriteriaBase
{
    public string Keyword { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_NewTicket_GetEventForLiveGameAsssignment";

    public override bool IsValid() => Keyword.Length > 1;
}