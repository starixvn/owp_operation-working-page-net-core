using Fanex.Data.Repository;

namespace OWP.ResourceManagement.Criteria;
public class UpdateCriteria : NonQueryCommand
{
    public string RefName { get; set; }

    public string Value { get; set; }

    public int ResourceId { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(Value) && !string.IsNullOrEmpty(RefName) && ResourceId > 0;

    public override string GetSettingKey() => "OWP_RefResource_Update";
}