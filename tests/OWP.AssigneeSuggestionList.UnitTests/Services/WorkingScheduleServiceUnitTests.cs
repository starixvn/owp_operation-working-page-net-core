using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

using Xunit.Asserts.Compare;

public class WorkingScheduleServiceUnitTests
{
    [Fact]
    public void UpdateWorkSchedule_None_ETPK_Success()
    {
        // Arrange
        var updateModel = new ScheduleUpdateModel
        {
            FilterDate = DateTime.Parse("2023-04-04 13:00:00"),
            EtPk = new ETPKUpdateSchedule(),
            PropertiesName = new List<string>(),
            AssignmentCell = new AssignmentCell
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            TimeRange = (DateTime.Parse("2023-04-04 10:00:00"), DateTime.Parse("2023-04-04 11:00:00"))
        };

        var schedule = new WorkingSchedule
        {
            UserInfo = new UserInfo
            {
                UserId = 123,
                Username = "Los"
            }
        };

        // Act
        new WorkingScheduleServiceImp().UpdateWorkingSchedule(updateModel, schedule, new AssignmentModel());

        // Assert
        var expect = new WorkingSchedule
        {
            UserInfo = new UserInfo
            {
                UserId = 123,
                Username = "Los"
            },
            Yesterday_10h = new AssignmentCell
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            Yesterday_11h = new AssignmentCell
            {
                Text = "",
                TextColor = "#444444",
                Color = "#C00000"
            },
            CategorizedAssignments = new List<AssignmentModel> { new() }
        };

        DeepAssert.Equal(expect, schedule);
    }

    [Fact]
    public void UpdateWorkSchedule_With_ETPK_Success()
    {
        // Arrange
        var updateModel = new ScheduleUpdateModel
        {
            FilterDate = DateTime.Parse("2023-04-04 13:00:00"),
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = true,
                EtPkValue = "ETPK"
            },
            PropertiesName = new List<string>(),
            AssignmentCell = new AssignmentCell
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            TimeRange = (DateTime.Parse("2023-04-04 9:00:00"), DateTime.Parse("2023-04-04 10:00:00"))
        };

        var schedule = new WorkingSchedule
        {
            UserInfo = new UserInfo
            {
                UserId = 123,
                Username = "Los"
            }
        };

        // Act
        new WorkingScheduleServiceImp().UpdateWorkingSchedule(updateModel, schedule, new AssignmentModel());

        // Assert
        var expect = new WorkingSchedule
        {
            UserInfo = new UserInfo
            {
                UserId = 123,
                Username = "Los"
            },
            Yesterday_09h = new AssignmentCell
            {
                Text = "V/15K",
                TextColor = "#444444",
                Color = "#C00000"
            },
            Yesterday_10h = new AssignmentCell
            {
                Text = "ETPK",
                TextColor = "#FFEA29",
                Color = "#000000"
            },
            CategorizedAssignments = new List<AssignmentModel> { new() }
        };

        DeepAssert.Equal(expect, schedule);
    }
}
