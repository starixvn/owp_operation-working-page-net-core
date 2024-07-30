using NSubstitute;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

using Xunit.Asserts.Compare;

public class FantasyServiceUnitTests
{
    private List<AssignmentModel> assignments = [];
    private IDictionary<int, List<AssignmentModel>> gameMapGameId = new Dictionary<int, List<AssignmentModel>>();

    public FantasyServiceUnitTests()
    {
        Substitute.For<SpecialGroupService>();
    }

    private void Initialize()
    {
        assignments =
        [
            new AssignmentModel
            {
                OWPMatchId = 1,
                KOTime = DateTime.Parse("2023-3-31 18:00:00").TimeOfDay,
                EventDateTime = DateTime.Parse("2023-3-31 18:00:00"),
                BettypeId = 1,
                UserId = 123,
                Remark = "ET/PK",
                LeagueName = "VN FANTASY MATCH"
            }
        ];

        gameMapGameId = assignments.GroupBy(x => x.OWPMatchId).ToDictionary(x => x.Key, x => x.ToList());
    }

    [Fact]
    public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
    {
        // Arrange
        Initialize();

        // Act
        var result = FantasyServiceImp.BuildScheduleUpdateModel(gameMapGameId, assignments, DateTime.Parse("2023-3-31 18:00:00"));

        var expect = new ScheduleUpdateModel
        {
            FilterDate = DateTime.Parse("2023-3-31 18:00:00"),
            EtPk = new ETPKUpdateSchedule
            {
                EtPkValue = "PK"
            },
            AssignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.FANTASY,
                TextColor = ColorCodeConst.TEXT_FANTASY,
                Text = "V*H"
            },
            TimeRange = (DateTime.Parse("2023-3-31 18:00:00"), DateTime.Parse("2023-3-31 20:00:00"))
        };

        //Assert
        DeepAssert.Equal(expect, result);
    }
}