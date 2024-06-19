using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class QueryUpdateTypeCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_EventPicking_GetUpdateType";

    public override bool IsValid() => true;
}
