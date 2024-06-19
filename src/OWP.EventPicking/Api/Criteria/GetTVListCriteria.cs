using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class GetTVListCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_LeagueManagement_GetAllTVCondition";

    public override bool IsValid() => true;
}