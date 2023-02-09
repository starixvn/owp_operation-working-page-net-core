namespace OWP.AssignmentUpload.UnitTests.Submit.Services;

using System.Collections.Generic;
using System.Linq;
using AssignmentUpload.Submit.Models;
using AssignmentUpload.Submit.Services;
using AssignmentUpload.Validation.Models;
using Xunit;
using Xunit.Asserts.Compare;

public class AssignmentAnalyzerUnitTests
{
    private readonly AssignmentAnalyzerImp analyzer;

    public AssignmentAnalyzerUnitTests()
    {
        analyzer = new AssignmentAnalyzerImp();
    }

    [Fact]
    public void GetAgentOfMatch_WhenComparableMatchIdAndAgent_ReturnNewListMatchAgent()
    {
        var assignmentModel = new AssignmentModel
        {
            HashKey = "d8a864fbe77108d7cfb0dfe6d9bd0a98d98bdae4e8099adcf47b0fe843cb9f",
            Away = "Liverpool",
            Home = "Arsenal",
            League = "PremeirLeague",
            Agent = "IPSD",
            InfoSiteMatchId = 0,
            AdminSiteId = 0,
            OWPMatchId = 1234,
            MatchNo = "",
            MatchOffer = "20s Accept"
        };
        var expected = new List<MatchAgent>
        {
            new MatchAgent
            {
                MatchId = 1234,
                Agent = "IPSD"
            }
        };

        var actual = analyzer.GetAgentOfMatch(assignmentModel).ToList();

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void GetSettingsOfMatch_AvailableAssignmentModel_ReturnListMatchSetting()
    {
        var assignmentModel = new AssignmentModel
        {
            HashKey = "d8a864fbe77108d7cfb0dfe6d9bd0a98d98bdae4e8099adcf47b0fe843cb9f",
            Away = "Liverpool",
            Home = "Arsenal",
            League = "PremeirLeague",
            Agent = "IPSD",
            InfoSiteMatchId = 0,
            AdminSiteId = 0,
            OWPMatchId = 1234,
            MatchNo = "",
            MatchOffer = "20s Accept",
            MaxBetHDP = 2000,
            MaxBet1H1X2 = 2000,
            MaxBet1HHDP = 2000,
            MaxBet1HOU = 2000,
            MaxBet1X2 = 2000,
            MaxBetFM = 2000,
            MaxBetOU = 2000,
            MaxBetXMins = 4000,
            Spread1H1X2 = 50,
            Spread1HHDP = 50,
            Spread1HOU = 50,
            Spread1X2 = 50,
            SpreadFM = 50,
            SpreadHDP = 50,
            SpreadOU = 50,
            SpreadXMins = 50,
        };

        var expected = ExpectedDataForTest();

        var actual = analyzer.GetSettingsOfMatch(assignmentModel).ToList();

        DeepAssert.Equal(expected, actual);
    }

    private static IEnumerable<MatchSetting> ExpectedDataForTest()
    {
        var matchSetting1 = new MatchSetting
        {
            MatchId = 1234,
            BetType = 1,
            MaxBet = 2000,
            Spread = 50
        };

        var matchSetting2 = new MatchSetting
        {
            MatchId = 1234,
            BetType = 5,
            MaxBet = 2000,
            Spread = 50
        };

        var matchSetting3 = new MatchSetting
        {
            MatchId = 1234,
            BetType = 15,
            MaxBet = 2000,
            Spread = 50
        };

        var matchSetting4 = new MatchSetting
        {
            MatchId = 1234,
            BetType = 7,
            MaxBet = 2000,
            Spread = 50
        };

        var matchSetting5 = new MatchSetting
        {
            MatchId = 1234,
            BetType = 3,
            MaxBet = 2000,
            Spread = 50
        };

        var matchSetting6 = new MatchSetting
        {
            MatchId = 1234,
            BetType = 8,
            MaxBet = 2000,
            Spread = 50
        };

        var listMatchSetting = new List<MatchSetting>
        {
            matchSetting1,
            matchSetting2,
            matchSetting3,
            matchSetting4,
            matchSetting5,
            matchSetting6
        };
        return listMatchSetting;
    }

    [Fact]
    public void GetAssignmentOfMatch_HaveAllOtherTraderFullTimeAndAllTrader_ReturnListMatchAssignment()
    {
        var assignmentModel = new AssignmentModel
        {
            OWPMatchId = 1234,
            HashKey = "h2",
            Away = "Liverpool",
            Home = "Arsenal",
            League = "PremeirLeague",
            Agent = "IPSD",
            Channel = "",
            IPTV = "",
            Division = "",
            InfoSiteMatchId = 0,
            AdminSiteId = 0,
            MatchNo = "",
            MatchOffer = "20s Accept",
            OtherProvider = "RB2/IPSD",
            Provider = "IPSD",
            TV = "Euro10",
            TraderHDP = 234,
            Trader1HOU = 234,
            Trader10Mins = 234,
            Trader15Mins = 234,
            Trader1H1X2 = 234,
            Trader1HHDP = 234,
            Trader1X2 = 234,
            TraderAIO = 234,
            TraderFM = 234,
            TraderLA = 234,
            TraderOU = 234,
            OtherTradersHDP = "123,starix1 abc<o>456,starix16 abc",
            OtherTraders1H1X2 = "0,starix1",
            OtherTraders1HHDP = "122,#<user> trader1h",
            OtherTraders1HOU = "0,aaaa",
            OtherTraders1X2 = "0,ET Same",
            OtherTradersOU = "193,#<user>",
            OtherTradersLA = "0,traderLa"
        };

        var expected = ExpectedData();
        var actual = analyzer.GetAssignmentOfMatch(assignmentModel);

        var orderedExpected = expected.OrderBy(b => b.AssignmentType).ThenBy(b => b.BetType).ToArray();
        var orderedActual = actual.OrderBy(b => b.AssignmentType).ThenBy(b => b.BetType).ToArray();

        DeepAssert.Equal(orderedExpected, orderedActual);
    }

    [Fact]
    public void GetOtherTraderFastMarket_HaveOtherTrader_ReturnListMatchAssignment()
    {
        var assignmentModel = new AssignmentModel
        {
            OWPMatchId = 1234,
            HashKey = "h2",
            Away = "Liverpool",
            Home = "Arsenal",
            League = "PremeirLeague",
            Agent = "IPSD",
            Channel = "",
            IPTV = "",
            Division = "",
            InfoSiteMatchId = 0,
            AdminSiteId = 0,
            MatchNo = "",
            MatchOffer = "20s Accept",
            OtherProvider = "RB2/IPSD",
            Provider = "IPSD",
            TV = "Euro10",
            TraderHDP = 234,
            Trader1HOU = 234,
            Trader10Mins = 234,
            Trader15Mins = 234,
            Trader1H1X2 = 234,
            Trader1HHDP = 234,
            Trader1X2 = 234,
            TraderAIO = 234,
            TraderFM = 234,
            TraderLA = 234,
            TraderOU = 234,
            OtherTradersHDP = "123,starix1 abc<o>456,starix16 abc",
            OtherTraders1H1X2 = "0,starix1",
            OtherTraders1HHDP = "122,#<user> trader1h",
            OtherTraders1HOU = "0,aaaa",
            OtherTraders1X2 = "0,ET Same",
            OtherTradersOU = "193,#<user>",
            OtherTradersLA = "0,traderLa",
            OtherTradersFM = "123,starix1 abc<o>456,starix16 abc"
        };
        var actual = analyzer.GetOtherTraderFastMarket(assignmentModel, 123).ToList();

        var actualExpected = new List<MatchAssignment>
        {
            new MatchAssignment
            {
                AssignmentNote="starix1 abc",
                AssignmentType=2,
                BetType=-1003,
                MatchId=1234,
                UserId=123
            },
            new MatchAssignment
            {
                AssignmentNote="starix16 abc",
                AssignmentType=2,
                BetType=-1003,
                MatchId=1234,
                UserId=456
            }
        };

        DeepAssert.Equal(actualExpected, actual);
    }

    [Fact]
    public void GetOtherTraderAllInOne_HaveOtherTrader_ReturnListMatchAssignment()
    {
        var assignmentModel = new AssignmentModel
        {
            OWPMatchId = 1234,
            OtherTradersAIO = "123,starix1 abc<o>456,starix16 abc"
        };
        var actual = analyzer.GetOtherTraderAllInOne(assignmentModel, 123).ToList();

        var actualExpected = new List<MatchAssignment>
        {
            new MatchAssignment
            {
                AssignmentNote="starix1 abc",
                AssignmentType=2,
                BetType=-1004,
                MatchId=1234,
                UserId=123
            },
            new MatchAssignment
            {
                AssignmentNote="starix16 abc",
                AssignmentType=2,
                BetType=-1004,
                MatchId=1234,
                UserId=456
            }
        };

        DeepAssert.Equal(actualExpected, actual);
    }

    [Fact]
    public void GetOtherTrader10Mins_HaveOtherTrader_ReturnListMatchAssignment()
    {
        var assignmentModel = new AssignmentModel
        {
            OWPMatchId = 1234,
            OtherTraders10Mins = "123,starix1 abc<o>456,starix16 abc"
        };
        var actual = analyzer.GetOtherTrader10Mins(assignmentModel, 123).ToList();

        var actualExpected = new List<MatchAssignment>
        {
            new MatchAssignment
            {
                AssignmentNote="starix1 abc",
                AssignmentType=2,
                BetType=-1001,
                MatchId=1234,
                UserId=123
            },
            new MatchAssignment
            {
                AssignmentNote="starix16 abc",
                AssignmentType=2,
                BetType=-1001,
                MatchId=1234,
                UserId=456
            }
        };

        DeepAssert.Equal(actualExpected, actual);
    }

    [Fact]
    public void GetOtherTrader15Mins_HaveOtherTrader_ReturnListMatchAssignment()
    {
        var assignmentModel = new AssignmentModel
        {
            OWPMatchId = 1234,
            OtherTraders15Mins = "123,starix1 abc<o>456,starix16 abc"
        };
        var actual = analyzer.GetOtherTrader15Mins(assignmentModel, 123).ToList();

        var actualExpected = new List<MatchAssignment>
        {
            new MatchAssignment
            {
                AssignmentNote="starix1 abc",
                AssignmentType=2,
                BetType=-1001,
                MatchId=1234,
                UserId=123
            },
            new MatchAssignment
            {
                AssignmentNote="starix16 abc",
                AssignmentType=2,
                BetType=-1001,
                MatchId=1234,
                UserId=456
            }
        };

        DeepAssert.Equal(actualExpected, actual);
    }

    private static List<MatchAssignment> ExpectedData()
    {
        var matchAssignment1002 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = -1002,
            UserId = 234,
            AssignmentType = 1
        };

        var matchAssignment1 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 1,
            UserId = 234,
            AssignmentType = 1
        };

