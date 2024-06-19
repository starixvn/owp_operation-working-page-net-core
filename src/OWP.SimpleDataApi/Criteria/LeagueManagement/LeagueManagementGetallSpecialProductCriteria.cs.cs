namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

public class LeagueManagementGetAllSpecialProductCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_SpecialProduct_Get";

    public override bool IsValid() => true;
}