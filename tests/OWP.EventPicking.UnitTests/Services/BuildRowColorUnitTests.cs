using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using OWP.EventPicking.Api.Misc;
using OWP.EventPicking.Api.Constant;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildRowColorUnitTests
{
    private readonly BuildRowColorImp _buildRowColor;

    public BuildRowColorUnitTests()
    {
        _buildRowColor = new BuildRowColorImp();
    }

    [Fact]
    public void BuildRowColor_ShouldSetGrayColor_WhenBlackListed()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                IsAwayBlackList = true,
                IsHomeBlackList = false,
                IsLeagueBlackList = false,
                Remark2 = string.Empty
            }
        };

        // Act
        _buildRowColor.BuildRowColor(matches);

        // Assert
        Assert.Equal(Constant.ColorGray, matches[0].RowColor);
    }

    [Fact]
    public void BuildRowColor_ShouldSetYellowColor_WhenMinorBetTypeAndEventPicking()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                MinorBettypeId = (byte)1,
                Remark2 = string.Empty
            }
        };

        // Act
        _buildRowColor.BuildRowColor(matches, true);

        // Assert
        Assert.Equal(Constant.ColorYellow, matches[0].RowColor);
    }

    [Fact]
    public void BuildRowColor_ShouldSetBeigeColor_WhenEventStateChanged()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                IsMovedEventDate = true,
                IsDeleted = false,
                Remark2 = "Postponed",
                InfoSiteMatchStatus = InfoSiteMatchStatus.Postponed
            }
        };

        // Act
        _buildRowColor.BuildRowColor(matches);

        // Assert
        Assert.Equal(Constant.ColorBeige, matches[0].RowColor);
    }

    [Fact]
    public void BuildRowColor_ShouldSetWhiteColor_WhenNoSpecialConditions()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                Remark2 = string.Empty
            }
        };

        // Act
        _buildRowColor.BuildRowColor(matches);

        // Assert
        Assert.Equal(Constant.ColorWhite, matches[0].RowColor);
    }

    [Fact]
    public void BuildRowColor_ShouldPrioritizeBlacklistOverOtherConditions()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                IsAwayBlackList = true,
                MinorBettypeId = (byte)1,
                IsMovedEventDate = true,
                Remark2 = string.Empty
            }
        };

        // Act
        _buildRowColor.BuildRowColor(matches, true);

        // Assert
        Assert.Equal(Constant.ColorGray, matches[0].RowColor);
    }

    [Fact]
    public void BuildRowColor_ShouldPrioritizeEventStateOverMinorBetType()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                MinorBettypeId = (byte)1,
                IsMovedEventDate = true,
                Remark2 = string.Empty
            }
        };

        // Act
        _buildRowColor.BuildRowColor(matches, true);

        // Assert
        Assert.Equal(Constant.ColorYellow, matches[0].RowColor);
    }

    [Theory]
    [InlineData("SABA - 5 Mins Play", true)]
    [InlineData("SABA - 6 Mins Play", true)]
    [InlineData("SABA - 15 Mins Play", true)]
    [InlineData("SABA - 16 Mins Play", true)]
    [InlineData("SABA - 20 Mins Play", true)]
    [InlineData("League - 20 Mins Play", true)]
    [InlineData("SABA - 10 Mins Play", false)]
    [InlineData("Other League", false)]
    public void IsSaBaGame_ShouldIdentifySaBaGamesCorrectly(string leagueName, bool expectedResult)
    {
        var result = BuildRowColorImp.IsSaBaGame(leagueName);
        Assert.Equal(expectedResult, result);
    }
}