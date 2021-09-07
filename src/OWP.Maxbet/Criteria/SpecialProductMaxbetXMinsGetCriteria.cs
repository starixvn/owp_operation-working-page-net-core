using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria
{
    public class SpecialProductMaxbetXMinsGetCriteria : CriteriaBase
    {
        public int OwpMatchId { get; set; }

        public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetMaxbetXMins";

        public override bool IsValid() => OwpMatchId > 0;
    }
}
