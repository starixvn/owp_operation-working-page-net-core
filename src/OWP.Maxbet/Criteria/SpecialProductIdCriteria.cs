namespace OWP.Maxbet.Criteria;

using Fanex.Data.Repository;

public class SpecialProductIdCriteria : CriteriaBase
{
    public int AdminsiteId { get; set; }

    public int BettypeId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetMatchIdByAdminsiteIdAndBettypeId";

    public override bool IsValid() => true;
}