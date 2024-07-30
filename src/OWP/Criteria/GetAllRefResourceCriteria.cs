using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetAllRefResourceCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "OWP_RefResourceGetAll";
    }
}