namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetUserIdsByTicketIdCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenterEvent_GetUserIdsByTicketId";

    public override bool IsValid()
        => TicketId > 0;
}