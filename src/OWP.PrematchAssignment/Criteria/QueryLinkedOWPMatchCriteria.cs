namespace OWP.PreMatchAssignment.Criteria
{
    using Fanex.Data.Repository;

    public class QueryLinkedOWPMatchCriteria : CriteriaBase
    {
        public string InfositeMatchIds { get; set; }

        public DateTime EventDate { get; set; }

        public override string GetSettingKey() => "OWP_PrematchAssignment_GetLinkedOWPMatch";

        public override bool IsValid() => true;
    }
}