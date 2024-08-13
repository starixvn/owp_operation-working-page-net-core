using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;
public class SpecialProductUpdateMaxbetSpreadCriteria : NonQueryCommand
{
    public string TargetJson { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_SpecialProduct_UpdateMaxbetSpread";

    public override bool IsValid() => true;
}

