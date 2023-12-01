using OWP.Models;

namespace OWP.UserManagement.Models;

public class QueryModel
{
    public string User { get; set; }

    public int[] RoleId { get; set; }

    public int[] SectionId { get; set; }

    public int[] DepartmentId { get; set; }

    public int[] CategoryId { get; set; }

    public string[] LocationCode { get; set; }

    public UserManagementPermission Permission { get; set; }
}