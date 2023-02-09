namespace OWP.PrematchAssignment.Criteria;

using Fanex.Data.Repository;

public class QueryInfoMatchByIdCriteria : CriteriaBase
{
    public int Id { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_GetInfoSiteMatchById";

    public override bool IsValid() => true;
}