using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using Xunit;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;

public class ScheduleHelperUnitTest
{
    [Fact]
    public void BuildScheduleTextSabaGroupGame_Test()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new AssignmentModel
            {
                EventDateTime = new DateTime(2023, 7, 5, 10, 0, 0),
                MatchType = MatchType.Saba5Mins
            },
            new AssignmentModel
            {
                EventDateTime = new DateTime(2023, 7, 5, 10, 0, 0),
                MatchType = MatchType.Saba5Mins
            }
        };

        // Act
        var result = ScheduleHelper.BuildScheduleTextSabaGroupGame(assignments);

        // Assert
        Assert.Equal("*V/5*2", result);
    }

    [Fact]
    public void BuildScheduleTextGroupGame_Test()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new AssignmentModel
            {
                EventDateTime = new DateTime(2023, 7, 5, 10, 30, 0),
                MaxbetHDP = 1000
            },
            new AssignmentModel
            {
                EventDateTime = new DateTime(2023, 7, 5, 10, 45, 0),
                MaxbetHDP = 2000
            }
        };

        // Act
        var result = ScheduleHelper.BuildScheduleTextGroupGame(assignments);

        // Assert
        Assert.Equal("10:30+10:45/1K+2K", result);
    }

    [Fact]
    public void BuildScheduleTextSabaSingleGame_Test()
    {
        // Arrange
        var assignment = new AssignmentModel
        {
            KOTime = new TimeSpan(10, 0, 0),
            MatchType = MatchType.Saba5Mins
        };

        // Act
        var result = ScheduleHelper.BuildScheduleTextSabaSingleGame(assignment);

        // Assert
        Assert.Equal("*V/5", result);
    }

    [Fact]
    public void BuildScheduleTextSingleGame_Test()
    {
        // Arrange
        var assignment = new AssignmentModel
        {
            KOTime = new TimeSpan(10, 30, 0),
            MaxbetHDP = 1000
        };

        // Act
        var result = ScheduleHelper.BuildScheduleTextSingleGame(assignment, "*H");

        // Assert
        Assert.Equal("10:30/1K*H", result);
    }

    [Fact]
    public void BuildTextSingleGamePostfixHdp_Test()
    {
        // Arrange
        var gameMapGameId = new Dictionary<int, List<AssignmentModel>>
        {
            { 1, new List<AssignmentModel> { new AssignmentModel { OWPMatchId = 1, BettypeId = Bettype.OverUnder, UserId = 1 } } }
        };
        var assignments = new List<AssignmentModel>
        {
            new AssignmentModel { OWPMatchId = 1, BettypeId = Bettype.Handicap, UserId = 1 }
        };

        // Act
        var result = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignments);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void BuildTextSingleGamePostfixOu_Test()
    {
        // Arrange
        var gameMapGameId = new Dictionary<int, List<AssignmentModel>>
        {
            { 1, new List<AssignmentModel> { new AssignmentModel { OWPMatchId = 1, BettypeId = Bettype.Handicap, UserId = 1 } } }
        };
        var assignments = new List<AssignmentModel>
        {
            new AssignmentModel { OWPMatchId = 1, BettypeId = Bettype.OverUnder, UserId = 1 }
        };

        // Act
        var result = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignments);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}