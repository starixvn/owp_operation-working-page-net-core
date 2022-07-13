namespace OWP.LiveDetail.User
{
    using Fanex.Data.Repository;

    public class GetInfoByNameCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Name);

        public override string GetSettingKey() => "OWP_UserGetByName";
    }
}