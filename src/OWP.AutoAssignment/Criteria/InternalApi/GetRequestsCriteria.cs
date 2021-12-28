using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.InternalApi
{
    public class GetRequestsCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignmentRequest_GetAll";
    }
}