namespace OWP.UserManagement.UnitTests.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.NAP.Client.Helpers.Web;
using Models;
using NSubstitute;
using OWP.Models;
using UserManagement.Services;
using Xunit;
using Xunit.Asserts.Compare;

public class UserServiceUnitTests
{
    private readonly ModelService modelService;
    private readonly Repository repository;
    private readonly UsersService usersService;
  
    public UserServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
        modelService = Substitute.For<ModelService>();
        
        var currentSession = Substitute.For<INapSession>();
        currentSession.UserId = 99;
        usersService = new UsersServiceImp(modelService, repository, currentSession);
    }

    [Fact]
    public async Task GetUserPagingAsync_WithUserManagementPermission_WhenCalled_ReturnFullPermission()
    {
        // Arrange
        var userPermission = new UserManagementPermission
        {
            UserManagement = new PermissionModel(true, true),
            UserManagementAllSections = new PermissionModel(true, true),
            UserManagementOwnSection = new PermissionModel(true, true),
        };
        var queryModel = new QueryModel
        {
            CategoryId = "1",
            User = "",
            DepartmentId = "1",
            LocationCode = "SMS",
            RoleId = "1",
            SectionId = "1"
        };

        var listUser = new List<User>
        {
            new (){UserId=1, UserName="sa"},
            new (){UserId=2, UserName="starix15" }
        };

        // Act
        var expected = listUser;

        var userColumnModels = new List<UserColumnModel>
        {
            new () { HeaderText="Employee ID", Name="EmployeeId"},
            new () {HeaderText="Account Name", Name="AccountName"},
            new () { HeaderText="Full Name", Name="FullName"},
            new () {HeaderText="Nick Name", Name="NickName"},
            new () { HeaderText= "Department", Name= "Department"},
            new () { HeaderText= "Section" },
            new () { HeaderText= "Group" },
            new () { HeaderText= "Role" },
            new () { HeaderText= "Extension" },
            new () { HeaderText= "Phone" },
            new () { HeaderText= "Email" },
            new () { HeaderText= "Skype" },
            new () { HeaderText= "Category", Name= "Category" },
            new () { HeaderText= "Location",Name= "Location"},
            new () { HeaderText= "Level", Name= "Level" },
            new () { HeaderText= "Pickup", Name= "Pickup" },
            new () { HeaderText= "Address", Name= "Address" },
            new () { HeaderText= "Second Account", Name= "IsSecondAccount" }

        };

        modelService.BuildPermissionViewModel().Returns(userPermission);
        modelService.BuildUserColumnModel().Returns(userColumnModels);

        repository.GetUserPagingAsync(Arg.Compat.Any<QueryModel>()).Returns(expected);

        var actual = await usersService.GetUserPagingAsync(queryModel);

        // Assert
        DeepAssert.Equal(actual, expected);
    }

    [Fact]
    public async Task GetUserPagingAsync_WithUserManagementPermission_WhenCalled_ReturnPermissionOwnSection()
    {
        // Act
        var userPermission = new UserManagementPermission
        {
            UserManagement = new PermissionModel(true, true),
            UserManagementAllSections = new PermissionModel(false, false),
            UserManagementOwnSection = new PermissionModel(true, true)
        };
        var queryModel = new QueryModel
        {
            CategoryId = "1",
            User = "",
            DepartmentId = "1",
            LocationCode = "SMS",
            RoleId = "1",
            SectionId = "1"
        };

        var listUser = new List<User>
        {
            new User{UserId=2, UserName="starix15", SectionId=null }
        };
        // Arrange
        var expected = listUser;

        var userColumnModels = new List<UserColumnModel>
        {
            new () { HeaderText="Employee ID", Name="EmployeeId"},
            new () {HeaderText="Account Name", Name="AccountName"},
            new () { HeaderText="Full Name", Name="FullName"},
            new () {HeaderText="Nick Name", Name="NickName"},
            new () { HeaderText= "Department", Name= "Department"},
            new () { HeaderText= "Section" },
            new () { HeaderText= "Group" },
            new () { HeaderText= "Role" },
            new () { HeaderText= "Extension" },
            new () { HeaderText= "Phone" },
            new () { HeaderText= "Email" },
            new () { HeaderText= "Skype" },
            new () { HeaderText= "Category", Name= "Category" },
            new () { HeaderText= "Location",Name= "Location"},
            new () { HeaderText= "Level", Name= "Level" },
            new () { HeaderText= "Pickup", Name= "Pickup" },
            new () { HeaderText= "Address", Name= "Address" },
            new () { HeaderText= "Second Account", Name= "IsSecondAccount" }

        };

        modelService.BuildPermissionViewModel().Returns(userPermission);
        modelService.BuildUserColumnModel().Returns(userColumnModels);

        repository.GetUserPagingAsync(Arg.Compat.Any<QueryModel>()).Returns(expected);

        var actual = await usersService.GetUserPagingAsync(queryModel);

        // Assert
        DeepAssert.Equal(actual, expected);
    }

    [Fact]
    public async Task GetUserPagingAsync_WithUserManagementPermission_WhenCalled_ReturnPermissionAllSections()
    {
        // Act
        var userPermission = new UserManagementPermission
        {
            UserManagement = new PermissionModel(true, true),
            UserManagementOwnSection = new PermissionModel(true, true),
            UserManagementAllSections = new PermissionModel(true, true)
        };
        var queryModel = new QueryModel
        {
            CategoryId = "1",
            User = "",
            DepartmentId = "1",
            LocationCode = "SMS",
            RoleId = "1",
            SectionId = "1"
        };

        var listUser = new List<User>
        {
            new User{UserId=1, UserName="sa", DepartmentId=1 },
            new User{UserId=2, UserName="starix15", SectionId=1 }
        };
        // Arrange
        var expected = listUser;

        var userColumnModels = new List<UserColumnModel>
        {
            new () { HeaderText="Employee ID", Name="EmployeeId"},
            new () {HeaderText="Account Name", Name="AccountName"},
            new () { HeaderText="Full Name", Name="FullName"},
            new () {HeaderText="Nick Name", Name="NickName"},
            new () { HeaderText= "Department", Name= "Department"},
            new () { HeaderText= "Section" },
            new () { HeaderText= "Group" },
            new () { HeaderText= "Role" },
            new () { HeaderText= "Extension" },
            new () { HeaderText= "Phone" },
            new () { HeaderText= "Email" },
            new () { HeaderText= "Skype" },
            new () { HeaderText= "Category", Name= "Category" },
            new () { HeaderText= "Location",Name= "Location"},
            new () { HeaderText= "Level", Name= "Level" },
            new () { HeaderText= "Pickup", Name= "Pickup" },
            new () { HeaderText= "Address", Name= "Address" },
            new () { HeaderText= "Second Account", Name= "IsSecondAccount" }
        };

        modelService.BuildPermissionViewModel().Returns(userPermission);
        modelService.BuildUserColumnModel().Returns(userColumnModels);

        repository.GetUserPagingAsync(Arg.Compat.Any<QueryModel>()).Returns(expected);

        var actual = await usersService.GetUserPagingAsync(queryModel);

        // Assert
        DeepAssert.Equal(actual, expected);
    }
}