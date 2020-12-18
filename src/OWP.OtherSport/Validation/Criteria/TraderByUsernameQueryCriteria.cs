namespace OWP.OtherSport.Validation.Criteria
{
    using Fanex.Data.Repository;

    public class TraderByUsernameQueryCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_TraderByUsernameSel";
    }
}
