using Fanex.Data.Repository;

namespace OWP.LiveDetail.MatchNote;

public class QueryCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override bool IsValid() => OWPMatchId > 0;

    public override string GetSettingKey() => "OWP_MatchNote_GetByMatchId";
}