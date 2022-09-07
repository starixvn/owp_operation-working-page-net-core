using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria;

public class GetSecondAcceptValueDefaultCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public decimal MaxbetHDP { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetSecondAcceptValueDefault";

    public override bool IsValid() => true;
}