using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.EventPicking;

public class RemoveSpecialProductCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string Id { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_RemoveSpecialProduct";

    public override bool IsValid() => ActorId > 0;
}