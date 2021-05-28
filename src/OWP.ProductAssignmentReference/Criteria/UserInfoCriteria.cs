using Fanex.Data.Repository;

namespace OWP.ProductAssignmentReference.Criteria
{
    public class UserInfoCriteria : CriteriaBase
    {
        public string Section { get; set; }

        public string User { get; set; }

        public int? RoleId { get; set; }

        public int SiteId { get; set; }

        public override string GetSettingKey()
            => "OWP_ProductAssignment_GetUserBy_User_Role_Section";

        public override bool IsValid()
        => true;
    }
}
