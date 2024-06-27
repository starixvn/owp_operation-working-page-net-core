namespace OWP.UserManagement.UnitTests.Services;

using Models;
using NSubstitute;
using OWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using UserManagement.Services;
using Xunit;

public class ExportServiceUnitTests
{
    private readonly ModelService modelService;
    private readonly ExportService exportService;

    public ExportServiceUnitTests()
    {
        modelService = Substitute.For<ModelService>();
        exportService = new ExportServiceImp(modelService);
    }

    [Fact]
    public void Export_WhenCall_Successfully()
    {
        // Act
        var userPermission = new UserManagementPermission
        {
            UserManagement = new PermissionModel(true, true),
            UserManagementAllSections = new PermissionModel(true, true),
            UserManagementOwnSection = new PermissionModel(true, true)
        };
        var users = new List<User>
        {
            new()
            {
                EmployeeId="1234",
                UserName="Kane",
                FullName="Le Thanh Tuan",
                NickName="Kane",
                DepartmentName="Development",
                SectionName="Dev",
                GroupName="Olympus",
                RoleName="Employeer",
                Phone="1234567890"
            },
            new()
            {
                EmployeeId="1232",
                UserName="Kane1",
                FullName="Le Thanh Tuan",
                NickName="Kane1",
                DepartmentName="Development",
                SectionName="Dev",
                GroupName="Olympus",
                RoleName="Employeer",
                Phone="123456795"
            }
        };

        var userColumnModels = new List<UserColumnModel>
        {
            new () { HeaderText="Employee ID", Name="EmployeeId"},
            new () {HeaderText="Account Name", Name="AccountName"},
            new () { HeaderText="Full Name", Name="FullName"},
            new () {HeaderText="Nick Name", Name="NickName"},
            new () { HeaderText= "Department", Name= "Department"},
            new () { HeaderText= "Section" , Name= "Section" },
            new () { HeaderText= "Group" , Name= "Group" },
            new () { HeaderText= "Role", Name= "Role"  },
            new () { HeaderText= "Extension" , Name= "Extension" },
            new () { HeaderText= "Phone" , Name= "Phone" },
            new () { HeaderText= "Email" , Name= "Email" },
            new () { HeaderText= "Skype" , Name= "Skype" },
            new () { HeaderText= "Category", Name= "Category" },
            new () { HeaderText= "Location",Name= "Location"},
            new () { HeaderText= "Level", Name= "Level" },
            new () { HeaderText= "Pickup", Name= "Pickup" },
            new () { HeaderText= "Address", Name= "Address" },
            new () { HeaderText= "Second Account", Name= "IsSecondAccount" }
        };

        modelService.BuildPermissionViewModel().Returns(userPermission);
        modelService.BuildUserColumnModel().Returns(userColumnModels);

        var exportDate = DateTime.Now;
        using (var stream = new MemoryStream())
        {
            exportService.ExportToExcel(stream, users, exportDate);
        }
    }
}
