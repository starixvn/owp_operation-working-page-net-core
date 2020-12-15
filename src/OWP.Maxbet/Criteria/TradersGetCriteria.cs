using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria
{
    public class TradersGetCriteria : CriteriaBase
    {
        public int OwpMatchId { get; set; }

        public override string GetSettingKey()
            => "OWP_API_SyncMaxbet_GetTrader";

        public override bool IsValid()
            => true;
    }
}