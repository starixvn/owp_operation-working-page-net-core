using OWP.Event.EventShared.Builder;
using OWP.Event.EventShared.Models;
using Xunit;

namespace OWP.Event.UnitTests.EventShared.Builder;

using Xunit.Asserts.Compare;

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
        var matchInfo = new MatchInfo { OWPMatchId = 117 };

        var result = MatchSettingBuilder.Build(matchInfo, matchSettings, new Dictionary<int, List<MatchSetting>>());

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void Build_MinorMatchNoOfConner_ReturnExpect()
    {
        var matchSettings = MockMinorMatchSetting();
        var expected = new MatchSettingModel
        {
            Handicap = new MatchSetting { OWPMatchId = 117, ParentMatchId = 100, Maxbet = 1, MinorBettypeId = 1, Spread = 10, BettypeId = 1 }
        };
        var matchInfo = new MatchInfo { OWPMatchId = 117, IsMinorMatch = true, MinorBettypeId = -3 };
        var result = MatchSettingBuilder.Build(matchInfo, matchSettings, new Dictionary<int, List<MatchSetting>>());

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
        var matchInfo = new MatchInfo { OWPMatchId = 117, IsMinorMatch = true, MinorBettypeId = -6 };
        var result = MatchSettingBuilder.Build(matchInfo, matchSettings, new Dictionary<int, List<MatchSetting>>());

        DeepAssert.Equal(expected, result);
    }

    public static Dictionary<int, List<MatchSetting>> MockMatchSetting()
    {
        return new Dictionary<int, List<MatchSetting>>
        {
            {
                117, [
                    new MatchSetting
                    {
                        BettypeId = 1, Maxbet = 1, MinorBettypeId = 1, OWPMatchId = 117, ParentMatchId = 100,
                        Spread = 10
                    },

                    new MatchSetting
                    {
                        BettypeId = 3, Maxbet = 2, MinorBettypeId = 3, OWPMatchId = 117, ParentMatchId = 200,
                        Spread = 20
                    }
                ]
            }
        };
    }

    public static Dictionary<int, List<MatchSetting>> MockMinorMatchSetting()
    {
        return new Dictionary<int, List<MatchSetting>>
        {
            {
                117, [
                    new MatchSetting
                    {
                        BettypeId = 1, Maxbet = 1, MinorBettypeId = 1, OWPMatchId = 117, ParentMatchId = 100,
                        Spread = 10
                    }
                ]
            }
        };
    }
}
