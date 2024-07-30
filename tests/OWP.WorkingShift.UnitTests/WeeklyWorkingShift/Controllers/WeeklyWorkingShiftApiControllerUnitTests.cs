
using Xunit.Asserts.Compare;

namespace OWP.WorkingShift.UnitTests.WeeklyWorkingShift.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using Fanex.NAP.Client.Helpers.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.WorkingShift.Common.Models;
using OWP.WorkingShift.Common.Services;
using OWP.WorkingShift.WeeklyWorkingShift.Controllers;
using OWP.WorkingShift.WeeklyWorkingShift.Models;
using OWP.WorkingShift.WeeklyWorkingShift.Repositories;
using OWP.WorkingShift.WeeklyWorkingShift.Services;

public class WeeklyWorkingShiftApiControllerUnitTests
{
    private readonly WeeklyWorkingShiftApiController controller;
    private readonly Repository repository;
    private readonly ModelService modelService;

    public WeeklyWorkingShiftApiControllerUnitTests()
    {
        repository = Substitute.For<Repository>();
        modelService = Substitute.For<ModelService>();
        var importService = Substitute.For<ImportService>();
        var exportService = Substitute.For<ExportService>();
        var viewLogService = Substitute.For<ViewLogService>();
        var currentSession = Substitute.For<INapSession>();
        controller = new WeeklyWorkingShiftApiController(repository, modelService, exportService, importService, viewLogService, currentSession);
    }


    [Fact]
    public async Task Search_WhenCalled_ReturnEmployeeWorkingShiftList()
    {
        // Arrange
        var fromDate = new DateTime(2019, 06, 10);
        var toDate = new DateTime(2019, 06, 16);
        var mockModel = new QueryModel
        {
            FromDate = fromDate,
            ToDate = toDate,
            User = "",
            Section = "1",
            Category = "",
            Location = "",
            ExcludeClosedAccount = true,
            OrderUserId = 1,
            IsFrame = false,
            TimeZoneOffset = 0
        };

        var queryResult = new QueryResult
        {
            ListSectionPublish = new List<SectionPublishWorkingShiftModel>(),
            WorkingShiftColors = new List<WorkingShiftColorModel>(),
            WorkingShiftUsers = new List<WorkingShiftUserModel>()
        };

        var expect = new JsonResult(queryResult, new JsonSerializerSettings());


        repository.SearchWorkingShiftWeekly(Arg.Compat.Any<QueryModel>()).Returns(queryResult);
        modelService.BuildPermissionViewModel().Returns(new WorkingShiftPermission());

        // Act
        var actual = await controller.SearchWorkingShiftWeekly(mockModel);

        // Assert
        Assert.IsType<JsonResult>(actual);

         DeepAssert.Equal(expect.Value, actual.Value);
    }
}