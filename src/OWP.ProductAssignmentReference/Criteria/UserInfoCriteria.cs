using Fanex.Data.Repository;

namespace OWP.ProductAssignmentReference.Criteria
{
    public class UserInfoCriteria : CriteriaBase
    {
        public string Section { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public override string GetSettingKey()
            => "OWP_ProductAssignment_GetUserBy_Name_Role_Section";

        public override bool IsValid()
        => true;
    }
}
