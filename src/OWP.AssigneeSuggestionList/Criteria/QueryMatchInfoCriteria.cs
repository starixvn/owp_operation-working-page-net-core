using Fanex.Data.Repository;

namespace OWP.AssigneeSuggestionList.Criteria;

public class QueryMatchInfoCriteria : CriteriaBase
{
    public int OwpMatchId { get; set; }

    public override string GetSettingKey() => "OWP_AssigneeSuggestionList_GetMatchInfo";

    public override bool IsValid() => true;
}