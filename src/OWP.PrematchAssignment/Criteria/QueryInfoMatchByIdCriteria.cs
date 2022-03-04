using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria
{
    public class QueryInfoMatchByIdCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_GetInfoSiteMatchById";

        public override bool IsValid() => true;
    }
}