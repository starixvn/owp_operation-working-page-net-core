namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetRecipientByTicketIdCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetRecipientByTicketId";

    public override bool IsValid() => TicketId > 0;
}