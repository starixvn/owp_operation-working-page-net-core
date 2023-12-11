using OWP.Extensions;
using OWP.Constants;
using OWP.UserManagement.Models;
using OWP.Services;

namespace OWP.UserManagement.Services;

public interface ModelService
{
    UserManagementPermission BuildPermissionViewModel();

    List<UserColumnModel> BuildUserColumnModel(UserManagementPermission permission, string section);
}

public class ModelServiceImp : ModelService
{
    private readonly PermissionService permissionService;

    public ModelServiceImp(PermissionService permissionService)
    {
        this.permissionService = permissionService;
    }

    public UserManagementPermission BuildPermissionViewModel()
    {
        var userManagementPolicies = permissionService.GetPermissions(FunctionKeys.UserManagement);
        var userManagementModelPolicies = permissionService.GetPermissionModel(FunctionKeys.UserManagement);

        return new UserManagementPermission
        {
            UserManagementOwnSection = userManagementPolicies.GetPermissionModel(FunctionKeys.UserManagement_OwnSection),
            UserManagementAllSections = userManagementPolicies.GetPermissionModel(FunctionKeys.UserManagement_AllSections),
            UserManagement = userManagementModelPolicies
        };
    }

    public List<UserColumnModel> BuildUserColumnModel(UserManagementPermission permission, string section)
    {
        var listColumns = new List<UserColumnModel>
        {
            new UserColumnModel(permission, section)
                { HeaderText="Employee ID", Name="EmployeeId", Width=80},
            new UserColumnModel(permission, section)
                { HeaderText="Account Name", Name="UserName", Width=150},
            new UserColumnModel(permission, section)
                { HeaderText="Full Name", Name="FullName", Width=200},
            new UserColumnModel(permission, section)
                { HeaderText="Nick Name", Name="NickName", Width=130},
            new UserColumnModel(permission, section)
                { HeaderText= "Department", Width= 120, Name= "DepartmentName"},
            new UserColumnModel(permission, section)
                { HeaderText= "Section", Width= 120, Name= "SectionName"},
            new UserColumnModel(permission, section)
                { HeaderText= "Group", Width= 150, Name= "GroupName" },
            new UserColumnModel(permission, section)
                { HeaderText= "Role", Width= 120, Name= "RoleName" },
            new UserColumnModel(permission, section)
                { HeaderText= "Extension", Width= 70, Name= "Extension" },
            new UserColumnModel(permission, section)
                { HeaderText= "Phone", Width= 120, Name= "Phone" },
            new UserColumnModel(permission, section)
                { HeaderText= "Email", Width= 250, Name= "Email" },
            new UserColumnModel(permission, section)
                { HeaderText= "Skype", Width= 120, Name= "Skype" },
            new UserColumnModel(permission, section)
                { HeaderText= "Category", Width= 120, Name= "CategoryName" },
            new UserColumnModel(permission, section)
                { HeaderText= "Location", Width= 90, Name= "LocationCode" },
            new UserColumnModel(permission, section)
                { HeaderText= "Level", Width= 80, Name= "LevelName" },
            new UserColumnModel(permission, section)
                { HeaderText= "Pickup", Width= 80, Name= "Pickup" },
            new UserColumnModel(permission, section)
                { HeaderText= "Address", Width= 200, Name= "Address" },
            new UserColumnModel(permission, section)
                { HeaderText= "Second Account", Width= 100, Name= "IsSecondAccount" },
            new UserColumnModel(permission, section)
                { HeaderText= "Language", Width= 120, Name= "Languages" }
        };

        return listColumns;
    }
}