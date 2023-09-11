using OWP.Enums;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;
using Xunit;

namespace OWP.Maxbet.UnitTests.Helpers;

using Xunit.Asserts.Compare;

public class MaxbetHelperUnitTests
{
    #region trader assignment

    [Fact]
    public void FindTraderAssignmentByBettype_ListAssignment_BettypeId_Found_ReturnAssignmentHDP()
    {
        var assignments = new List<TraderAssignment>
        {
            new()
            {
                BettypeId= (int)EnumBettype.Handicap,
                UserId=111
            },
            new()
            {
                BettypeId= (int)EnumBettype.OverUnder,
                UserId=222
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                UserId=333
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                UserId=444
            }
        };
        var result = MaxbetHelper.FindTraderAssignmentByBettype(assignments, (int)EnumBettype.Handicap);
        var expected = new TraderAssignment
        {
            BettypeId = (int)EnumBettype.Handicap,
            UserId = 111
        };
        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void FindTraderAssignmentByBettype_ListAssignment_BettypeId_NotFound_ReturnNull()
    {
        var assignments = new List<TraderAssignment>
        {
            new()
            {
                BettypeId= (int)EnumBettype.OverUnder,
                UserId=222
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                UserId=333
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                UserId=444
            }
        };
        var result = MaxbetHelper.FindTraderAssignmentByBettype(assignments, (int)EnumBettype.Handicap);
        Assert.Null(result);
    }

    #endregion trader assignment

    #region spread

    [Fact]
    public void FindSpreadByBettype_ListSpread_BettypeId_Found_ReturnSpreadHDP()
    {
        var spreads = new List<SpreadModel>
        {
            new()
            {
                Bettype= (int)EnumBettype.Handicap,
                Spread=(decimal)0.1
            },
            new()
            {
                Bettype= (int)EnumBettype.OverUnder,
                Spread=(decimal)0.3
            },
            new()
            {
                Bettype= (int)EnumBettype.FirstHalfHandicap,
                Spread=(decimal)0.3
            },
            new()
            {
                Bettype= (int)EnumBettype.FirstHalfOverUnder,
                Spread=(decimal)0.4
            }
        };
        var result = MaxbetHelper.FindSpreadByBettype(spreads, (int)EnumBettype.Handicap);
        var expected = new SpreadModel
        {
            Bettype = (int)EnumBettype.Handicap,
            Spread = (decimal)0.1
        };
        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void FindSpreadByBettype_ListSpread_BettypeId_Found_ReturnNull()
    {
        var spreads = new List<SpreadModel>
        {
            new()
            {
                Bettype= (int)EnumBettype.OverUnder,
                Spread=(decimal)0.3
            },
            new()
            {
                Bettype= (int)EnumBettype.FirstHalfHandicap,
                Spread=(decimal)0.3
            },
            new()
            {
                Bettype= (int)EnumBettype.FirstHalfOverUnder,
                Spread=(decimal)0.4
            }
        };
        var result = MaxbetHelper.FindSpreadByBettype(spreads, (int)EnumBettype.Handicap);
        Assert.Null(result);
    }

    #endregion spread

    #region maxbet

    [Fact]
    public void FindMaxbetByBettype_ListMaxbet_BettypeId_Found_ReturnMaxbetHDP()
    {
        var maxbets = new List<MaxbetModel>
        {
            new()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=(decimal)0.1
            },
            new()
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=(decimal)0.3
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                Value=(decimal)0.3
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                Value=(decimal)0.4
            }
        };
        var result = MaxbetHelper.FindMaxbetByBettype(maxbets, (int)EnumBettype.Handicap);
        var expected = new MaxbetModel
        {
            BettypeId = (int)EnumBettype.Handicap,
            Value = (decimal)0.1
        };
        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void FindMaxbetByBettype_ListSpread_BettypeId_Found_ReturnNull()
    {
        var maxbets = new List<MaxbetModel>
        {
            new()
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=111
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                Value=111
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                Value=111
            }
        };
        var result = MaxbetHelper.FindMaxbetByBettype(maxbets, (int)EnumBettype.Handicap);
        Assert.Null(result);
    }

    #endregion maxbet

    [Fact]
    public void UpdateSpreadPEN_Always_Returns()
    {
        var spread = new MaxbetSpread
        {
            Handicap = 123,
            OverUnder = 123,
            FirstHalfHandicap = 123,
            FirstHalfOverUnder = 123
        };

        const decimal newHandicap = (decimal)0.2;

        const decimal newOverUnder = (decimal)0.2;

        MaxbetHelper.UpdateSpreadPEN(spread, newHandicap, newOverUnder);

        var expected = new MaxbetSpread
        {
            Handicap = (decimal)0.2,
            OverUnder = (decimal)0.2,
            FirstHalfHandicap = 123,
            FirstHalfOverUnder = 123
        };

        DeepAssert.Equal(expected, spread);
    }

    [Fact]
    public void UpdateMaxbetPEN_Always_Returns()
    {
        var maxbet = new MaxbetSpread
        {
            Handicap = 1000,
            OverUnder = 1000,
            FirstHalfHandicap = 123,
            FirstHalfOverUnder = 123
        };

        MaxbetHelper.UpdateMaxbetPEN(maxbet);

        var expected = new MaxbetSpread
        {
            Handicap = 500,
            OverUnder = 500,
            FirstHalfHandicap = 123,
            FirstHalfOverUnder = 123
        };

        DeepAssert.Equal(expected, maxbet);
    }

    [Fact]
    public void SetDefaultMaxbetInfo_Always_Returns()
    {
        var maxbet = new MaxbetSpread
        {
            Handicap = 1000,
            OverUnder = 1000,
            FirstHalfHandicap = 123,
            FirstHalfOverUnder = 123
        };

        var spread = new MaxbetSpread
        {
            Handicap = 123,
            OverUnder = 123,
            FirstHalfHandicap = 123,
            FirstHalfOverUnder = 123
        };

        var numberOfLines = new NumberOfLines
        {
            CreditFirstHalf = 1,
            CreditFt = 2,
            LicenseeFirstHalf = 3,
            LicenseeFt = 4
        };

        var maxbetInfo = new MaxbetInfo
        {
            Spread = spread,
            Maxbet = maxbet,
            NumberOfLines = numberOfLines
        };

        MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

        var expectedMaxbet = new MaxbetSpread
        {
            Handicap = 1000,
            OverUnder = 1000,
            FirstHalfHandicap = null,
            FirstHalfOverUnder = null
        };

        var expectedNumberOfLines = new NumberOfLines
        {
            CreditFirstHalf = 2,
            CreditFt = 2,
            LicenseeFirstHalf = 0,
            LicenseeFt = 0
        };

        DeepAssert.Equal(maxbetInfo.Maxbet, expectedMaxbet);
        DeepAssert.Equal(maxbetInfo.NumberOfLines, expectedNumberOfLines);
    }
}