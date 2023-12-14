using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Models.ValueObject;

namespace OWP.UserManagement.UnitTests.Services;

using System.Collections.Generic;
using Constants;
using Models;
using NSubstitute;
using OWP.Services;
using UserManagement.Services;
using Xunit;
using Xunit.Asserts.Compare;

public class ModelServiceUnitTests
{
    private readonly ModelService modelService;
    private readonly PermissionService permissionService;

    public ModelServiceUnitTests()
    {
        permissionService = Substitute.For<PermissionService>();
            
        modelService = new ModelServiceImp(permissionService);
    }

    [Fact]
    public void BuildPermissionViewModel_WithUserManagementPermission_WhenCalled_ReturnFullPermission()
    {
        // Arrange
        const string functionKey = FunctionKeys.UserManagement;
        var expected = new List<FunctionPolicy>
        {
            new()
            {
                Policy = (int)Constant.Policy.Host,
                Key = FunctionKeys.UserManagement
            },
            new()
            {
                Policy = (int)Constant.Policy.Host,
                Key = FunctionKeys.UserManagement_AllSections
            },
            new()
            {
                Policy = (int)Constant.Policy.Host,
                Key = FunctionKeys.UserManagement_OwnSection
            }
        };

        // Act
        permissionService.GetPermissions(functionKey).Returns(expected);

        var actual = modelService.BuildPermissionViewModel();

        // Assert
        Assert.True(actual.UserManagementAllSections.IsRead);
    }

    [Fact]
    public void BuildUserColumnModel_WithOnlyReadPermission_WhenCalled_ReturnExactly()
    {
        // Act
        var actual = modelService.BuildUserColumnModel();

        // Assert
        var expected = new List<UserColumnModel>
        {
            new() { HeaderText="Employee ID", Name="EmployeeId"},
            new (){ HeaderText="Account Name", Name="UserName"},
            new (){HeaderText = "Full Name", Name = "FullName"},
            new (){HeaderText = "Nick Name", Name = "NickName"},
            new (){ HeaderText= "Department", Name= "DepartmentName"},
            new (){ HeaderText= "Section", Name= "SectionName"},
            new (){ HeaderText= "Group", Name= "GroupName" },
            new (){ HeaderText= "Role", Name= "RoleName"},
            new (){ HeaderText= "Extension", Name= "Extension" },
            new (){ HeaderText= "Phone", Name= "Phone" },
            new (){ HeaderText= "Email", Name= "Email" },
            new (){ HeaderText= "Skype", Name= "Skype" },
            new (){ HeaderText= "Category", Name= "CategoryName" },
            new (){ HeaderText= "Location", Name= "LocationCode" },
            new (){ HeaderText= "Level", Name= "LevelName"},
            new (){ HeaderText= "Pickup", Name= "Pickup"},
            new (){ HeaderText= "Address", Name= "Address" },
            new (){ HeaderText= "Second Account", Name= "IsSecondAccount" },
            new (){ HeaderText= "Language", Name= "Languages"}
        };


        DeepAssert.Equal(actual, expected);
    }

    [Fact]
    public void BuildUserColumnModel_ExistHostPermission_WhenCalled_ReturnExactly()
    {
        // Act
        var actual = modelService.BuildUserColumnModel();

        // Assert
        var expected = new List<UserColumnModel>
        {
            new() { HeaderText="Employee ID", Name="EmployeeId"},
            new (){ HeaderText="Account Name", Name="UserName"},
            new (){HeaderText = "Full Name", Name = "FullName"},
            new (){HeaderText = "Nick Name", Name = "NickName"},
            new (){ HeaderText= "Department", Name= "DepartmentName"},
            new (){ HeaderText= "Section", Name= "SectionName"},
            new (){ HeaderText= "Group", Name= "GroupName" },
            new (){ HeaderText= "Role", Name= "RoleName"},
            new (){ HeaderText= "Extension", Name= "Extension" },
            new (){ HeaderText= "Phone", Name= "Phone" },
            new (){ HeaderText= "Email", Name= "Email" },
            new (){ HeaderText= "Skype", Name= "Skype" },
            new (){ HeaderText= "Category", Name= "CategoryName" },
            new (){ HeaderText= "Location", Name= "LocationCode" },
            new (){ HeaderText= "Level", Name= "LevelName"},
            new (){ HeaderText= "Pickup", Name= "Pickup"},
            new (){ HeaderText= "Address", Name= "Address" },
            new (){ HeaderText= "Second Account", Name= "IsSecondAccount" },
            new (){ HeaderText= "Language", Name= "Languages"}
        };
        DeepAssert.Equal(actual, expected);
    }
}