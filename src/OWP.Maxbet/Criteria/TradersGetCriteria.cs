namespace OWP.Maxbet.Criteria;

using Fanex.Data.Repository;

public class TradersGetCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetTrader";

    public override bool IsValid() => true;
}