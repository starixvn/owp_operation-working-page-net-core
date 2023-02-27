using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria;

public class OddGetCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetOddByOwpMatchId";

    public override bool IsValid() => OwpMatchId > 0;
}