        var matchAssignment3 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 3,
            UserId = 234,
            AssignmentType = 1
        };
        var matchAssignment5 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 5,
            UserId = 234,
            AssignmentType = 1
        };

        var matchAssignment7 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 7,
            UserId = 234,
            AssignmentType = 1
        };

        var matchAssignment8 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 8,
            UserId = 234,
            AssignmentType = 1
        };

        var matchAssignment15 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 15,
            UserId = 234,
            AssignmentType = 1
        };

        var matchAssignment21 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 1,
            UserId = 123,
            AssignmentType = 2,
            AssignmentNote = "starix1 abc"
        };

        var matchAssignment22 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 1,
            UserId = 456,
            AssignmentType = 2,
            AssignmentNote = "starix16 abc"
        };

        var matchAssignment215 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 15,
            UserId = 0,
            AssignmentType = 2,
            AssignmentNote = "starix1"
        };

        var matchAssignment27 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 7,
            UserId = 122,
            AssignmentType = 2,
            AssignmentNote = "#<user> trader1h"
        };

        var matchAssignment28 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 8,
            UserId = 0,
            AssignmentType = 2,
            AssignmentNote = "aaaa"
        };

        var matchAssignment25 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 5,
            UserId = 234,
            AssignmentType = 2,
            AssignmentNote = "ET Same"
        };

        var matchAssignment21002 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = -1002,
            UserId = 0,
            AssignmentType = 2,
            AssignmentNote = "traderLa"
        };

        var matchAssignment23 = new MatchAssignment
        {
            MatchId = 1234,
            BetType = 3,
            UserId = 193,
            AssignmentType = 2,
            AssignmentNote = "#<user>"
        };

        var listMatchAssignment = new List<MatchAssignment>
        {
            matchAssignment1,
            matchAssignment3,
            matchAssignment5,
            matchAssignment7,
            matchAssignment8,
            matchAssignment15,
            matchAssignment1002,
            matchAssignment21,
            matchAssignment22,
            matchAssignment21002,
            matchAssignment215,
            matchAssignment25,
            matchAssignment27,
            matchAssignment28,
            matchAssignment23
        };

        return listMatchAssignment;
    }

    [Fact]
    public void GetOtherTraders_Null_ReturnsNoTrader()
    {
        const int mainTrader = 111;
        const string otherTrader = "";
        var actual = analyzer.GetOtherTraders(otherTrader, mainTrader);

        Assert.Empty(actual);
    }

    [Fact]
    public void GetOtherTraders_WithValidTraderId_ReturnsThatTrader()
    {
        const int mainTrader = 111;
        const string otherTrader = "122,#<user> my note";
        var expected = new OtherTrader
        {
            AssignmentNote = "#<user> my note",
            UserId = 122
        };
        var actual = analyzer.GetOtherTraders(otherTrader, mainTrader);

        Assert.Single(actual);
        DeepAssert.Equal(expected, actual.First());
    }

    [Theory]
    [InlineData("ET/PK Same")]
    [InlineData("PK/ET Same")]
    [InlineData("PK Same")]
    [InlineData("ET Same")]
    [InlineData("ET same")]
    [InlineData("et same")]
    [InlineData("pk same")]
    public void GetOtherTraders_PKSameMultipleFormat_WithInvalidTraderId_ReturnsMainTrader(string note)
    {
        var otherTrader = $"0,{note}";
        const int mainTrader = 111;
        var expected = new OtherTrader
        {
            AssignmentNote = $"{note}",
            UserId = 111
        };

        var actual = analyzer.GetOtherTraders(otherTrader, mainTrader);

        Assert.Single(actual);
        DeepAssert.Equal(expected, actual.First());
    }

    [Fact]
    public void GetOtherTraders_WithInvalidTraderIdAndInvalidMainTrader_ReturnsZeroTrader()
    {
        const string otherTrader = "0,starix1 testing";
        const int mainTrader = 0;
        var expected = new OtherTrader
        {
            AssignmentNote = "starix1 testing",
            UserId = 0
        };

        var actual = analyzer.GetOtherTraders(otherTrader, mainTrader);

        Assert.Single(actual);
        DeepAssert.Equal(expected, actual.First());
    }

    [Fact]
    public void GetOtherTraders_NotPKSame_WithInvalidTraderId_ReturnsZeroTrader()
    {
        const string otherTrader = "0,My Note";
        const int mainTrader = 111;
        var expected = new OtherTrader
        {
            AssignmentNote = "My Note",
            UserId = 0
        };

        var actual = analyzer.GetOtherTraders(otherTrader, mainTrader);

        Assert.Single(actual);
        DeepAssert.Equal(expected, actual.First());
    }

    [Fact]
    public void GetTelevisionOfMatch_NotPKSame_WithInvalidTraderId_ReturnsZeroTrader()
    {
        var assignment = new AssignmentModel
        {
            TV = "AB/hh /io ",
            OWPMatchId = 2
        };
        var expected = new List<MatchTelevision>
        {
            new MatchTelevision
            {
                MatchId = 2,
                TV = "AB"
            },
            new MatchTelevision
            {
                MatchId = 2,
                TV = "hh"
            },
            new MatchTelevision
            {
                MatchId = 2,
                TV = "io"
            },
        };

        var actual = analyzer.GetTelevisionOfMatch(assignment);
        DeepAssert.Equal(expected, actual.ToList());
    }

    [Fact]
    public void ReassignMatchId_Always_ReturnAssignmentModelList()
    {
        var assignmentModel = new List<AssignmentModel>
        {
            new AssignmentModel
            {
                OWPMatchId = 1234,
                Away = "Liverpool",
                HashKey = "h1",
                Home = "Arsenal",
                League = "PremeirLeague",
                Agent = "IPSD"
            },

            new AssignmentModel
            {
                OWPMatchId = 2345,
                Away = "Chelsea",
                Home = "MC",
                League = "PremeirLeague",
                HashKey = "h2"
            }
        };

        var hashKeyMap = new List<MatchByHashKey>
        {
            new MatchByHashKey
            {
                OWPMatchId = 1234,
                HashKey = "h1"
            },

            new MatchByHashKey
            {
                OWPMatchId = 2345,
                HashKey = "h2"
            }
        };

        var expectedAssignmentModel = new List<AssignmentModel>
        {
            new AssignmentModel
            {
                OWPMatchId = 1234,
                Away = "Liverpool",
                HashKey = "h1",
                Home = "Arsenal",
                League = "PremeirLeague",
                Agent = "IPSD"
            },

            new AssignmentModel
            {
                OWPMatchId = 2345,
                Away = "Chelsea",
                Home = "MC",
                League = "PremeirLeague",
                HashKey = "h2"
            }
        };

        analyzer.ReassignMatchId(assignmentModel, hashKeyMap);

        DeepAssert.Equal(assignmentModel, expectedAssignmentModel);
    }
}