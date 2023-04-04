using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

public class WorkingScheduleServiceUnitTests
{
    [Fact]
    public void UpdateWorkSchedule_None_ETPK_Success()
    {
        // Arrange
        var updateModel = new ScheduleUpdateModel
        {
            EventDate = DateTime.Parse("2023-04-04 13:00:00"),
            EtPk = new ETPKUpdateSchedule(),
            PropertiesName = new List<string>(),
            AssignmentCell= new AssignmentCell()
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            TimeRange = (DateTime.Parse("2023-04-03 13:00:00"), DateTime.Parse("2023-04-03 14:00:00"))
        };

        var schedule = new WorkingSchedule
        {
            UserInfo = new UserInfo()
            {
                UserId = 123,
                Username = "Los"
            },
        };

        // Act
        new WorkingScheduleServiceImp().UpdateWorkingSchedule(updateModel, schedule);

        // Assert
        var expect = new WorkingSchedule()
        {
            UserInfo = new UserInfo()
            {
                UserId = 123,
                Username = "Los"
            },
            Today_13h = new AssignmentCell()
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            Today_14h = new AssignmentCell()
            {
                Text = "",
                TextColor = "#444444",
                Color = "#C00000"
            }
        };

        DeepAssert.Equal(expect, schedule);
    }

    [Fact]
    public void UpdateWorkSchedule_With_ETPK_Success()
    {
        // Arrange
        var updateModel = new ScheduleUpdateModel
        {
            EventDate = DateTime.Parse("2023-04-04 13:00:00"),
            EtPk = new ETPKUpdateSchedule()
            {
                IsHasEtPk=true,
                EtPkValue = "ETPK"
            },
            PropertiesName = new List<string>(),
            AssignmentCell = new AssignmentCell()
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            TimeRange = (DateTime.Parse("2023-04-03 13:00:00"), DateTime.Parse("2023-04-03 15:00:00"))
        };

        var schedule = new WorkingSchedule
        {
            UserInfo = new UserInfo()
            {
                UserId = 123,
                Username = "Los"
            },
        };

        // Act
        new WorkingScheduleServiceImp().UpdateWorkingSchedule(updateModel, schedule);

        // Assert
        var expect = new WorkingSchedule()
        {
            UserInfo = new UserInfo()
            {
                UserId = 123,
                Username = "Los"
            },
            Today_13h = new AssignmentCell()
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            Today_14h = new AssignmentCell()
            {
                Text = "",
                TextColor = "#444444",
                Color = "#C00000"
            },
            Today_15h = new AssignmentCell()
            {
                Text = "ETPK",
                Color = ColorCodeConst.ET_PK,
                TextColor = ColorCodeConst.TEXT_ET_PK
            }
        };

        DeepAssert.Equal(expect, schedule);
    }
}
