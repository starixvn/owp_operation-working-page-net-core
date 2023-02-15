namespace OWP.Maxbet.Criteria;

using Fanex.Data.Repository;

public class SpecialProductTraderXMinsGetCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetTraderXMins";

    public override bool IsValid() => OwpMatchId > 0;
}