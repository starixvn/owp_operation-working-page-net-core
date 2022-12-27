using Fanex.Data.Repository;

namespace OWP.Report.PreMatchReport.Criteria
{
    public class GetUserIdFromNapUserCriteria : CriteriaBase
    {
        public int SiteId { get; set; }

        public override string GetSettingKey() => "OWP_AutoAssignment_GetUserInfoFromNAPUserControl";

        public override bool IsValid() => SiteId > 0;
    }
}