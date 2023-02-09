using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria;

public class SpecialProductSpreadXMinsGetCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetSpreadXMins";

    public override bool IsValid() => OwpMatchId > 0;
}