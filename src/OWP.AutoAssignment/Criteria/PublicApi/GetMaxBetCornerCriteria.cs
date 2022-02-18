using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.PublicApi
{
    public class GetMaxBetCornerCriteria : CriteriaBase
    {
        public string Xml { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignment_GetMaxBetCorner";
    }
}