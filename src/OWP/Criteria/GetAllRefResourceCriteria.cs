using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetAllRefResourceCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_RefResourceGetAll";
}