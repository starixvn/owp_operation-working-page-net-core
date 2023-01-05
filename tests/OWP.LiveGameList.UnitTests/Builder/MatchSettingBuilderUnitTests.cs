using OWP.LiveGameList.Builder;
using OWP.LiveGameList.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.LiveGameList.UnitTests.Builder;

public class MatchSettingBuilderUnitTests
{
    [Fact]
    public void Build_MainMatch_ReturnExpect()
    {
        var matchSettings = MockMatchSetting();
        var expected = new MatchSettingModel
        {
            Handicap = new MatchSetting { OWPMatchId = 117, ParentMatchId = 100, Maxbet = 1, MinorBettypeId = 1, Spread = 10, BettypeId = 1 },
            OverUnder = new MatchSetting { OWPMatchId = 117, ParentMatchId = 200, Maxbet = 2, MinorBettypeId = 3, Spread = 20, BettypeId = 3 }
        };
        var result = MatchSettingBuilder.Build(new MatchInfo { OWPMatchId = 117 }, matchSettings);

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void Build_MinorMatchNoofConner_ReturnExpect()
    {
        var matchSettings = MockMinorMatchSetting();
        var expected = new MatchSettingModel
        {
            Handicap = new MatchSetting { OWPMatchId = 117, ParentMatchId = 100, Maxbet = 1, MinorBettypeId = 1, Spread = 10, BettypeId = 1 }
        };
        var result = MatchSettingBuilder.Build(new MatchInfo { OWPMatchId = 117, IsMinorMatch = true, MinorBettypeId = -3 }, matchSettings);

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void Build_MinorMatchTotalBooking_ReturnExpect()
    {
        var matchSettings = MockMinorMatchSetting();
        var expected = new MatchSettingModel
        {
            Handicap = new MatchSetting { OWPMatchId = 117, ParentMatchId = 100, Maxbet = 1, MinorBettypeId = 1, Spread = 10, BettypeId = 1 }
        };
        var result = MatchSettingBuilder.Build(new MatchInfo { OWPMatchId = 117, IsMinorMatch = true, MinorBettypeId = -6 }, matchSettings);

        DeepAssert.Equal(expected, result);
    }

    public List<MatchSetting> MockMatchSetting()
        => new()
        {
            new() { BettypeId = 1, Maxbet = 1, MinorBettypeId = 1, OWPMatchId = 117, ParentMatchId = 100, Spread = 10 },
            new () { BettypeId = 3, Maxbet = 2 ,MinorBettypeId = 3, OWPMatchId = 117, ParentMatchId = 200, Spread = 20},
        };

    public List<MatchSetting> MockMinorMatchSetting()
        => new()
        {
            new() { BettypeId = 1, Maxbet = 1, MinorBettypeId = 1, OWPMatchId = 117, ParentMatchId = 100, Spread = 10 },
        };
}
