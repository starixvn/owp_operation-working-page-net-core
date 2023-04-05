using Fanex.Data.Repository;

namespace OWP.AssigneeSuggestionList.Criteria;

public class QueryAssignmentCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }
    public override string GetSettingKey() => "OWP_AssigneeSuggestionList_GetAssignmentByDate";

    public override bool IsValid() => true;
}