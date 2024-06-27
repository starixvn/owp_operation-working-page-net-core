using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

using Xunit.Asserts.Compare;

public class XMinsServiceUnitTests
{
    private List<AssignmentModel> assignmentsCorner = [];

    private void Initialize()
    {
        assignmentsCorner =
        [
            new AssignmentModel
            {
                OWPMatchId = 2,
                KOTime = DateTime.Parse("2023-3-31 18:05:00").TimeOfDay,
                EventDateTime = DateTime.Parse("2023-3-31 18:05:00"),
                IsCornerMatch = true,
                BettypeId = 1,
                UserId = 123,
                ParentMatchId = 1,
                Remark = "ET/PK"
            }
        ];
    }

    [Fact]
    public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
    {
        // Arrange
        Initialize();

        var result = XMinsGroupServiceImp.BuildScheduleUpdateModel(assignmentsCorner, DateTime.Parse("2023-3-31 18:05:00"));

        var expect = new ScheduleUpdateModel
        {
            FilterDate = DateTime.Parse("2023-3-31 18:05:00"),
            EtPk = new ETPKUpdateSchedule()
            ,
            AssignmentCell = new AssignmentCell
            {
                Text = "06:05"
            },
            TimeRange = (DateTime.Parse("2023-3-31 18:05:00"), DateTime.Parse("2023-3-31 19:05:00"))
        };
        DeepAssert.Equal(expect, result);
    }
}