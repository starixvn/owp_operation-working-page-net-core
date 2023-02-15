namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetExistedTicketIdCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetExistedTicketId";

    public override bool IsValid() => TicketId > 0;
}