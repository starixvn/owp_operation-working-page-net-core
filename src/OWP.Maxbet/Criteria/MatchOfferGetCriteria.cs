using Fanex.Data.Repository;

namespace OWP.Maxbet.Criteria;

public class MatchOfferGetCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetMatchOffer";

    public override bool IsValid() => OWPMatchId > 0;
}