using NSubstitute;
using OWP.Enums;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder;

public class MaxbetBuilderUnitTests
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly MaxbetBuilderImp maxbetBuilder;

    public MaxbetBuilderUnitTests()
    {
        syncMaxbetRepository = Substitute.For<MaxbetRepository>();
        maxbetBuilder = new MaxbetBuilderImp(syncMaxbetRepository);
    }

    [Fact]
    public async Task BuildMaxbet_OwpmatchId_NotFoundMaxbet_ReturnDefaultmaxbetInfo()
    {
        var matchId = 123;
        IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>();
        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(matchId)
            .ReturnsForAnyArgs(Task.FromResult(maxbet));

        var result = await maxbetBuilder.BuildMaxbet(matchId);
        DeepAssert.Equal(result, new MaxbetSpread());
    }

    [Fact]
    public async Task BuildMaxbet_Found_HDP_OU_1HHDP_1HOU_ReturnMaxbetSpreadCorrect()
    {
        IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=1000
            },
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                Value=1000
            },
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                Value=1000
            }
        };

        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(123)
            .ReturnsForAnyArgs(Task.FromResult(maxbet));

        var result = await maxbetBuilder.BuildMaxbet(123);

        var expected = new MaxbetSpread
        {
            Handicap = 1000,
            OverUnder = 1000,
            FirstHalfHandicap = 1000,
            FirstHalfOverUnder = 1000
        };

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildMaxbetSpecialProduct_Found_MaxbetNoOFCorner_ReturnMaxbetSpreadCorrect()
    {
        IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>
        {
            new ()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=1000
            },
        };
        IEnumerable<MaxbetModel> maxBetMainMatch = new List<MaxbetModel>
        {
            new ()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=1000
            },
            new ()
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=1000
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                Value=1000
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                Value=1000
            }
        };
        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(1)
            .Returns(maxbet);
        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(2)
            .Returns(maxBetMainMatch);

        syncMaxbetRepository.GetMaxbetSettingDefault()
            .Returns(GetMaxbetSettingInfosRaw());

        var result = await maxbetBuilder
            .BuildMaxbetSpecialProduct(new MinorMatchInfo { MinorMatchId = 1, ParentMatchId = 2 }, -3);

        var expected = new MaxbetSpread
        {
            Handicap = 1000,
            OverUnder = 1000,
            FirstHalfHandicap = 1000,
            FirstHalfOverUnder = 1000
        };

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildMaxbetSpecialProduct_Found_MaxbetNextCorners_ReturnMaxbetSpreadCorrect()
    {

        IEnumerable<MaxbetModel> maxBetMainMatch = new List<MaxbetModel>
        {
            new ()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=1000
            },

        };

        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(2)
            .Returns(maxBetMainMatch);

        syncMaxbetRepository.GetMaxbetSettingDefault()
            .Returns(GetMaxbetSettingInfosRaw());

        var result = await maxbetBuilder
            .BuildMaxbetSpecialProduct(new MinorMatchInfo { MinorMatchId = 1, ParentMatchId = 2 }, -4);

        var expected = new MaxbetSpread
        {
            Handicap = 1000,

        };

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildMaxbetSpecialProduct_Found_MaxbetNextBooking_ReturnMaxbetSpreadCorrect()
    {

        IEnumerable<MaxbetModel> maxBetMainMatch = new List<MaxbetModel>
        {
            new ()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=1000
            },

        };

        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(2)
            .Returns(maxBetMainMatch);

        syncMaxbetRepository.GetMaxbetSettingDefault()
            .Returns(GetMaxbetSettingInfosRaw());

        var result = await maxbetBuilder
            .BuildMaxbetSpecialProduct(new MinorMatchInfo { MinorMatchId = 1, ParentMatchId = 2 }, -7);

        var expected = new MaxbetSpread
        {
            Handicap = 1000,

        };

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildMaxbetSpecialProduct_Found_MaxbetTotalBooking_ReturnMaxbetSpreadCorrect()
    {

        IEnumerable<MaxbetModel> maxBetMainMatch = new List<MaxbetModel>
        {
            new ()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=1000
            },

        };

        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(2)
            .Returns(maxBetMainMatch);

        syncMaxbetRepository.GetMaxbetSettingDefault()
            .Returns(GetMaxbetSettingInfosRaw());

        var result = await maxbetBuilder
            .BuildMaxbetSpecialProduct(new MinorMatchInfo { MinorMatchId = 1, ParentMatchId = 2 }, -6);

        var expected = new MaxbetSpread
        {
            Handicap = 1000,
            OverUnder = 1000,
            FirstHalfHandicap = 1000,
            FirstHalfOverUnder = 1000,
        };

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildMaxbetSpecialProduct_Found_OverUnder_ReturnMaxbetSpreadCorrect()
    {

        IEnumerable<MaxbetModel> maxBetMainMatch = new List<MaxbetModel>
        {
            new ()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=1000
            },

        };

        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(2)
            .Returns(maxBetMainMatch);

        syncMaxbetRepository.GetMaxbetSettingDefault()
            .Returns(GetMaxbetSettingInfosRaw());

        var result = await maxbetBuilder
            .BuildMaxbetSpecialProduct(new MinorMatchInfo { MinorMatchId = 1, ParentMatchId = 2 }, -7);

        var expected = new MaxbetSpread
        {
            Handicap = 1000,

        };

        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildMaxbetSender_ListMaxbetEmpty_ReturnMaxbetSenderDefault()
    {
        var maxbet = new List<MaxbetModel>();
        var result = MaxbetBuilderImp.BuildMaxbetSender(maxbet);
        DeepAssert.Equal(new MaxbetSpread(), result);
    }

    [Fact]
    public void BuildMaxbetSender_ListMaxbet_HasFull_HDP_OU_1HHDP_1HOU_ReturnMaxbetSenderFullBettype()
    {
        var maxbet = new List<MaxbetModel>
        {
            new ()
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=10000
            },
            new ()
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=10000
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                Value=10000
            },
            new ()
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                Value=10000
            }
        };

        var result = MaxbetBuilderImp.BuildMaxbetSender(maxbet);
        var expected = new MaxbetSpread
        {
            Handicap = 10000,
            OverUnder = 10000,
            FirstHalfOverUnder = 10000,
            FirstHalfHandicap = 10000
        };
        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildMaxbet_FirstHalfOverUnder_NotFound_ReturnMaxbetSpreadIncludeFirstHaflOverUnder()
    {
        var matchId = 123;
        IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=400000
            },
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=400000
            }
        };
        syncMaxbetRepository.GetMaxbetByOwpMatchId(Arg.Any<int>()).Returns(maxbet);
        syncMaxbetRepository.GetMaxbetSettingDefault().Returns(GetMaxbetSettingInfosRaw());

        var result = await maxbetBuilder.BuildMaxbet(matchId);
        var expected = new MaxbetSpread
        {
            Handicap = 400000,
            OverUnder = 400000,
            FirstHalfHandicap = 100000,
            FirstHalfOverUnder = 120000
        };
        DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildMaxbetFirstHalfHandicap_Found_HDP_InMaxbetMapper_AddMaxbet1HHDPToListSuccess()
    {
        var maxbet = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=550000
            }
        };

        maxbetBuilder.BuildMaxbetFirstHalfHandicap(maxbet, GetMaxbetSettingInfosRaw().ToList());

        var expected = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=550000
            },
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                Value=550000
            }
        };
        DeepAssert.Equal(expected, maxbet);
    }

    [Fact]
    public void BuildMaxbetFirstHalfHandicap_NotFound_HDP_InMaxbetMapper_AddMaxbet1HHDPToListSuccess()
    {
        var maxbet = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=400
            }
        };

        maxbetBuilder.BuildMaxbetFirstHalfHandicap(maxbet, new List<MaxbetSetting>());

        var expected = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=400
            },
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
            }
        };
        DeepAssert.Equal(expected, maxbet);
    }

    [Fact]
    public void BuildMaxbetFirstHalfOverUnder_Found_HDP_InMaxbetMapper_AddMaxbet1HOUToListSuccess()
    {
        var maxbet = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=400000
            }
        };

        maxbetBuilder.BuildMaxbetFirstHalfOverUnder(maxbet, GetMaxbetSettingInfosRaw().ToList());

        var expected = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.Handicap,
                Value=400000
            },
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                Value=120000
            }
        };
        DeepAssert.Equal(expected, maxbet);
    }

    [Fact]
    public void BuildMaxbetFirstHalfOverUnder_NotFound_HDP_InMaxbetMapper_AddMaxbet1HOUToListSuccess()
    {
        var maxbet = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=400
            }
        };

        maxbetBuilder.BuildMaxbetFirstHalfOverUnder(maxbet, new List<MaxbetSetting>());

        var expected = new List<MaxbetModel>
        {
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.OverUnder,
                Value=400
            },
            new MaxbetModel
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
            }
        };
        DeepAssert.Equal(expected, maxbet);
    }

    private IEnumerable<MaxbetSetting> GetMaxbetSettingInfosRaw()
        => new List<MaxbetSetting>
        {
            new()
            {
                FT_HDP   = 400_000,
                FirstHalf_HDP = 100_000,
                FirstHalf_OU = 120_000
            }
        };
}