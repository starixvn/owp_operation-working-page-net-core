namespace OWP.EventPicking.Criteria;

using Fanex.Data.Repository;

public class QueryChildMatchCriteria : CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetInfoSiteChildMatch";

    public override bool IsValid() => true;
}
