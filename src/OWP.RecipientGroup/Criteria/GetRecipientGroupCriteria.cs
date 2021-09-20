namespace OWP.RecipientGroup.Criteria
{
    using Fanex.Data.Repository;

    public class GetRecipientGroupCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "OWP_RecipientGroup_GetRecipientGroup";

        public override bool IsValid() => true;
    }
}
