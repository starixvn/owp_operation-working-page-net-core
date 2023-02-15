namespace OWP.SimpleDataApi.Criteria.LiveDetail;

using Fanex.Data.Repository;

public class GetMatchSettingByMatchIdCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_LiveDetail_MatchSetting_GetById";

    public int MatchId { get; set; }

    public override bool IsValid() => true;
}