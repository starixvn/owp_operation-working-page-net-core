using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria
{
    public class SpecialProductIdCriteria : CriteriaBase
    {
        public int AdminsiteId { get; set; }

        public int BettypeId { get; set; }

        public override string GetSettingKey()
            => "OWP_API_SyncMaxbet_GetMatchIdByAdminsiteIdAndBettypeId";

        public override bool IsValid()
        => true;
    }
}