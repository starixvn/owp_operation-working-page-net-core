namespace OWP.SimpleDataApi.Criteria.TicketCenter;

using Fanex.Data.Repository;

public class GetAllRecipientCriteria : CriteriaBase
{
    public bool IncludeGroup { get; set; }

    public bool IncludeSection { get; set; }

    public int SiteId { get; set; }

    public override string GetSettingKey() => "OWP_TicketSetting_GetAllRecipients";

    public override bool IsValid() => true;
}