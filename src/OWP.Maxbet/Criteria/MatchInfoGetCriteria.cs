using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria;

public class MatchInfoGetCriteria : CriteriaBase
{
    public int AdminsiteId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetMatchInfoByAdminsiteId";

    public override bool IsValid() => true;
}