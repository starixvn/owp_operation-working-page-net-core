namespace OWP.ProductAssignmentReference.Criteria
{
    using Fanex.Data.Repository;

    public class GetMainBettypeBySectionIdCriteria : CriteriaBase
    {
        public int? SectionId { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_RefResource_GetMainBettypeBySectionId";
    }
}