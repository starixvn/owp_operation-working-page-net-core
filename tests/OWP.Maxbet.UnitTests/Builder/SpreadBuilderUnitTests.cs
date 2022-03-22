using NSubstitute;
using OWP.Enums;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Models;
using OWP.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder
{
  public class SpreadBuilderUnitTests
  {
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly SpreadBuilderImp spreadBuilder;

    private readonly LogServiceWrapper logServiceWrapper;

    public SpreadBuilderUnitTests()
    {
      logServiceWrapper = Substitute.For<LogServiceWrapper>();
      syncMaxbetRepository = Substitute.For<MaxbetRepository>();
      spreadBuilder = new SpreadBuilderImp(syncMaxbetRepository, logServiceWrapper);
    }

    [Fact]
    public async Task BuildSpread_NotFound_HDP_OU_1HHDP_1HOU_ReturnDefaultMaxbetSpread()
    {
      var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>
      {
        Item1 = new List<SpreadModel>(),
        Item2 = new List<LeagueGroup>()
      };

      syncMaxbetRepository
          .GetSpreadByOwpmatchId(123)
          .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

      var expectedResult = await spreadBuilder.BuildSpreadMainMatch(123);

      DeepAssert.Equal(expectedResult, new MaxbetSpread());
    }

    [Fact]
    public async Task BuildSpread_Found_HDP_OU_1HHDP_1HOU_ReturnMaxbetSpreadCorrect()
    {
      var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>
      {
        Item1 = new List<SpreadModel>
                {
                    new SpreadModel
                    {
                        Bettype= (int)EnumBettype.Handicap,
                        Spread=(decimal)0.1
                    },
                    new SpreadModel
                    {
                        Bettype= (int)EnumBettype.OverUnder,
                        Spread=(decimal)0.2
                    },
                     new SpreadModel
                    {
                        Bettype= (int)EnumBettype.FirstHalfHandicap,
                        Spread=(decimal)0.3
                    },
                    new SpreadModel
                    {
                        Bettype= (int)EnumBettype.FirstHalfOverUnder,
                        Spread=(decimal)0.4
                    }
                },
        Item2 = new List<LeagueGroup>
                {
                    new LeagueGroup
                    {
                        LeagueGroupId=1
                    }
                }
      };

      syncMaxbetRepository
          .GetSpreadByOwpmatchId(123)
          .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

      var result = await spreadBuilder.BuildSpreadMainMatch(123);

      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.1,
        OverUnder = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.3,
        FirstHalfOverUnder = (decimal)0.4
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildSpread_ForGroupMJ_NotExists_1HHDP_1HOU_ReturnSpreadHasSpread_1HHDP_1HOU_Follow_HDP_OU()
    {
      var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>
      {
        Item1 = new List<SpreadModel>
                {
                    new SpreadModel
                    {
                        Bettype= (int)EnumBettype.Handicap,
                        Spread=(decimal)0.1
                    },
                    new SpreadModel
                    {
                        Bettype= (int)EnumBettype.OverUnder,
                        Spread=(decimal)0.2
                    }
                },

        Item2 = new List<LeagueGroup> {
                    new LeagueGroup {
                        LeagueGroupId = 1
                    }
                }
      };

      syncMaxbetRepository
          .GetSpreadByOwpmatchId(123)
          .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

      var result = await spreadBuilder.BuildSpreadMainMatch(123);
      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.1,
        OverUnder = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.1,
        FirstHalfOverUnder = (decimal)0.2
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public async Task BuildSpread_ForGroupNotMJ_NotExists_1HHDP_1HOU_ReturnSpreadHasSpread_1HHDP_1HOU_Follow_HDP_OU()
    {
      var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>
      {
        Item1 = new List<SpreadModel>
                {
                    new SpreadModel
                    {
                        Bettype= (int)EnumBettype.Handicap,
                        Spread=(decimal)0.1
                    },
                    new SpreadModel
                    {
                        Bettype= (int)EnumBettype.OverUnder,
                        Spread=(decimal)0.1
                    }
                },

        Item2 = new List<LeagueGroup> {
                    new LeagueGroup {
                        LeagueGroupId = 2
                    }
                }
      };

      syncMaxbetRepository
          .GetSpreadByOwpmatchId(123)
          .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

      var result = await spreadBuilder.BuildSpreadMainMatch(123);
      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.1,
        OverUnder = (decimal)0.1,
        FirstHalfHandicap = (decimal)0.1,
        FirstHalfOverUnder = (decimal)0.1
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildMaxbetSpread_NotExistsHDP_OU_1HHDP_1HOU_ReturnEmptySpread()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel()
            };

      var actual = SpreadBuilderImp.BuildSpreadSender(spread);
      var expected = new MaxbetSpread();
      DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void BuildMaxbetSpread_ExistsHDP_OU_1HHDP_1HOU_ReturnSpread()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.1
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder,
                    Spread=(decimal)0.2
                },
                 new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfHandicap,
                    Spread=(decimal)0.3
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfOverUnder,
                    Spread=(decimal)0.4
                }
            };

      var actual = SpreadBuilderImp.BuildSpreadSender(spread);
      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.1,
        OverUnder = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.3,
        FirstHalfOverUnder = (decimal)0.4
      };
      DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupMJ_ListSpread_NotFoundHdp_ReturnMaxbetSpread_HasDefaultValueHdp()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder
                }
            };
      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupMJ(spread);
      DeepAssert.Equal(new MaxbetSpread(), result);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupMJ_ListSpread_NotFoundOU_ReturnMaxbetSpread_HasDefaultValueHdp()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap
                }
            };
      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupMJ(spread);
      DeepAssert.Equal(new MaxbetSpread(), result);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupNotAllowMJ_Exists_1HHDP_ReturnSpreadModelCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.1
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder,
                    Spread=(decimal)0.2
                },
                 new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfHandicap,
                    Spread=(decimal)0.3
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfOverUnder,
                    Spread=(decimal)0.4
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);

      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.1,
        OverUnder = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.3,
        FirstHalfOverUnder = (decimal)0.4
      };

      DeepAssert.Equal(expected, result);
    }

    // number=10%
    [Fact]
    public void BuildSpreadSenderForLeagueGroupNotAllowMJ_NotExists_1HHDP_And_HDP_HasSpread_Smaller_Specific_Number_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.01
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);

      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.01,
        FirstHalfHandicap = (decimal)0.1,
      };

      DeepAssert.Equal(expected, result);
    }

    // number=10%
    [Fact]
    public void BuildSpreadSenderForLeagueGroupNotAllowMJ_NotExists_1HHDP_And_HDP_HasSpread_Greater_Specific_Number_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.2
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);

      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.2,
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupNotAllowMJ_Exists_1HOU_ReturnSpreadModelCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.1
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder,
                    Spread=(decimal)0.2
                },
                 new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfHandicap,
                    Spread=(decimal)0.3
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfOverUnder,
                    Spread=(decimal)0.4
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);

      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.1,
        OverUnder = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.3,
        FirstHalfOverUnder = (decimal)0.4
      };

      DeepAssert.Equal(expected, result);
    }

    // number=10%
    [Fact]
    public void BuildSpreadSenderForLeagueGroupNotAllowMJ_NotExists_1HOU_And_OU_HasSpread_Smaller_Specific_Number_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder,
                    Spread=(decimal)0.01
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);

      var expected = new MaxbetSpread
      {
        OverUnder = (decimal)0.01,
        FirstHalfOverUnder = (decimal)0.1
      };

      DeepAssert.Equal(expected, result);
    }

    // number=10%
    [Fact]
    public void BuildSpreadSenderForLeagueGroupNotAllowMJ_NotExists_1HOU_And_OU_HasSpread_Greater_Specific_Number_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder,
                    Spread=(decimal)0.2
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);

      var expected = new MaxbetSpread
      {
        OverUnder = (decimal)0.2,
        FirstHalfOverUnder = (decimal)0.2
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupMJ_Exists_1HHDP_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.2
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfHandicap,
                    Spread=(decimal)0.1
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);
      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.1,
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupMJ_NotExists_1HHDP_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.2
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);
      var expected = new MaxbetSpread
      {
        Handicap = (decimal)0.2,
        FirstHalfHandicap = (decimal)0.2,
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupMJ_Exists_1HOU_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder,
                    Spread=(decimal)0.2
                },
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.FirstHalfOverUnder,
                    Spread=(decimal)0.1
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);
      var expected = new MaxbetSpread
      {
        OverUnder = (decimal)0.2,
        FirstHalfOverUnder = (decimal)0.1
      };

      DeepAssert.Equal(expected, result);
    }

    [Fact]
    public void BuildSpreadSenderForLeagueGroupMJ_NotExists_1HOU_ReturnSpreadCorrect()
    {
      var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    Bettype= (int)EnumBettype.OverUnder,
                    Spread=(decimal)0.2
                }
            };

      var result = SpreadBuilderImp.BuildSpreadSenderForLeagueGroupNotMJ(spread);
      var expected = new MaxbetSpread
      {
        OverUnder = (decimal)0.2,
        FirstHalfOverUnder = (decimal)0.2
      };
      DeepAssert.Equal(expected, result);
    }
  }
}