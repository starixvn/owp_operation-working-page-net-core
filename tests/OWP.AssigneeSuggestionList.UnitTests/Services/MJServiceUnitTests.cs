using NSubstitute;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

public class MJServiceUnitTests
{
    private List<AssignmentModel> assignments = new();

    private void Initialize()
    {
        assignments = new List<AssignmentModel>
        {
            new()
            {
                OWPMatchId = 1,
                KOTime = DateTime.Parse("2023-3-31 18:00:00").TimeOfDay,
                EventDateTime = DateTime.Parse("2023-3-31 18:00:00"),
                BettypeId = 1,
                UserId = 123,
                Remark = "ET/PK"
            }
        };
    }

    [Fact]
    public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
    {
        // Arrange
        Initialize();

        // Act
        var result = MJGroupServiceImp.BuildScheduleUpdateModel(assignments, DateTime.Parse("2023-3-31 18:00:00"));

        var expect = new ScheduleUpdateModel
        {
            FilterDate = DateTime.Parse("2023-3-31 18:00:00"),
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = true,
                EtPkValue = "ET/PK"
            },
            AssignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.MJ,
                TextColor = ColorCodeConst.TEXT_MJ,
                Text = "V"
            },
            TimeRange = (DateTime.Parse("2023-3-31 18:00:00"), DateTime.Parse("2023-3-31 20:00:00"))
        };

        //Assert
        DeepAssert.Equal(expect, result);
    }
}