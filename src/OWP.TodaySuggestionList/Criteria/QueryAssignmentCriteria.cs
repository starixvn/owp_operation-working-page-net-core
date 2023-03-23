using Fanex.Data.Repository;

namespace OWP.TodaySuggestionList.Criteria
{
    public class QueryAssignmentCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }
        public override string GetSettingKey() => "OWP_TodaySuggestionList_GetAssignmentByDate";

        public override bool IsValid() => true;
    }
}