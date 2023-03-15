using Fanex.Data.Repository;

namespace OWP.ResourceManagement.Criteria;

public class InsertCriteria : NonQueryCommand
{
    public int ResourceId { get; set; }

    public string RefName { get; set; }

    public string Value { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(Value) && !string.IsNullOrEmpty(RefName);

    public override string GetSettingKey() => "OWP_RefResource_Insert";
}