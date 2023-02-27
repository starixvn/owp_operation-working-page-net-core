using Fanex.Data.Repository;

namespace OWP.LiveDetail.MatchInfo;

public class GetMatchStatusByMatchIdCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override bool IsValid() => OWPMatchId > 0;

    public override string GetSettingKey() => "OWP_LiveDetail_MatchStatus_GetByMatchId";
}