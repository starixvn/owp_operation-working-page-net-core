using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class RemoveSpecialProductCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string ParentId { get; set; }

    public string MinorBettypeId { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_RemoveSpecialProduct";

    public override bool IsValid() => ActorId > 0;
}