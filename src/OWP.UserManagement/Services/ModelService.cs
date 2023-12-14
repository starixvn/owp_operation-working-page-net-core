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

    public List<UserColumnModel> BuildUserColumnModel()
    => new List<UserColumnModel>
        {
             new UserColumnModel()
                 { HeaderText="Employee ID", Name="EmployeeId"},
             new UserColumnModel()
                 { HeaderText="Account Name", Name="UserName"},
             new UserColumnModel()
                 { HeaderText="Full Name", Name="FullName"},
             new UserColumnModel()
                 { HeaderText="Nick Name", Name="NickName"},
             new UserColumnModel()
                 { HeaderText= "Department", Name= "DepartmentName"},
             new UserColumnModel()
                 { HeaderText= "Section", Name= "SectionName"},
             new UserColumnModel()
                 { HeaderText= "Group", Name= "GroupName" },
             new UserColumnModel()
                 { HeaderText= "Role", Name= "RoleName" },
             new UserColumnModel()
                 { HeaderText= "Extension", Name= "Extension" },
             new UserColumnModel()
                 { HeaderText= "Phone", Name= "Phone" },
             new UserColumnModel()
                 { HeaderText= "Email", Name= "Email" },
             new UserColumnModel()
                 { HeaderText= "Skype", Name= "Skype" },
             new UserColumnModel()
                 { HeaderText= "Category", Name= "CategoryName" },
             new UserColumnModel()
                 { HeaderText= "Location", Name= "LocationCode" },
             new UserColumnModel()
                 { HeaderText= "Level", Name= "LevelName" },
             new UserColumnModel()
                 { HeaderText= "Pickup", Name= "Pickup" },
             new UserColumnModel()
                 { HeaderText= "Address", Name= "Address" },
             new UserColumnModel()
                 { HeaderText= "Second Account", Name= "IsSecondAccount" },
             new UserColumnModel()
                 { HeaderText= "Language", Name= "Languages" }
        };
}