using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.InternalApi
{
    public class GetAutoAssignmentResultCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignment_GetInfoSiteMatchResultByDate";
    }
}