using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.PublicApi
{
    internal class NewRequestCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignment_GetRequestNew";
    }
}