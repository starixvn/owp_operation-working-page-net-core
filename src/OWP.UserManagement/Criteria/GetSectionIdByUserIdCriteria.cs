namespace OWP.UserManagement.Criteria
{
    using Fanex.Data.Repository;
    public class GetSectionIdByUserIdCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_User_GetSectionIdByUserId";
    }
}