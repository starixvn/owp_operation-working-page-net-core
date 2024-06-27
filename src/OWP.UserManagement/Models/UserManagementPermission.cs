namespace OWP.UserManagement.Models;

using OWP.Models;

public class UserManagementPermission
{
    public PermissionModel UserManagement { get; set; } = new();

    public PermissionModel UserManagementOwnSection { get; set; } = new();

    public PermissionModel UserManagementAllSections { get; set; } = new();

}