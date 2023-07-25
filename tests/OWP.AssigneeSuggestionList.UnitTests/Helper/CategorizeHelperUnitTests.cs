using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;
using MatchType = Constant.MatchType;

public class CategorizeHelperUnitTests
{
    [Fact]
    public void IsFreeInRangeTime_NoAssignmentsInRange_ReturnsTrue()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                EventDateTime = new DateTime(2023, 4, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 4, 20, 12, 0, 0)
            },
            new()
            {
                EventDateTime = new DateTime(2023, 4, 21, 6, 0, 0),
                EndTime = new DateTime(2023, 4, 21, 8, 0, 0)
            }
        };
        var kOTime = new DateTime(2023, 4, 22, 8, 0, 0);
        var endTime = new DateTime(2023, 4, 22, 10, 0, 0);

        // Act
        var isFree = assignments.IsFreeInRangeTime(kOTime, endTime);

        // Assert
        Assert.True(isFree);
    }

    [Fact]
    public void IsFreeInRangeTime_OneAssignmentInRange_ReturnsFalse()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                EventDateTime = new DateTime(2023, 4, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 4, 20, 12, 0, 0)
            },
            new()
            {
                EventDateTime = new DateTime(2023, 4, 21, 14, 0, 0),
                EndTime = new DateTime(2023, 4, 21, 16, 0, 0)
            }
        };
        var kOTime = new DateTime(2023, 4, 20, 11, 0, 0);
        var endTime = new DateTime(2023, 4, 20, 13, 0, 0);

        // Act
        var isFree = assignments.IsFreeInRangeTime(kOTime, endTime);

        // Assert
        Assert.False(isFree);
    }

    [Fact]
    public void IsFreeInRangeTime_AllAssignmentsInRange_ReturnsFalse()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                EventDateTime = new DateTime(2023, 4, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 4, 20, 16, 0, 0)
            },
            new()
            {
                EventDateTime = new DateTime(2023, 4, 21, 14, 0, 0),
                EndTime = new DateTime(2023, 4, 21, 16, 0, 0)
            }
        };
        var kOTime = new DateTime(2023, 4, 20, 10, 30, 0);
        var endTime = new DateTime(2023, 4, 21, 15, 0, 0);

        // Act
        var isFree = assignments.IsFreeInRangeTime(kOTime, endTime);

        // Assert
        Assert.False(isFree);
    }

    [Theory]
    [InlineData("SABA ELITE FRIENDLY Virtual PES 20 - 5 Mins Play - ALL IN ONE", MatchType.Saba5Mins)]
    [InlineData("SABA CLUB FRIENDLY Virtual PES 20 - 6 Mins Play", MatchType.SabaFutsal6Mins)]
    [InlineData("M88 SABA SPANISH LEAGUE Virtual FIFA 21 Season 6 - 16 Mins Play", MatchType.SabaM8816Mins)]
    [InlineData("saba", MatchType.SabaOther)]
    [InlineData("SABA LeagueTest-LT - 20 Mins Play", MatchType.Saba20Mins)]
    [InlineData("SABA CLUB FRIENDLY Virtual FIFA 21 - 16 Mins Play b", MatchType.Saba16Mins)]
    [InlineData("SABA CLUB FRIENDLY Virtual PES 20 - 15 Mins Play", MatchType.Saba15Mins)]
    [InlineData("SABA CLUB FRIENDLY Virtual PES 15 Mins Play", MatchType.SabaOther)]
    public void CategorizeSabaMatches(string leagueName, MatchType expected)
    {
        var actual = leagueName.GetSabaBetType();

        Assert.Equal(expected, actual);
    }
}
