namespace OWP.UserManagement.Models
{
    using OWP.Models;

    public class UserManagementPermission
    {
        public PermissionModel UserManagement { get; set; } = new PermissionModel();

        public PermissionModel UserManagementOwnSection { get; set; } = new PermissionModel();

        public PermissionModel UserManagementAllSections { get; set; } = new PermissionModel();

    }
}