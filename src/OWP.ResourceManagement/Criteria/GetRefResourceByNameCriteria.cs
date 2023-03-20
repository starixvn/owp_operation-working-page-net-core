using Fanex.Data.Repository;

namespace OWP.ResourceManagement.Criteria;

public class GetRefResourceByNameCriteria : CriteriaBase
{
    public string RefName { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(RefName);

    public override string GetSettingKey() => "OWP_RefResourceGetByName";
}