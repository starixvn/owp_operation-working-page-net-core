using Fanex.Data.Repository;

namespace OWP.AssigneeSuggestionList.Criteria;

public class QueryTraderInfoCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_AssigneeSuggestionList_GetTraderByDate";

    public override bool IsValid() => true;
}
