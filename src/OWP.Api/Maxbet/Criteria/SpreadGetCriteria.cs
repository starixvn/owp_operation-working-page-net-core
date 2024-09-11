using Fanex.Data.Repository;

namespace OWP.Api.Maxbet.Criteria;

public class SpreadGetCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetSpread";

    public override bool IsValid() => true;
}