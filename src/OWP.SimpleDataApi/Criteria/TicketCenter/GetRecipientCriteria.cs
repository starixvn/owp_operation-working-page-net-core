namespace OWP.SimpleDataApi.Criteria.TicketCenter;

public class GetRecipientCriteria : CriteriaBase
{
    public string Keyword { get; set; }

    public bool IncludeGroup { get; set; }

    public bool IncludeSection { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetRecipient";

    public override bool IsValid() => true;
}