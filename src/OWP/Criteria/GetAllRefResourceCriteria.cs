namespace OWP.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllRefResourceCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_RefResourceGetAll";
    }
}