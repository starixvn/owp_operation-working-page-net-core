using OWP.Models;

namespace OWP.UserManagement.Models;

public class QueryModel
{
    public string User { get; set; }

    public string RoleId { get; set; }

    public string SectionId { get; set; }

    public string DepartmentId { get; set; }

    public string CategoryId { get; set; }

    public string LocationCode { get; set; }

    public UserManagementPermission Permission { get; set; }
}