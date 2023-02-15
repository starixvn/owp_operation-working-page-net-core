namespace OWP.LiveDetail.MatchInfo;

using Fanex.Data.Repository;

public class QueryCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override bool IsValid() => OWPMatchId > 0;

    public override string GetSettingKey() => "OWP_LiveDetail_MatchInfo_Get";
}