namespace OWP.PreMatchNotification.NewLeague.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetPreMatchLeaguesCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchNotification_QueryLeague";

        public override bool IsValid() => true;
    }
}