using Fanex.Data.Repository;

namespace OWP.LiveDetail.MatchInfo;

public class ValidateInfoSiteMatchIdCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    public int InfoSiteMatchId { get; set; }

    public override string GetSettingKey() => "OWP_LiveDetail_ValidateInfoSiteMatchId";

    public override bool IsValid() => MatchId > 0;
}