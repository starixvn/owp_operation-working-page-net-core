using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria
{
    public class MatchIdGetCriteria : CriteriaBase
    {
        public int AdminsiteId { get; set; }

        public override string GetSettingKey()
            => "OWP_API_SyncMaxbet_GetMatchIdByAdminsiteId";

        public override bool IsValid()
        => true;
    }
}