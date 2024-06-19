namespace OWP.SimpleDataApi.Criteria.TicketCenter;

public class GetDefaultRecipientCriteria : CriteriaBase
{
    public int SubTypeId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetDefaultRecipientBySubTypeId";

    public override bool IsValid() => true;
}