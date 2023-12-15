namespace OWP.UserManagement.Criteria;

using Fanex.Data.Repository;

public class QueryUsersCriteria : CriteriaBase
{
    public string User { get; set; }

    public string RoleId { get; set; }

    public string SectionId { get; set; }

    public string DepartmentId { get; set; }

    public string CategoryId { get; set; }

    public string LocationCode { get; set; }

    public int SiteId { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_UserListGet_New";
}