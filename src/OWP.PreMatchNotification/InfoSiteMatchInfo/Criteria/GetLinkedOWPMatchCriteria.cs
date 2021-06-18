namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Criteria
{
    using Fanex.Data.Repository;

    public class GetLinkedOWPMatchCriteria : CriteriaBase
    {
        public string InfositeMatchIds { get; set; }

        public override string GetSettingKey() => "OWP_PrematchAssignment_GetLinkedOWPMatch";

        public override bool IsValid() => true;
    }
}