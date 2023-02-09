namespace OWP.Maxbet.Criteria;

using Fanex.Data.Repository;

public class SpreadGetCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetSpread";

    public override bool IsValid() => true;
}