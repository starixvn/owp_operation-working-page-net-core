namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

using Fanex.Data.Repository;

public class LeagueManagementGetTVListCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_LeagueManagement_GetAllTVCondition";

    public override bool IsValid() => true;
}