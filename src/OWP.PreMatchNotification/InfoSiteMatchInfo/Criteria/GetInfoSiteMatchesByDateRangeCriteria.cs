namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetInfoSiteMatchesByDateRangeCriteria : CriteriaBase
    {
        public DateTime EventFromDateTime { get; set; }

        public DateTime EventToDateTime { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchNotification_QueryInfoSiteMatchesByDate";

        public override bool IsValid() => true;
    }
}
