using Fanex.NAP.Client.Helpers.Web;
using Microsoft.AspNetCore.Mvc;
using OWP.WorkingShift.DailyWorkingShift.Controllers;
using OWP.WorkingShift.DailyWorkingShift.Models;
using OWP.WorkingShift.DailyWorkingShift.Repositories;
using OWP.WorkingShift.DailyWorkingShift.Services;
using Xunit.Asserts.Compare;

namespace OWP.WorkingShift.UnitTests.DailyWorkingShift.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using NSubstitute;
    using Xunit;

    public class DailyWorkingShiftControllerUnitTests
    {
        [Fact]
        public async Task SearchWorkingShiftDaily_Always_ReturnsDataFromService()
        {
            // Arrange
            var queryResult = new QueryResult()
            {
                Matches = new List<QueryMatchInfo>(),
                Users = new List<QueryUserGroup>()
            };
            var expect = new JsonResult(queryResult, new JsonSerializerSettings());
            
            var repository = Substitute.For<Repository>();
            var permissionService = Substitute.For<ModelService>();
            var currentSession = Substitute.For<INapSession>();
            repository.Query(Arg.Compat.Any<QueryModel>()).Returns(Task.FromResult(queryResult));
            permissionService.BuildPermissionViewModel().Returns(new DailyWorkingShiftPermission());
            currentSession.UserName.Returns("starix");
            var controller = new DailyWorkingShiftApiController(repository, permissionService, currentSession);

            // Act
            var queryModel = new QueryModel
            {
                Date = DateTime.Now,
                SectionId = "1,2",
                LocationCode = "CTI",
                ShiftId = "1,2,3",
                User = "starix",
                ExcludeClosedAccount = true
            };
            var actual = await controller.SearchWorkingShiftDaily(queryModel);

            // Assert
            DeepAssert.Equal(expect, actual);
        }
    }
}