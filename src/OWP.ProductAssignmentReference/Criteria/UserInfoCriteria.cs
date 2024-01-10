using Fanex.Data.Repository;

namespace OWP.ProductAssignmentReference.Criteria;

public class UserInfoCriteria : CriteriaBase
{
    public string SectionId { get; set; }

    public string LocationId { get; set; }

    public string User { get; set; }

    public string RoleId { get; set; }

    public int SiteId { get; set; }

    public override string GetSettingKey() => "OWP_ProductAssignment_GetUserBy_User_Role_Section_Location";

    public override bool IsValid() => true;
}