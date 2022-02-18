using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.PublicApi
{
    public class GetTradersByDateCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public int SiteId { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignment_GetTradersByDate";
    }
}