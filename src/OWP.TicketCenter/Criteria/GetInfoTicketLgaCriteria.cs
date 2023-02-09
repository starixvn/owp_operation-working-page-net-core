using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetInfoTicketLgaCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetInfoTicketLga";

    public override bool IsValid() => TicketId > 0;
}