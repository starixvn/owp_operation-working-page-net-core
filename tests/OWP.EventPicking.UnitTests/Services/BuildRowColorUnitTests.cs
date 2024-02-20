using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildRowColorUnitTests
{
    private readonly BuildRowColorImp service;
    public BuildRowColorUnitTests()
    {
        service = new BuildRowColorImp();
    }

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
                MinorBettypeId = -1,
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
        service.BuildRowColor(eventPickings);

        Assert.Equal("gray", eventPickings[0].RowColor);
        Assert.Equal("white", eventPickings[1].RowColor);
        Assert.Equal("white", eventPickings[2].RowColor);
        Assert.Equal("white", eventPickings[3].RowColor);
        Assert.Equal("white", eventPickings[4].RowColor);
        Assert.Equal("#FFFFFF", eventPickings[5].RowColorHex);
    }
}