namespace OWP.PreMatchNotification.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetInfoSiteMatchesByDateRangeCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "OWP_InfoSiteMatch_GetInfoSiteMatchesByDateRange";

        public override bool IsValid() => true;
    }
}
