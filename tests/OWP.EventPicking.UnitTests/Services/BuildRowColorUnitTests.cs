using OWP.EventPicking.Api.Misc;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildRowColorUnitTests
{
    private readonly BuildRowColorImp service = new();

    [Fact]
    public void BuildRowColor_MapRowColor()
    {
        var eventPickings = new List<EventPickingModel>
        {
            new() { // Gray
                IsBlackList = true,
                OtherProvider = "RB2",
                MaxBetHDPNumber = 1000,
                MinorBettypeId = null,
                LeagueName = "League"
            },
            new() { // Red
                IsBlackList = false,
                OtherProvider = "",
                MaxBetHDPNumber = 1000,
                MinorBettypeId = null,
                LeagueName = "Saba"
            },
            new() { // White
                IsBlackList = false,
                OtherProvider = "",
                MaxBetHDPNumber = null,
                MinorBettypeId = null,
                LeagueName = "Saba - 5 Mins Play"
            },
            new() { // white
                IsBlackList = false,
                OtherProvider = "RB2",
                MaxBetHDPNumber = 1000,
                MinorBettypeId = null,
                LeagueName = "LeagueA"
            },
            new() { // Red
                IsBlackList = false,
                OtherProvider = "",
                MaxBetHDPNumber = null,
                MinorBettypeId = null,
                LeagueName = "Saba"
            },
            new() { // White
                IsBlackList = false,
                OtherProvider = "",
                MaxBetHDPNumber = null,
                MinorBettypeId = null,
                LeagueName = "League - 20 Mins Play"
            }
        };
        service.BuildRowColor(eventPickings,false);

        Assert.Equal("white", eventPickings[0].RowColor);
        Assert.Equal("white", eventPickings[1].RowColor);
        Assert.Equal("white", eventPickings[4].RowColor);
        Assert.Equal("#FFFFFF", eventPickings[5].RowColorHex);
    }

    [Fact]
    public void BuildRowColor_MapRowColor2()
    {
        var eventPickings = new List<EventPickingModel>
        {
            new() { IsAwayBlackList = true, LeagueName = "League" },
            new() { IsHomeBlackList = false, LeagueName = "Saba" },
            new() { IsLeagueBlackList = false, LeagueName = "Saba - 5 Mins Play" },
            new() { IsAwayBlackList = false, LeagueName = "LeagueA" },
            new() { IsHomeBlackList = false, LeagueName = "Saba" },
            new() { IsLeagueBlackList = false, LeagueName = "League - 20 Mins Play" }
        };

        service.BuildRowColor(eventPickings, false);

        Assert.Equal(Constant.ColorGray, eventPickings[0].RowColor);
        Assert.Equal(Constant.ColorWhite, eventPickings[1].RowColor);
        Assert.Equal(Constant.ColorWhite, eventPickings[2].RowColor);
        Assert.Equal(Constant.ColorWhite, eventPickings[3].RowColor);
        Assert.Equal(Constant.ColorWhite, eventPickings[4].RowColor);
        Assert.Equal(Constant.ColorWhite, eventPickings[5].RowColor);
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