using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class AddSpecialProductCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string ParentId { get; set; }

    public string MinorBettypeId { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_AddSpecialProduct";

    public override bool IsValid() => ActorId > 0;
}