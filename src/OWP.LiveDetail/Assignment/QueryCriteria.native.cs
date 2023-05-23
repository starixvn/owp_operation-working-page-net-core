using Fanex.Data.Repository;

namespace OWP.LiveDetail.Assignment;

public class QueryCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override bool IsValid() => OWPMatchId > 0;

    public override string GetSettingKey() => "OWP_Assignment_GetByMatchId";
}