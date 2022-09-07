using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria
{
    public class GetMaxbetMainMatchCriteria : CriteriaBase
    {
        public int OWPMatchId { get; set; }

        public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetMaxbetMainMatchByMinorMatchId";

        public override bool IsValid() => OWPMatchId > 0;
    }
}