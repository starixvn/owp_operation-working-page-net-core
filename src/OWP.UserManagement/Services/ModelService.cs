using OWP.Extensions;
using OWP.Constants;
using OWP.UserManagement.Models;
using OWP.Services;

namespace OWP.UserManagement.Services;

public interface ModelService
{
    List<UserColumnModel> BuildUserColumnModel();

    UserManagementPermission BuildPermissionViewModel();
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

    public List<UserColumnModel> BuildUserColumnModel() => new List<UserColumnModel>
        {
             new()
                 { HeaderText="Employee ID", Name="EmployeeId"},
             new()
                 { HeaderText="Account Name", Name="UserName"},
             new()
                 { HeaderText="Full Name", Name="FullName"},
             new()
                 { HeaderText="Nick Name", Name="NickName"},
             new()
                 { HeaderText= "Department", Name= "DepartmentName"},
             new()
                 { HeaderText= "Section", Name= "SectionName"},
             new()
                 { HeaderText= "Group", Name= "GroupName" },
             new()
                 { HeaderText= "Role", Name= "RoleName" },
             new()
                 { HeaderText= "Extension", Name= "Extension" },
             new()
                 { HeaderText= "Phone", Name= "Phone" },
             new()
                 { HeaderText= "Email", Name= "Email" },
             new()
                 { HeaderText= "Skype", Name= "Skype" },
             new()
                 { HeaderText= "Category", Name= "CategoryName" },
             new()
                 { HeaderText= "Location", Name= "LocationCode" },
             new()
                 { HeaderText= "Level", Name= "LevelName" },
             new()
                 { HeaderText= "Pickup", Name= "Pickup" },
             new()
                 { HeaderText= "Address", Name= "Address" },
             new()
                 { HeaderText= "Second Account", Name= "IsSecondAccount" },
             new()
                 { HeaderText= "Language", Name= "Languages" }
        };
}