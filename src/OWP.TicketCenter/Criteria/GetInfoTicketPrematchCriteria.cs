using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetInfoTicketPrematchCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public bool IsTicketLeague { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetInfoTicketForPrematch";

    public override bool IsValid() => TicketId > 0;
}