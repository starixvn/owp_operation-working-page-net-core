using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

public class LeagueManagementGetTVListCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_LeagueManagement_GetAllTVCondition";

    public override bool IsValid() => true;
}