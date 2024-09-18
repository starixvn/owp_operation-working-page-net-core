using Fanex.Data.Repository;

namespace OWP.Api.Maxbet.Criteria;

public class SpecialProductTraderXMinsGetCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetTraderXMins";

    public override bool IsValid() => OwpMatchId > 0;
}