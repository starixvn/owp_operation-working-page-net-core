using Fanex.Data.Repository;

namespace OWP.AssigneeSuggestionList.Criteria;

public class QueryParentMatchInfoCriteria : CriteriaBase
{
    public string OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_TodaySuggestionList_GetParentMatchInfo";

    public override bool IsValid() => true;
}
